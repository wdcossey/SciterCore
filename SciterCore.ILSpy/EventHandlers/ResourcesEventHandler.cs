using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Metadata;
using SciterCore.ILSpy.Languages;
using SciterCore.Interop;

namespace SciterCore.ILSpy.EventHandlers
{
    public class ResourcesEventHandler : RootEventHandler<object, PEFile>
    {
        public ResourcesEventHandler(object parent, PEFile value)
            : base(parent, value)
        {

        }

        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            //EnsureLazyChildren();
            //foreach (ILSpyTreeNode child in this.Children)
            //{
            //    child.Decompile(language, output, options);
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
                child.SetAttribute("tooltip", $"from: {this.GetType().Name}, to: {o?.Handler?.Name}");

                child.SetState(SciterXDom.ELEMENT_STATE_BITS.STATE_COLLAPSED, 0, false);

                parent.Append(child);

                if (o.Handler != null && o.Handler is Type)
                {
                    var eventHandler = Activator.CreateInstance(o.Handler, args: new object[] { o, o.Value });

                    child.AttachEventHandler(eventHandler);
                }

                var text = child.Append("text", o.Text);

            }

            return true;
        }

        protected override string Title => "Resources";

        protected override string Suffix => null;

        protected override string Image => "resource";


        protected IEnumerable<dynamic> LoadChildren()
        {
            var result = new List<dynamic>();

            foreach (Resource r in Value.Resources.OrderBy(m => m.Name, NaturalStringComparer.Instance))
            {
                result.Add(
                    new
                    {
                        Handler = (SciterEventHandler)null,
                        Value = r,
                        Text = r.Name,
                    });

                //new StreamReader(r.TryOpenStream()).ReadToEnd()

                //this.Children.Add(ResourceTreeNode.Create(r));
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