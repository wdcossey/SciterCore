using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Metadata;
using SciterCore.ILSpy.Languages;
using SciterCore.Interop;

namespace SciterCore.ILSpy.EventHandlers
{
    public class ReferencesEventHandler : RootEventHandler<object, (PEFile Module, LoadedAssembly ParentAssembly)>
    {
        public ReferencesEventHandler(object parent, (PEFile Module, LoadedAssembly ParentAssembly) value)
            : base(parent, value)
        {

        }

        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            //language.WriteCommentLine(output, $"Detected Target-Framework-Id: {Value.ParentAssembly.GetTargetFrameworkIdAsync().Result}");
            //Dispatcher.UIThread.InvokeAsync(new Action(EnsureLazyChildren), DispatcherPriority.Normal);
            //output.WriteLine();
            //language.WriteCommentLine(output, "Referenced assemblies (in metadata order):");
            //// Show metadata order of references
            //foreach (var node in this.Children.OfType<ILSpyTreeNode>())
            //    node.Decompile(language, output, options);

            //output.WriteLine();
            //output.WriteLine();
            //// Show full assembly load log:
            //language.WriteCommentLine(output, "Assembly load log including transitive references:");
            //var info = Value.ParentAssembly.LoadedAssemblyReferencesInfo;
            //foreach (var asm in info.Entries)
            //{
            //    language.WriteCommentLine(output, asm.FullName);
            //    output.Indent();
            //    foreach (var item in asm.Messages)
            //    {
            //        language.WriteCommentLine(output, $"{item.Item1}: {item.Item2}");
            //    }
            //    output.Unindent();
            //    output.WriteLine();
            //}
        }

        public override bool GetChildren(SciterElement parent, SciterValue[] args, out SciterValue result)
        {
            var children = LoadChildren();

            result = null;

            foreach (var o in children)
            {
                var child = SciterElement.Create("option");
                child.SetAttribute("tooltip", $"from: {this.GetType().Name}, to: {o.Handler.Name}");

                child.SetAttribute("image", o?.Image);

                child.SetState(SciterXDom.ELEMENT_STATE_BITS.STATE_COLLAPSED, 0, false);

                parent.Append(child);

                //child.AttachEventHandler(new AssemblyEventHandler(peFile: o.Module));

                var text = child.Append("text", o.Text);
            }

            return true;
        }

        protected override string Title => "References";

        protected override string Suffix => null;

        protected override string Image => "reference-folder";

        protected IEnumerable<dynamic> LoadChildren()
        {
            var result = new List<dynamic>();

            var metadata = Value.Module.Metadata;
            foreach (var r in Value.Module.AssemblyReferences.OrderBy(r => r.Name))
            {
                //this.Children.Add(new AssemblyReferenceTreeNode(r, parentAssembly));
                result.Add(
                    new
                    {
                        AssemblyReference = r,
                        Image = "assembly",
                        Text = $"{r.Name}{((System.Reflection.Metadata.EntityHandle)r.Handle).ToSuffixString()}"
                    });
            }

            foreach (var r in metadata.GetModuleReferences()
                .OrderBy(r => metadata.GetString(metadata.GetModuleReference(r).Name)))
            {

                //    this.Children.Add(new ModuleReferenceTreeNode(parentAssembly, r, metadata));
                if (r.IsNil)
                    continue;

                var handle = r;
                var reference = metadata.GetModuleReference(r);
                var moduleName = metadata.GetString(reference.Name);

                //foreach (var h in metadata.AssemblyFiles)
                //{
                //    var file = metadata.GetAssemblyFile(h);
                //    if (metadata.StringComparer.Equals(file.Name, moduleName))
                //    {
                //        this.file = file;
                //        this.fileHandle = h;
                //        this.containsMetadata = file.ContainsMetadata;
                //        break;
                //    }
                //}

                result.Add(
                    new
                    {
                        ModuleReferenceHandle = r,
                        Image = "library",
                        Text = $"{moduleName}{((EntityHandle)handle).ToSuffixString()}",
                    });
            }

            return result;
        }

        protected override void Attached(SciterElement element)
        {
            element?.SetAttribute("image", Image);
            var textElement = element?.Append("text", $"{Title} {Suffix}"?.Trim());
            textElement?.SetAttribute("suffix", Suffix);

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