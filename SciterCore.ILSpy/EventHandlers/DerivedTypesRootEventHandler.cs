using System.Collections.Generic;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;
using SciterCore.ILSpy.Languages;
using SciterCore.Interop;

namespace SciterCore.ILSpy.EventHandlers
{
    public class DerivedTypesRootEventHandler : RootEventHandler<object, (PEFile Module, ITypeDefinition TypeDefinition)>
    {

        public DerivedTypesRootEventHandler(object parent, (PEFile, ITypeDefinition) value) : base(parent, value)
        {
        }

        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            //threading.Decompile(language, output, options, EnsureLazyChildren);
        }

        public override bool GetChildren(SciterElement parent, SciterValue[] args, out SciterValue result)
        {
            result = null;

            var children = LoadChildren();

            result = null;

            foreach (var o in children)
            {
                var child = SciterElement.Create("option");
                child.SetAttribute("tooltip", $"from: {this.GetType().Name}, to: {o.Handler.Name}");

                child.SetState(SciterXDom.ELEMENT_STATE_BITS.STATE_COLLAPSED, 0, false);

                parent.Append(child);

                //if (o.Handler != null && o.Handler is Type)
                //{
                //    var eventHandler = Activator.CreateInstance(o.Handler, args: new object[] { o, o.Value });
                //    child.AttachEventHandler(eventHandler);
                //}

                var text = child.Append("text", o.Text);
            }

            return true;
        }

        protected override string Title => "Derived Types";

        protected override string Suffix => null;

        protected override string Image => null;

        private IEnumerable<dynamic> LoadChildren()
        {
            var result = new List<dynamic>();

            return result;
        }

        protected override void Attached(SciterElement element)
        {
            element?.SetAttribute("image", Image);
            var textElement = element?.Append("text", $"{Title} {Suffix}"?.Trim());
            textElement?.SetAttribute("suffix", Suffix);

            base.Attached(element);
        }
    }
}