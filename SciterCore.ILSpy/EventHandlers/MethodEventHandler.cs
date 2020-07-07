using System;
using System.Reflection.Metadata;
using System.Text;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.TypeSystem;
using SciterCore.ILSpy.Languages;

namespace SciterCore.ILSpy.EventHandlers
{
    public class MethodEventHandler : RootEventHandler<object, IMethod>
    {
        public MethodEventHandler(object parent, IMethod value)
            : base(parent, value)
        {

        }

        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            language.DecompileMethod(Value, output, options);
        }

        public override bool GetChildren(SciterElement parent, SciterValue[] args, out SciterValue result)
        {
            result = null;
            return true;
        }

        private static object GetText(IMethod method, Language language)
        {
            return language.MethodToString(method, false, false, false);
        }

        protected override string Title => $"{GetText(Value, Language)}";

        protected override string Suffix => Value.MetadataToken.ToSuffixString();

        protected override string Image => GetIcon();

        private string GetIcon()
        {
            if (Value.IsOperator)
                return "operator";
            //return Images.GetIcon(MemberIcon.Operator, GetOverlayIcon(Value.Accessibility), false);

            if (Value.IsExtensionMethod)
                return "extensionMethod";
            //return Images.GetIcon(MemberIcon.ExtensionMethod, GetOverlayIcon(Value.Accessibility), false);

            if (Value.IsConstructor)
                return "constructor";
            //return Images.GetIcon(MemberIcon.Constructor, GetOverlayIcon(Value.Accessibility), Value.IsStatic);

            if (!Value.HasBody && Value.HasAttribute(KnownAttribute.DllImport))
                return "pInvokeMethod";
            //return Images.GetIcon(MemberIcon.PInvokeMethod, GetOverlayIcon(Value.Accessibility), true);

            return Value.IsVirtual ? "virtualMethod" : "method";
            //Images.GetIcon(Value.IsVirtual ? MemberIcon.VirtualMethod : MemberIcon.Method,
            //GetOverlayIcon(Value.Accessibility), Value.IsStatic);
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