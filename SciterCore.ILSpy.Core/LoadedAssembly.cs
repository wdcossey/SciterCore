﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.Decompiler.DebugInfo;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.PdbProvider.Cecil;
using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.Decompiler.TypeSystem.Implementation;
using SciterCore.ILSpy.DebugInfo;

namespace SciterCore.ILSpy
{
	/// <summary>
	/// Represents an assembly loaded into ILSpy.
	/// </summary>
	[DebuggerDisplay("[LoadedAssembly {shortName}]")]
	public sealed class LoadedAssembly
	{
		internal static readonly ConditionalWeakTable<PEFile, LoadedAssembly> loadedAssemblies = new ConditionalWeakTable<PEFile, LoadedAssembly>();

		readonly Task<PEFile> assemblyTask;
		readonly AssemblyList assemblyList;
		readonly string fileName;
		readonly string shortName;

		public LoadedAssembly(AssemblyList assemblyList, string fileName, Stream stream = null)
		{
			this.assemblyList = assemblyList ?? throw new ArgumentNullException(nameof(assemblyList));
			this.fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

			this.assemblyTask = stream != null || File.Exists(fileName) ?
								Task.Factory.StartNew(LoadAssembly, stream) : // requires that this.fileName is set
								Task.FromException<PEFile>(new FileNotFoundException("Assembly file not found", fileName));
			this.shortName = Path.GetFileNameWithoutExtension(fileName);
		}

		/// <summary>
		/// Returns a target framework identifier in the form '&lt;framework&gt;Version=v&lt;version&gt;'.
		/// Returns an empty string if no TargetFrameworkAttribute was found or the file doesn't contain an assembly header, i.e., is only a module.
		/// </summary>
		public async Task<string> GetTargetFrameworkIdAsync()
		{
			var assembly = await GetPEFileAsync().ConfigureAwait(false);
			return assembly.Reader.DetectTargetFrameworkId() ?? string.Empty;
		}

		public ReferenceLoadInfo LoadedAssemblyReferencesInfo { get; } = new ReferenceLoadInfo();

		IDebugInfoProvider debugInfoProvider;

		/// <summary>
		/// Gets the Cecil ModuleDefinition.
		/// </summary>
		public Task<PEFile> GetPEFileAsync()
		{
			return assemblyTask;
		}

		/// <summary>
		/// Gets the Cecil ModuleDefinition.
		/// Returns null in case of load errors.
		/// </summary>
		public PEFile GetPEFileOrNull()
		{
			try
			{
				if (IsLoaded && HasLoadError) return null;
				return GetPEFileAsync().Result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.TraceError(ex.ToString());
				return null;
			}
		}

		ICompilation typeSystem;

		/// <summary>
		/// Gets a type system containing all types from this assembly + primitve types from mscorlib.
		/// Returns null in case of load errors.
		/// </summary>
		/// <remarks>
		/// This is an uncached type system.
		/// </remarks>
		public ICompilation GetTypeSystemOrNull()
		{
			if (typeSystem != null)
				return typeSystem;
			var module = GetPEFileOrNull();
			if (module == null)
				return null;
			return typeSystem = new SimpleCompilation(
				module.WithOptions(TypeSystemOptions.Default | TypeSystemOptions.Uncached | TypeSystemOptions.KeepModifiers),
				MinimalCorlib.Instance);
		}

		public AssemblyList AssemblyList => assemblyList;

		public string FileName => fileName;

		public string ShortName => shortName;

		public string Text
		{
			get
			{
				if (IsLoaded && !HasLoadError)
				{
                    var version = Version;
                    return $"{ShortName} {((version == null) ? string.Empty : $"({version})")}";
				}
				else
				{
					return ShortName;
				}
			}
		}

        public Version Version
        {
            get
            {
                if (IsLoaded && !HasLoadError)
                {
                    var metadata = GetPEFileOrNull()?.Metadata;
                    if (metadata != null && metadata.IsAssembly)
                        return metadata.GetAssemblyDefinition().Version;
                }

                return (Version)null;
            }
        }

		public bool IsLoaded => assemblyTask.IsCompleted;

		public bool HasLoadError => assemblyTask.IsFaulted;

