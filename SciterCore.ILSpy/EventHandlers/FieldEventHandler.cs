using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.TypeSystem;
using SciterCore.ILSpy.Languages;

namespace SciterCore.ILSpy.EventHandlers
{
    public class FieldEventHandler : RootEventHandler<object, IField>
    {
        public FieldEventHandler(object parent, IField value) 
            : base(parent, value)
        {

        }

        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            language.DecompileField(Value, output, options);
        }

        public override bool GetChildren(SciterElement parent, SciterValue[] args, out SciterValue result)
        {
            result = null;
            return true;
        }

        private static object GetText(IField field, Language language)
        {
            return language.FieldToString(field, includeDeclaringTypeName: false, includeNamespace: false, includeNamespaceOfDeclaringTypeName: false);
        }

        protected override string Title => $"{GetText(Value, Language)}";

        protected override string Suffix => Value.MetadataToken.ToSuffixString();

        protected override string Image => GetIcon();

        private string GetIcon()
        {
            if (Value.DeclaringType.Kind == TypeKind.Enum && Value.ReturnType.Kind == TypeKind.Enum)
                return "enumValue";
            //return Images.GetIcon(MemberIcon.EnumValue, MethodTreeNode.GetOverlayIcon(field.Accessibility), false);

            if (Value.IsConst)
                return "literal";
            //return Images.GetIcon(MemberIcon.Literal, MethodTreeNode.GetOverlayIcon(field.Accessibility), false);

            if (Value.IsReadOnly)
                return "fieldReadOnly";
            //return Images.GetIcon(MemberIcon.FieldReadOnly, MethodTreeNode.GetOverlayIcon(field.Accessibility), field.IsStatic);

            return
                "field"; //Images.GetIcon(MemberIcon.Field, MethodTreeNode.GetOverlayIcon(field.Accessibility), field.IsStatic);
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