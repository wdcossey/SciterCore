using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;
using SciterCore.ILSpy.Languages;
using SciterCore.Interop;

namespace SciterCore.ILSpy.EventHandlers
{
    public class TypeEventHandler : RootEventHandler<object, (PEFile Module, ITypeDefinition TypeDefinition)>
    {
        public TypeEventHandler(object parent, (PEFile, ITypeDefinition) valueDefinition)
            : base(parent, valueDefinition)
        {

        }

        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            language.DecompileType(Value.TypeDefinition, output, options);
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

        protected override string Title =>
            $"{this.Language.TypeToString(Value.TypeDefinition, includeNamespace: false)}";

        protected override string Suffix => Value.TypeDefinition.MetadataToken.ToSuffixString();

        protected override string Image 
        {
            get
            {
                switch (Value.TypeDefinition.Kind)
                {
                    case TypeKind.Interface:
                        return "interface";
                    case TypeKind.Struct:
                        return "struct";
                    case TypeKind.Delegate:
                        return "delegate";
                    case TypeKind.Enum:
                        return "enum";
                    default:
                        if (Value.TypeDefinition.GetDefinition()?.IsStatic == true)
                            return "staticClass";
                        return "class";
                }
            }
        }

        protected IEnumerable<dynamic> LoadChildren()
        {
            var result = new List<dynamic>();

            if (Value.TypeDefinition.DirectBaseTypes.Any())
            {
                //this.Children.Add(new BaseTypesTreeNode(ParentAssemblyNode.LoadedAssembly.GetPEFileOrNull(), _typeDefinition));

                result.Add(
                    new
                    {
                        Handler = typeof(BaseTypesRootEventHandler),
                        Value = (Value.Module, Value.TypeDefinition),
                    });
            }


            if (!Value.TypeDefinition.IsSealed)
            {
                //this.Children.Add(new DerivedTypesTreeNode(ParentAssemblyNode.AssemblyList, _typeDefinition));

                result.Add(
                    new
                    {
                        Handler = typeof(DerivedTypesRootEventHandler),
                        Value = (Value.Module, Value.TypeDefinition),
                    });
            }

            foreach (var nestedType in Value.TypeDefinition.NestedTypes.OrderBy(t => t.Name, NaturalStringComparer.Instance))
            {
                result.Add(
                    new
                    {
                        Handler = typeof(TypeEventHandler),
                        Value = (Value.Module, nestedType),
                        //Text = $"{TypeToString(nestedType)}{nestedType.MetadataToken.ToSuffixString()}",
                    });

                //this.Children.Add(new TypeTreeNode(nestedType, ParentAssemblyNode));
            }
            if (Value.TypeDefinition.Kind == TypeKind.Enum)
            {
                // if the type is an enum, it's better to not sort by field name.
                foreach (var field in Value.TypeDefinition.Fields)
                {
                    result.Add(
                        new
                        {
                            Handler = typeof(FieldEventHandler),
                            Value = field,
                        });

                    //this.Children.Add(new FieldTreeNode(field));
                }
            }
            else
            {
                foreach (var field in Value.TypeDefinition.Fields.OrderBy(f => f.Name, NaturalStringComparer.Instance))
                {
                    result.Add(
                        new
                        {
                            Handler = typeof(FieldEventHandler),
                            Value = field,
                        });

                    //this.Children.Add(new FieldTreeNode(field));
                }
            }
            foreach (var property in Value.TypeDefinition.Properties.OrderBy(p => p.Name, NaturalStringComparer.Instance))
            {
                result.Add(
                    new
                    {
                        Handler = typeof(PropertyEventHandler),
                        Value = property,
                    });

                //this.Children.Add(new PropertyTreeNode(property));
            }
            foreach (var ev in Value.TypeDefinition.Events.OrderBy(e => e.Name, NaturalStringComparer.Instance))
            {
                result.Add(
                    new
                    {
                        Handler = typeof(EventEventHandler),
                        Value = ev,
                    });


                //this.Children.Add(new EventTreeNode(ev));
            }

            foreach (var method in Value.TypeDefinition.Methods.OrderBy(m => m.Name, NaturalStringComparer.Instance))
            {
                if (method.MetadataToken.IsNil)
                    continue;

                result.Add(
                    new
                    {
                        Handler = typeof(MethodEventHandler),
                        Value = method,
                    });

                //this.Children.Add(new MethodTreeNode(method));
            }

            return result;
        }

        //private static string PropertyToString(IProperty property, bool includeDeclaringTypeName = false, bool includeNamespace = false, bool includeNamespaceOfDeclaringTypeName = false)
        //{
        //    if (property == null)
        //        throw new ArgumentNullException(nameof(property));
        //    return GetDisplayName(property, includeDeclaringTypeName, includeNamespace, includeNamespaceOfDeclaringTypeName) + " : " + TypeToString(property.ReturnType, includeNamespace);
        //}

        //private static string EventToString(IEvent @event, bool includeDeclaringTypeName = false, bool includeNamespace = false, bool includeNamespaceOfDeclaringTypeName = false)
        //{
        //    if (@event == null)
        //        throw new ArgumentNullException(nameof(@event));
        //    var buffer = new StringBuilder();
        //    buffer.Append(GetDisplayName(@event, includeDeclaringTypeName, includeNamespace, includeNamespaceOfDeclaringTypeName));
        //    buffer.Append(" : ");
        //    buffer.Append(TypeToString(@event.ReturnType, includeNamespace));
        //    return buffer.ToString();
        //}

        private static string GetTypeAccess(ITypeDefinition type)
        {
            switch (type.Accessibility)
            {
                case Accessibility.Public:
                    return "public";
                case Accessibility.Internal:
                    return "internal";
                case Accessibility.ProtectedAndInternal:
                    return "privateProtected";
                case Accessibility.Protected:
                case Accessibility.ProtectedOrInternal:
                    return "protected";
                case Accessibility.Private:
                    return "private";
                default:
                    return "compilerControlled";
            }
        }

        protected override void Attached(SciterElement element)
        {
            element?.SetAttribute("image", Image);
            element?.SetAttribute("access", GetTypeAccess(Value.TypeDefinition));
            var textElement = element?.Append("text", $"{Title} {Suffix}");
            textElement?.SetAttribute("suffix", Suffix);
            base.Attached(element);
        }

    }
}