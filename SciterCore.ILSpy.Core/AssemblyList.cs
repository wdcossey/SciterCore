using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SciterCore.ILSpy
{
	public sealed class AssemblyList
	{
		readonly string listName;

		/// <summary>Dirty flag, used to mark modifications so that the list is saved later</summary>
		bool dirty;

		internal readonly ConcurrentDictionary<(string assemblyName, bool isWinRT), LoadedAssembly> assemblyLookupCache = new ConcurrentDictionary<(string assemblyName, bool isWinRT), LoadedAssembly>();
		internal readonly ConcurrentDictionary<string, LoadedAssembly> moduleLookupCache = new ConcurrentDictionary<string, LoadedAssembly>();

		/// <summary>
		/// The assemblies in this list.
		/// Needs locking for multi-threaded access!
		/// Write accesses are allowed on the GUI thread only (but still need locking!)
		/// </summary>
		/// <remarks>
		/// Technically read accesses need locking when done on non-GUI threads... but whenever possible, use the
		/// thread-safe <see cref="GetAssemblies()"/> method.
		/// </remarks>
		internal readonly ObservableCollection<LoadedAssembly> assemblies = new ObservableCollection<LoadedAssembly>();

		public AssemblyList(string listName)
		{
			this.listName = listName;
			assemblies.CollectionChanged += Assemblies_CollectionChanged;
		}

		/// <summary>
		/// Loads an assembly list from XML.
		/// </summary>
		public AssemblyList(XElement listElement)
			: this((string)listElement.Attribute("name"))
		{
			foreach (var asm in listElement.Elements("Assembly"))
			{
				OpenAssembly((string)asm);
			}
			this.dirty = false; // OpenAssembly() sets dirty, so reset it afterwards
		}

		/// <summary>
		/// Gets the loaded assemblies. This method is thread-safe.
		/// </summary>
		public LoadedAssembly[] GetAssemblies()
		{
			lock (assemblies)
			{
				return assemblies.ToArray();
			}
		}

		/// <summary>
		/// Saves this assembly list to XML.
		/// </summary>
		internal XElement SaveAsXml()
		{
			return new XElement(
				"List",
				new XAttribute("name", this.ListName),
				assemblies.Where(asm => !asm.IsAutoLoaded).Select(asm => new XElement("Assembly", asm.FileName))
			);
		}

		/// <summary>
		/// Gets the name of this list.
		/// </summary>
		public string ListName
		{
			get { return listName; }
		}

		void Assemblies_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ClearCache();
			// Whenever the assembly list is modified, mark it as dirty
			// and enqueue a task that saves it once the UI has finished modifying the assembly list.
			if (!dirty)
			{
				dirty = true;
                //TODO: wdcossey
                ////Dispatcher.UIThread.InvokeAsync(
				//	new Action(
				//		delegate {
				dirty = false;
				AssemblyListManager.SaveList(this);
				//			ClearCache();
				//		}),
				//	DispatcherPriority.Background
				//);
			}
		}

		internal void RefreshSave()
		{
			if (!dirty)
			{
				dirty = true;
                //TODO: wdcossey
				//Dispatcher.UIThread.InvokeAsync(
				//	new Action(
				//		delegate {
				dirty = false;
				AssemblyListManager.SaveList(this);
				//		}),
				//	DispatcherPriority.Background
				//);
			}
		}

		internal void ClearCache()
		{
			assemblyLookupCache.Clear();
		}

		public LoadedAssembly Open(string assemblyUri, bool isAutoLoaded = false)
		{
			if (assemblyUri.StartsWith("nupkg://", StringComparison.OrdinalIgnoreCase))
			{
				string fileName = assemblyUri.Substring("nupkg://".Length);
				int separator = fileName.LastIndexOf(';');
				string componentName = null;
				if (separator > -1)
				{
					componentName = fileName.Substring(separator + 1);
					fileName = fileName.Substring(0, separator);
					LoadedNugetPackage package = new LoadedNugetPackage(fileName);
					var entry = package.Entries.FirstOrDefault(e => e.Name == componentName);
					if (entry != null)
					{
						return OpenAssembly(assemblyUri, entry.Stream, true);
					}
				}
				return null;
			}
			else
			{
				return OpenAssembly(assemblyUri, isAutoLoaded);
			}
		}

		/// <summary>
		/// Opens an assembly from disk.
		/// Returns the existing assembly node if it is already loaded.
		/// </summary>
		public LoadedAssembly OpenAssembly(string file, bool isAutoLoaded = false)
		{
            //TODO: wdcossey
			//Dispatcher.UIThread.VerifyAccess();

			file = Path.GetFullPath(file);

			foreach (LoadedAssembly asm in this.assemblies)
			{
				if (file.Equals(asm.FileName, StringComparison.OrdinalIgnoreCase))
					return asm;
			}

			var newAsm = new LoadedAssembly(this, file);
			newAsm.IsAutoLoaded = isAutoLoaded;
			lock (assemblies)
			{
				this.assemblies.Add(newAsm);
			}
			return newAsm;
		}

		/// <summary>
		/// Opens an assembly from a stream.
		/// </summary>
		public LoadedAssembly OpenAssembly(string file, Stream stream, bool isAutoLoaded = false)
		{
            //TODO: wdcossey
			//Dispatcher.UIThread.VerifyAccess();

			foreach (LoadedAssembly asm in this.assemblies)
			{
				if (file.Equals(asm.FileName, StringComparison.OrdinalIgnoreCase))
					return asm;
			}

			var newAsm = new LoadedAssembly(this, file, stream);
			newAsm.IsAutoLoaded = isAutoLoaded;
			lock (assemblies)
			{
				this.assemblies.Add(newAsm);
			}
			return newAsm;
		}

		/// <summary>
		/// Replace the assembly object model from a crafted stream, without disk I/O
		/// Returns null if it is not already loaded.
		/// </summary>
		public LoadedAssembly HotReplaceAssembly(string file, Stream stream)
		{
            //TODO: wdcossey
			//Dispatcher.UIThread.VerifyAccess();
			file = Path.GetFullPath(file);

			var target = this.assemblies.FirstOrDefault(asm => file.Equals(asm.FileName, StringComparison.OrdinalIgnoreCase));
			if (target == null)
				return null;

			var index = this.assemblies.IndexOf(target);
			var newAsm = new LoadedAssembly(this, file, stream);
			newAsm.IsAutoLoaded = target.IsAutoLoaded;
			lock (assemblies)
			{
				this.assemblies.Remove(target);
				this.assemblies.Insert(index, newAsm);
			}
			return newAsm;
		}

		public LoadedAssembly ReloadAssembly(string file)
		{
            //TODO: wdcossey
			//Dispatcher.UIThread.VerifyAccess();
			file = Path.GetFullPath(file);

			var target = this.assemblies.FirstOrDefault(asm => file.Equals(asm.FileName, StringComparison.OrdinalIgnoreCase));
			if (target == null)
				return null;

			var index = this.assemblies.IndexOf(target);
			var newAsm = new LoadedAssembly(this, file);
			newAsm.IsAutoLoaded = target.IsAutoLoaded;
			lock (assemblies)
			{
				this.assemblies.Remove(target);
				this.assemblies.Insert(index, newAsm);
			}
			return newAsm;
		}

		public void Unload(LoadedAssembly assembly)
		{
            //TODO: wdcossey
			//Dispatcher.UIThread.VerifyAccess();
			lock (assemblies)
			{
				assemblies.Remove(assembly);
			}
			RequestGC();
		}

		static bool gcRequested;

		void RequestGC()
		{
			if (gcRequested) return;
			gcRequested = true;
            //TODO: wdcossey
			//Dispatcher.UIThread.InvokeAsync(new Action(
			//	delegate {
					gcRequested = false;
					GC.Collect();
			//	}), DispatcherPriority.ContextIdle);
		}

		public void Sort(IComparer<LoadedAssembly> comparer)
		{
			Sort(0, int.MaxValue, comparer);
		}

		public void Sort(int index, int count, IComparer<LoadedAssembly> comparer)
		{
            //TODO: wdcossey
			//Dispatcher.UIThread.VerifyAccess();
			lock (assemblies)
			{
				List<LoadedAssembly> list = new List<LoadedAssembly>(assemblies);
				list.Sort(index, Math.Min(count, list.Count - index), comparer);
				assemblies.Clear();
				assemblies.AddRange(list);
			}
		}
	}
}
