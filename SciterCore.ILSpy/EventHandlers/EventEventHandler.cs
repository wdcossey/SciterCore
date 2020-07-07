using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.TypeSystem;
using SciterCore.ILSpy.Languages;

namespace SciterCore.ILSpy.EventHandlers
{
    public class EventEventHandler : RootEventHandler<object, IEvent>
    {
        public EventEventHandler(object parent, IEvent value)
            : base(parent, value)
        {

        }

        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            language.DecompileEvent(Value, output, options);
        }

        public override bool GetChildren(SciterElement parent, SciterValue[] args, out SciterValue result)
        {
            result = null;
            return true;
        }

        //public override object Text => GetText(EventDefinition, this.Language) + EventDefinition.MetadataToken.ToSuffixString();

        private static object GetText(IEvent ev, Language language)
        {
            return language.EventToString(ev, false, false, false);
        }

        protected override string Title => $"{GetText(Value, this.Language)}";

        protected override string Suffix => Value.MetadataToken.ToSuffixString();

        protected override string Image => "event";
            //return Images.GetIcon(MemberIcon.Event, MethodTreeNode.GetOverlayIcon(@event.Accessibility), @event.IsStatic);
        
        protected override void Attached(SciterElement element)
        {
            element?.SetAttribute("image", Image);
            var textElement = element?.Append("text", $"{Title} {Suffix}"?.Trim());
            textElement?.SetAttribute("suffix", Suffix);

            base.Attached(element);
        }
    }
}