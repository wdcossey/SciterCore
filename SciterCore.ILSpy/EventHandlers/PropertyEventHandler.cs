using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.TypeSystem;
using SciterCore.ILSpy.Languages;
using SciterCore.Interop;

namespace SciterCore.ILSpy.EventHandlers
{
    public class PropertyEventHandler : RootEventHandler<object, IProperty>
    {

        public PropertyEventHandler(object parent, IProperty value)
            : base(parent, value)
        {

        }

        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            language.DecompileProperty(Value, output, options);
        }

        public override bool GetChildren(SciterElement parent, SciterValue[] args, out SciterValue result)
        {
            var children = LoadChildren();

            result = null;

            foreach (var o in children)
            {
                var child = SciterElement.Create("option");
                child.SetAttribute("tooltip", $"from: {this.GetType().Name}, to: {o.Handler.Name}");

                child.SetState(SciterXDom.ELEMENT_STATE_BITS.STATE_COLLAPSED, 0, false);

                parent.Append(child);

                if (o.Handler != null && o.Handler is Type)
                {
                    var eventHandler = Activator.CreateInstance(o.Handler, args: new object[] { o, o.Value });

                    child.AttachEventHandler(eventHandler);
                }

                //var text = child.Append("text", o.Text);
            }

            return true;
        }

        private static object GetText(IProperty property, Language language)
        {
            return language.PropertyToString(property, false, false, false);
        }

        protected override string Title => $"{GetText(Value, Language)}";

        protected override string Suffix => Value.MetadataToken.ToSuffixString();

        protected override string Image => (Value.IsIndexer ? "indexer" : "property");
        //  return  Images.GetIcon(property.IsIndexer ? MemberIcon.Indexer : MemberIcon.Property,
        //      MethodTreeNode.GetOverlayIcon(property.Accessibility), property.IsStatic);

        protected IEnumerable<dynamic> LoadChildren()
        {
            var result = new List<dynamic>();


            if (Value.CanGet)
            {
                result.Add(new
                {
                    Handler = typeof(MethodEventHandler),
                    Value = Value.Getter
                    //Text = $"{Value.Getter.Name}{Value.Getter.MetadataToken.ToSuffixString()}",
                });
                //this.Children.Add(new MethodTreeNode(property.Getter));
            }

            if (Value.CanSet)
            {
                result.Add(new
                {
                    Handler = typeof(MethodEventHandler),
                    Value = Value.Setter
                    //Text = $"{Value.Setter.Name}{Value.Setter.MetadataToken.ToSuffixString()}",
                });
                //this.Children.Add(new MethodTreeNode(property.Setter));
            }


            return result;
        }

        //private static string MethodToString(IMethod method, bool includeDeclaringTypeName, bool includeNamespace, bool includeNamespaceOfDeclaringTypeName)
        //{
        //    if (method == null)
        //        throw new ArgumentNullException(nameof(method));

        //    int i = 0;
        //    var buffer = new StringBuilder();
        //    buffer.Append(GetDisplayName(method, includeDeclaringTypeName, includeNamespace, includeNamespaceOfDeclaringTypeName));
        //    var typeParameters = method.TypeParameters;
        //    if (typeParameters.Count > 0)
        //    {
        //        buffer.Append("``");
        //        buffer.Append(typeParameters.Count);
        //        buffer.Append('<');
        //        foreach (var tp in typeParameters)
        //        {
        //            if (i > 0)
        //                buffer.Append(", ");
        //            buffer.Append(tp.Name);
        //            i++;
        //        }
        //        buffer.Append('>');
        //    }
        //    buffer.Append('(');

        //    i = 0;
        //    var parameters = method.Parameters;
        //    foreach (var param in parameters)
        //    {
        //        if (i > 0)
        //            buffer.Append(", ");
        //        buffer.Append(TypeToString(param.Type, includeNamespace));
        //        i++;
        //    }
        //    buffer.Append(')');
        //    if (!method.IsConstructor)
        //    {
        //        buffer.Append(" : ");
        //        buffer.Append(TypeToString(method.ReturnType, includeNamespace));
        //    }
        //    return buffer.ToString();
        //}

        //protected string GetDisplayName(IEntity entity, bool includeDeclaringTypeName, bool includeNamespace, bool includeNamespaceOfDeclaringTypeName)
        //{
        //    string entityName;
        //    if (entity is ITypeDefinition t && !t.MetadataToken.IsNil)
        //    {
        //        MetadataReader metadata = t.ParentModule.PEFile.Metadata;
        //        var typeDef = metadata.GetTypeDefinition((TypeDefinitionHandle)t.MetadataToken);
        //        entityName = EscapeName(metadata.GetString(typeDef.Name));
        //    }
        //    else
        //    {
        //        entityName = EscapeName(entity.Name);
        //    }
        //    if (includeNamespace || includeDeclaringTypeName)
        //    {
        //        if (entity.DeclaringTypeDefinition != null)
        //            return TypeToString(entity.DeclaringTypeDefinition, includeNamespaceOfDeclaringTypeName) + "." + entityName;
        //        return EscapeName(entity.Namespace) + "." + entityName;
        //    }
        //    else
        //    {
        //        return entityName;
        //    }
        //}

        protected override void Attached(SciterElement element)
        {
            element?.SetAttribute("image", Image);
            var textElement = element?.Append("text", $"{Title} {Suffix}"?.Trim());
            textElement?.SetAttribute("suffix", Suffix);

            base.Attached(element);
        }

        private static string PropertyToString(IProperty property, bool includeDeclaringTypeName = false, bool includeNamespace = false, bool includeNamespaceOfDeclaringTypeName = false)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            return GetDisplayName(property, includeDeclaringTypeName, includeNamespace, includeNamespaceOfDeclaringTypeName) + " : " + TypeToString(property.ReturnType, includeNamespace);
        }


        private static string GetDisplayName(IEntity entity, bool includeDeclaringTypeName = false, bool includeNamespace = false, bool includeNamespaceOfDeclaringTypeName = false)
        {
            string entityName;
            if (entity is ITypeDefinition t && !t.MetadataToken.IsNil)
            {
                MetadataReader metadata = t.ParentModule.PEFile.Metadata;
                var typeDef = metadata.GetTypeDefinition((TypeDefinitionHandle)t.MetadataToken);
                entityName = EscapeName(metadata.GetString(typeDef.Name));
            }
            else
            {
                entityName = EscapeName(entity.Name);
            }
            if (includeNamespace || includeDeclaringTypeName)
            {
                if (entity.DeclaringTypeDefinition != null)
                    return TypeToString(entity.DeclaringTypeDefinition, includeNamespaceOfDeclaringTypeName) + "." + entityName;
                return EscapeName(entity.Namespace) + "." + entityName;
            }
            else
            {
                return entityName;
            }
        }

        public static StringBuilder EscapeName(StringBuilder sb, string name)
        {
            foreach (char ch in name)
            {
                if (Char.IsWhiteSpace(ch) || Char.IsControl(ch) || Char.IsSurrogate(ch))
                    sb.AppendFormat("\\u{0:x4}", (int)ch);
                else
                    sb.Append(ch);
            }
            return sb;
        }

        /// <summary>
        /// Escape characters that cannot be displayed in the UI.
        /// </summary>
        public static string EscapeName(string name)
        {
            return EscapeName(new StringBuilder(name.Length), name).ToString();
        }

        private static string TypeToString(IType type, bool includeNamespace = false)
        {
            var visitor = new TypeToStringVisitor(includeNamespace);
            type.AcceptVisitor(visitor);
            return visitor.ToString();
        }
    }
}
