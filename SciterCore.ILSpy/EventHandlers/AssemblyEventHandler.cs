using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;
using SciterCore.ILSpy.Languages;
using SciterCore.Interop;

namespace SciterCore.ILSpy.EventHandlers
{
    public class AssemblyEventHandler : RootEventHandler<object, LoadedAssembly>
    {
        readonly Dictionary<string, dynamic> namespaces = new Dictionary<string, dynamic>();

        public AssemblyEventHandler(object parent, LoadedAssembly value) 
            : base(parent, value)
        {

        }

        public bool GetIcon(SciterElement parent, SciterValue[] args, out SciterValue result)
        {
            result = null;


            return false;
        }

        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            
        }

        public override bool GetChildren(SciterElement parent, SciterValue[] args, out SciterValue result)
        {
            var children = LoadChildren();

            result = null;// SciterValue.FromJsonString(value);

            foreach (var o in children)
            {
                var child = SciterElement.Create("option");

                child.SetAttribute("tooltip", nameof(AssemblyEventHandler));

                //child.SetAttribute("version", assembly?.Version?.ToString());
                //child.SetAttribute("isAutoLoaded", assembly?.IsAutoLoaded);

                child.SetState(SciterXDom.ELEMENT_STATE_BITS.STATE_COLLAPSED, 0, false);

                parent.Append(child);

                if (o.Handler != null && o.Handler is Type)
                {
                    var eventHandler = Activator.CreateInstance(o.Handler, args: new object[] { o, o.Value });

                    child.AttachEventHandler(eventHandler /*new AssemblyEventHandler(peFile: o.Module)*/);
                }

                //TODO: Remove this!!!
                //var text = child.Append("text", $"{o.Text}");

                //text.SetStyle("display", "inline");
                //text.SetStyle("overflow", "hidden");
                //text.SetAttribute("version", assembly.Version);

                //var tag = text.Append("text", "test");
            }

            return true;
        }

        protected override string Title => Value.Text;

        protected override string Suffix => null;

        protected override string Image => "assembly";

        protected IEnumerable<dynamic> LoadChildren()
        {
            var result = new List<dynamic>();

            var module = Value.GetPEFileOrNull();
            if (module == null)
            {
                // if we crashed on loading, then we don't have any children
                return null;
            }

            var typeSystem = Value.GetTypeSystemOrNull();

            var assembly = (MetadataModule)typeSystem.MainModule;
            var metadata = module.Metadata;

            //this.Children.Add(new ReferenceFolderTreeNode(module, this));
            result.Add(
                new
                {
                    Handler = typeof(ReferencesEventHandler),
                    Value = (module, Value),
                    Children = new List<dynamic>()
                });

            if (module.Resources.Any())
            {
                result.Add(
                    new
                    {
                        Handler = typeof(ResourcesEventHandler),
                        Value = module,
                        //module.Version,
                        //module.ShortName,
                        module.FileName,
                        //module.HasLoadError,
                        //module.IsAutoLoaded,
                        //module.IsLoaded,
                        Children = new List<dynamic>()
                    });

                //    this.Children.Add(new ResourceListTreeNode(module));
            }

            foreach (var ns in namespaces.Values)
            {
                ns.Children.Clear();
            }
            foreach (var type in assembly.TopLevelTypeDefinitions.OrderBy(t => t.ReflectionName, NaturalStringComparer.Instance))
            {
                if (!namespaces.TryGetValue(type.Namespace, out var ns))
                {
                    //ns = new NamespaceTreeNode(escapedNamespace);
                    ns = new
                    {
                        Handler = typeof(NamespaceEventHandler),// (SciterEventHandler)null,//typeof(TypeEventHandler),
                        Value = type,
                        //module.Version,
                        //module.ShortName,
                        module.FileName,
                        //module.HasLoadError,
                        //module.IsAutoLoaded,
                        //module.IsLoaded,
                        Children = new List<dynamic>()
                    };
                    namespaces.Add(type.Namespace, ns);
                }

                //    TypeTreeNode node = new TypeTreeNode(type, this);
                //    typeDict[(TypeDefinitionHandle)type.MetadataToken] = node;
                //     ns.Children.Add(node);

                ns.Children.Add(new
                {
                    Handler = typeof(TypeEventHandler),
                    Value = (module, type),
                    //module.Version,
                    //Text = $"{TypeToString(type)}{type.MetadataToken.ToSuffixString()}",
                    //module.ShortName,
                    module.FileName,
                    //module.HasLoadError,
                    //module.IsAutoLoaded,
                    //module.IsLoaded,
                });

            }
            foreach (var ns in namespaces.Values/*.OrderBy(n => n.Name, NaturalStringComparer.Instance)*/)
            {
                //if (ns.Children.Count > 0)
                //this.Children.Add(ns);
                result.Add(ns);
            }

            return result;
        }

        private static string TypeToString(IType type, bool includeNamespace = false)
        {
            var visitor = new TypeToStringVisitor(includeNamespace);
            type.AcceptVisitor(visitor);
            return visitor.ToString();
        }

        public static string GetPlatformDisplayName(PEFile module)
        {
            var headers = module.Reader.PEHeaders;
            var architecture = headers.CoffHeader.Machine;
            var characteristics = headers.CoffHeader.Characteristics;
            var corflags = headers.CorHeader.Flags;
            switch (architecture)
            {
                case Machine.I386:
                    if ((corflags & CorFlags.Prefers32Bit) != 0)
                        return "AnyCPU (32-bit preferred)";
                    if ((corflags & CorFlags.Requires32Bit) != 0)
                        return "x86";
                    // According to ECMA-335, II.25.3.3.1 CorFlags.Requires32Bit and Characteristics.Bit32Machine must be in sync
                    // for assemblies containing managed code. However, this is not true for C++/CLI assemblies.
                    if ((corflags & CorFlags.ILOnly) == 0 && (characteristics & Characteristics.Bit32Machine) != 0)
                        return "x86";
                    return "AnyCPU (64-bit preferred)";
                case Machine.Amd64:
                    return "x64";
                case Machine.IA64:
                    return "Itanium";
                default:
                    return architecture.ToString();
            }
        }

        protected override void Attached(SciterElement element)
        {
            element?.SetAttribute("image", Image);
            
            var text = element?.Append("text", Title);
            text?.SetAttribute("version", Value.Version);
            text?.SetAttribute("suffix", Suffix);

            var module = Value.GetPEFileOrNull();
            var metadata = module?.Metadata;

            text?.SetAttribute("tooltip", $"{metadata?.GetFullAssemblyName()}<br/>{Value.FileName}<br/>{GetPlatformDisplayName(module)}<br/>{module?.Metadata?.MetadataVersion}");


            base.Attached(element);
        }


        protected override void Subscription(SciterElement element, out SciterBehaviors.EVENT_GROUPS event_groups)
        {
            base.Subscription(element, out event_groups);
        }

        protected override bool OnScriptCall(SciterElement element, string name, SciterValue[] args, out SciterValue result)
        {
            return base.OnScriptCall(element, name, args, out result);
        }

        protected override bool OnMethodCall(SciterElement element, SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodID)
        {
            return base.OnMethodCall(element, methodID);
        }
    }
}