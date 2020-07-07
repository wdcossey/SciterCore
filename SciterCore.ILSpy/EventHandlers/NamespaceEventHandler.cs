using System;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.TypeSystem;
using SciterCore.ILSpy.Languages;
using SciterCore.Interop;

namespace SciterCore.ILSpy.EventHandlers
{
    public class NamespaceEventHandler : RootEventHandler<object, ITypeDefinition>
    {
        public NamespaceEventHandler(object parent, ITypeDefinition value)
            : base(parent, value)
        {

        }


        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            //language.DecompileNamespace(Value.Namespace, ((dynamic)Parent).Children.OfType<TypeEventHandler>().Select(t => t.TypeDefinition), output, options);
        }

        public override bool GetChildren(SciterElement parent, SciterValue[] args, out SciterValue result)
        {

            result = null;

            foreach (var child in ((dynamic)Parent).Children)
            {
                var root = SciterElement.Create("option");

                root.SetAttribute("tooltip", $"from: {this.GetType().Name}, to: {child.Handler.Name}");
                //root.SetAttribute("image", child?.Image);

                root.SetState(SciterXDom.ELEMENT_STATE_BITS.STATE_COLLAPSED, 0, false);

                parent.Append(root);

                var eventHandler = Activator.CreateInstance(child.Handler, args: new object[] { child, child.Value });

                root.AttachEventHandler(eventHandler);

                //root.Append("text", child.Text);
            }


            return true;
        }

        protected override string Title
        {
            get
            {
                var escapedNamespace = Language.EscapeName(Value.Namespace);
                return string.IsNullOrWhiteSpace(escapedNamespace) ? "-" : escapedNamespace;
            }
        }

        protected override string Suffix => null;

        protected override string Image => "namespace";

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