		public bool IsAutoLoaded { get; set; }

		PEFile LoadAssembly(object state)
		{
			MetadataReaderOptions options;
            //TODO: wdcossey
			//if (DecompilerSettingsPanel.CurrentDecompilerSettings.ApplyWindowsRuntimeProjections)
			//{
			options = MetadataReaderOptions.ApplyWindowsRuntimeProjections;
			//}
			//else
			//{
			//	options = MetadataReaderOptions.None;
			//}

			// runs on background thread
			PEFile module;

			if (state is Stream stream)
			{
				// Read the module from a precrafted stream
				module = new PEFile(fileName, stream, metadataOptions:  /*DecompilerSettingsPanel.CurrentDecompilerSettings.ApplyWindowsRuntimeProjections ?*/ MetadataReaderOptions.ApplyWindowsRuntimeProjections /*: MetadataReaderOptions.None*/);
			}
			else
			{
				// Read the module from disk (by default)
				stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				module = new PEFile(fileName, stream, PEStreamOptions.PrefetchEntireImage,
					metadataOptions: options);
			}

            //TODO: wdcossey
			//if (DecompilerSettingsPanel.CurrentDecompilerSettings.UseDebugSymbols)
			{
				try
				{
					LoadSymbols(module);
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
				catch (InvalidOperationException)
				{
					// ignore any errors during symbol loading
				}
			}
			lock (loadedAssemblies)
			{
				loadedAssemblies.Add(module, this);
			}
			return module;
		}

		void LoadSymbols(PEFile module)
		{
			try
			{
				var reader = module.Reader;
				// try to open portable pdb file/embedded pdb info:
				if (TryOpenPortablePdb(module, out var provider, out var pdbFileName))
				{
					debugInfoProvider = new PortableDebugInfoProvider(pdbFileName, provider);
				}
				else
				{
					// search for pdb in same directory as dll
					string pdbDirectory = Path.GetDirectoryName(fileName);
					pdbFileName = Path.Combine(pdbDirectory, Path.GetFileNameWithoutExtension(fileName) + ".pdb");
					if (File.Exists(pdbFileName))
					{
						debugInfoProvider = new MonoCecilDebugInfoProvider(module, pdbFileName);
						return;
					}

					// TODO: use symbol cache, get symbols from microsoft
				}
			}
			catch (Exception ex) when (ex is BadImageFormatException || ex is COMException)
			{
				// Ignore PDB load errors
			}
		}

		const string LegacyPDBPrefix = "Microsoft C/C++ MSF 7.00";
		byte[] buffer = new byte[LegacyPDBPrefix.Length];

		bool TryOpenPortablePdb(PEFile module, out MetadataReaderProvider provider, out string pdbFileName)
		{
			provider = null;
			pdbFileName = null;
			var reader = module.Reader;
			foreach (var entry in reader.ReadDebugDirectory())
			{
				if (entry.IsPortableCodeView)
				{
					return reader.TryOpenAssociatedPortablePdb(fileName, OpenStream, out provider, out pdbFileName);
				}
				if (entry.Type == DebugDirectoryEntryType.CodeView)
				{
					string pdbDirectory = Path.GetDirectoryName(fileName);
					pdbFileName = Path.Combine(pdbDirectory, Path.GetFileNameWithoutExtension(fileName) + ".pdb");
					if (File.Exists(pdbFileName))
					{
						Stream stream = OpenStream(pdbFileName);
						if (stream.Read(buffer, 0, buffer.Length) == LegacyPDBPrefix.Length
							&& System.Text.Encoding.ASCII.GetString(buffer) == LegacyPDBPrefix)
						{
							return false;
						}
						stream.Position = 0;
						provider = MetadataReaderProvider.FromPortablePdbStream(stream);
						return true;
					}
				}
			}
			return false;
		}

		Stream OpenStream(string fileName)
		{
			if (!File.Exists(fileName))
				return null;
			var memory = new MemoryStream();
			using (var stream = File.OpenRead(fileName))
				stream.CopyTo(memory);
			memory.Position = 0;
			return memory;
		}

		[ThreadStatic]
		static int assemblyLoadDisableCount;

		public static IDisposable DisableAssemblyLoad()
		{
			assemblyLoadDisableCount++;
			return new DecrementAssemblyLoadDisableCount();
		}

		sealed class DecrementAssemblyLoadDisableCount : IDisposable
		{
			bool disposed;

			public void Dispose()
			{
				if (!disposed)
				{
					disposed = true;
					assemblyLoadDisableCount--;
					// clear the lookup cache since we might have stored the lookups failed due to DisableAssemblyLoad()
					//TODO: wdcossey
					//MainWindow.Instance.CurrentAssemblyList.ClearCache();
				}
			}
		}

		sealed class MyAssemblyResolver : IAssemblyResolver
		{
			readonly LoadedAssembly parent;

			public MyAssemblyResolver(LoadedAssembly parent)
			{
				this.parent = parent;
			}

			public PEFile Resolve(ICSharpCode.Decompiler.Metadata.IAssemblyReference reference)
			{
				return parent.LookupReferencedAssembly(reference)?.GetPEFileOrNull();
			}

			public PEFile ResolveModule(PEFile mainModule, string moduleName)
			{
				return parent.LookupReferencedModule(mainModule, moduleName)?.GetPEFileOrNull();
			}
		}

		public IAssemblyResolver GetAssemblyResolver()
		{
			return new MyAssemblyResolver(this);
		}

		/// <summary>
		/// Returns the debug info for this assembly. Returns null in case of load errors or no debug info is available.
		/// </summary>
		public IDebugInfoProvider GetDebugInfoOrNull()
		{
			if (GetPEFileOrNull() == null)
				return null;
			return debugInfoProvider;
		}

		public LoadedAssembly LookupReferencedAssembly(ICSharpCode.Decompiler.Metadata.IAssemblyReference reference)
		{
			if (reference == null)
				throw new ArgumentNullException(nameof(reference));
			if (reference.IsWindowsRuntime)
			{
				return assemblyList.assemblyLookupCache.GetOrAdd((reference.Name, true), key => LookupReferencedAssemblyInternal(reference, true));
			}
			else
			{
				return assemblyList.assemblyLookupCache.GetOrAdd((reference.FullName, false), key => LookupReferencedAssemblyInternal(reference, false));
			}
		}

		public LoadedAssembly LookupReferencedModule(PEFile mainModule, string moduleName)
		{
			if (mainModule == null)
				throw new ArgumentNullException(nameof(mainModule));
			if (moduleName == null)
				throw new ArgumentNullException(nameof(moduleName));
			return assemblyList.moduleLookupCache.GetOrAdd(mainModule.FileName + ";" + moduleName, _ => LookupReferencedModuleInternal(mainModule, moduleName));
		}

		class MyUniversalResolver : UniversalAssemblyResolver
		{
			public MyUniversalResolver(LoadedAssembly assembly)
				: base(assembly.FileName, false, assembly.GetTargetFrameworkIdAsync().Result, PEStreamOptions.PrefetchEntireImage, /*DecompilerSettingsPanel.CurrentDecompilerSettings.ApplyWindowsRuntimeProjections ?*/ MetadataReaderOptions.ApplyWindowsRuntimeProjections/* : MetadataReaderOptions.None*/)
			{
			}
		}

		static Dictionary<string, LoadedAssembly> loadingAssemblies = new Dictionary<string, LoadedAssembly>();

		LoadedAssembly LookupReferencedAssemblyInternal(ICSharpCode.Decompiler.Metadata.IAssemblyReference fullName, bool isWinRT)
		{
			string GetName(ICSharpCode.Decompiler.Metadata.IAssemblyReference name) => isWinRT ? name.Name : name.FullName;

			string file;
			LoadedAssembly asm;
			lock (loadingAssemblies)
			{
				foreach (LoadedAssembly loaded in assemblyList.GetAssemblies())
				{
					var reader = loaded.GetPEFileOrNull()?.Metadata;
					if (reader == null || !reader.IsAssembly) continue;
					var asmDef = reader.GetAssemblyDefinition();
					var asmDefName = isWinRT ? reader.GetString(asmDef.Name) : reader.GetFullAssemblyName();
					if (GetName(fullName).Equals(asmDefName, StringComparison.OrdinalIgnoreCase))
					{
						LoadedAssemblyReferencesInfo.AddMessageOnce(fullName.FullName, MessageKind.Info, "Success - Found in Assembly List");
						return loaded;
					}
				}

				var resolver = new MyUniversalResolver(this);
				file = resolver.FindAssemblyFile(fullName);

				foreach (LoadedAssembly loaded in assemblyList.GetAssemblies())
				{
					if (loaded.FileName.Equals(file, StringComparison.OrdinalIgnoreCase))
					{
						return loaded;
					}
				}

				if (file != null && loadingAssemblies.TryGetValue(file, out asm))
					return asm;

				if (assemblyLoadDisableCount > 0)
					return null;

				if (file != null)
				{
					LoadedAssemblyReferencesInfo.AddMessage(fullName.ToString(), MessageKind.Info, "Success - Loading from: " + file);
					asm = new LoadedAssembly(assemblyList, file) { IsAutoLoaded = true };
				}
				else
				{
					LoadedAssemblyReferencesInfo.AddMessageOnce(fullName.ToString(), MessageKind.Error, "Could not find reference: " + fullName);
					return null;
				}
				loadingAssemblies.Add(file, asm);
			}
            //TODO: wdcossey
			//Dispatcher.UIThread.InvokeAsync((Action)delegate () {
				lock (assemblyList.assemblies)
				{
					assemblyList.assemblies.Add(asm);
				}
				lock (loadingAssemblies)
				{
					loadingAssemblies.Remove(file);
				}
			//});
			return asm;
		}

		LoadedAssembly LookupReferencedModuleInternal(PEFile mainModule, string moduleName)
		{
			string file;
			LoadedAssembly asm;
			lock (loadingAssemblies)
			{
				foreach (LoadedAssembly loaded in assemblyList.GetAssemblies())
				{
					var reader = loaded.GetPEFileOrNull()?.Metadata;
					if (reader == null || reader.IsAssembly) continue;
					var moduleDef = reader.GetModuleDefinition();
					if (moduleName.Equals(reader.GetString(moduleDef.Name), StringComparison.OrdinalIgnoreCase))
					{
						LoadedAssemblyReferencesInfo.AddMessageOnce(moduleName, MessageKind.Info, "Success - Found in Assembly List");
						return loaded;
					}
				}

				file = Path.Combine(Path.GetDirectoryName(mainModule.FileName), moduleName);
				if (!File.Exists(file))
					return null;

				foreach (LoadedAssembly loaded in assemblyList.GetAssemblies())
				{
					if (loaded.FileName.Equals(file, StringComparison.OrdinalIgnoreCase))
					{
						return loaded;
					}
				}

				if (file != null && loadingAssemblies.TryGetValue(file, out asm))
					return asm;

				if (assemblyLoadDisableCount > 0)
					return null;

				if (file != null)
				{
					LoadedAssemblyReferencesInfo.AddMessage(moduleName, MessageKind.Info, "Success - Loading from: " + file);
					asm = new LoadedAssembly(assemblyList, file) { IsAutoLoaded = true };
				}
				else
				{
					LoadedAssemblyReferencesInfo.AddMessageOnce(moduleName, MessageKind.Error, "Could not find reference: " + moduleName);
					return null;
				}
				loadingAssemblies.Add(file, asm);
			}
            //TODO: wdcossey
			//Dispatcher.UIThread.InvokeAsync((Action)delegate () {
				lock (assemblyList.assemblies)
				{
					assemblyList.assemblies.Add(asm);
				}
				lock (loadingAssemblies)
				{
					loadingAssemblies.Remove(file);
				}
			//});
			return asm;
		}

		public Task ContinueWhenLoaded(Action<Task<PEFile>> onAssemblyLoaded, TaskScheduler taskScheduler)
		{
			return this.assemblyTask.ContinueWith(onAssemblyLoaded, default(CancellationToken), TaskContinuationOptions.RunContinuationsAsynchronously, taskScheduler);
		}

		/// <summary>
		/// Wait until the assembly is loaded.
		/// Throws an AggregateException when loading the assembly fails.
		/// </summary>
		public void WaitUntilLoaded()
		{
			assemblyTask.Wait();
		}

	}
}
