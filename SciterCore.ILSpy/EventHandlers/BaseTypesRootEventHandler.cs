using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;
using SciterCore.ILSpy.Languages;
using SciterCore.Interop;

namespace SciterCore.ILSpy.EventHandlers
{
    public class BaseTypesRootEventHandler : RootEventHandler<object, (PEFile Module, ITypeDefinition TypeDefinition)>
    {

        public BaseTypesRootEventHandler(object parent, (PEFile, ITypeDefinition) value) : base(parent, value)
        {
        }

        public override void Decompile(Language language, ITextOutput output, DecompilationOptions options)
        {
            //EnsureLazyChildren();
            //foreach (ILSpyTreeNode child in this.Children)
            //{
            //    child.Decompile(language, output, options);
            //}
        }

        public override bool GetChildren(SciterElement parent, SciterValue[] args, out SciterValue result)
        {
            result = null;

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

        protected override string Title => "Base Types";

        protected override string Suffix => null;

        protected override string Image => null;

        private IEnumerable<dynamic> LoadChildren()
        {
            var result = new List<dynamic>();

            var typeDef = Value.Module.Metadata.GetTypeDefinition((TypeDefinitionHandle)Value.TypeDefinition.MetadataToken);
            var baseTypes = Value.TypeDefinition.DirectBaseTypes.ToArray();
            int i = 0;
            
            if (Value.TypeDefinition.Kind == TypeKind.Interface)
            {
                i++;
            }
            else if (!typeDef.BaseType.IsNil)
            {
                //children.Add(new BaseTypesEntryNode(module, typeDef.BaseType, baseTypes[i], false));

                result.Add(
                    new
                    {
                        //Resource = r,
                        Handler = (SciterEventHandler)null,
                        Value = (Value.Module, Value.TypeDefinition),
                        Text = $"{TypeToString(baseTypes[i], true)}{typeDef.BaseType.ToSuffixString()}",
                    });

                i++;
            }

            foreach (var h in typeDef.GetInterfaceImplementations())
            {
                var impl = Value.Module.Metadata.GetInterfaceImplementation(h);
                //children.Add(new BaseTypesEntryNode(module, impl.Interface, baseTypes[i], true));

                result.Add(
                    new
                    {
                        //Resource = r,
                        Handler = (SciterEventHandler)null,
                        Value = (Value.Module, Value.TypeDefinition),
                        Text = $"{TypeToString(baseTypes[i], true)}{typeDef.BaseType.ToSuffixString()}",
                    });

                i++;
            }

            return result;
        }

        private string TypeToString(IType type, bool includeNamespace)
        {
            var visitor = new TypeToStringVisitor(includeNamespace);
            type.AcceptVisitor(visitor);
            return visitor.ToString();
        }

        protected override void Attached(SciterElement element)
        {
            var textElement = element?.Append("text", $"{Title} {Suffix}"?.Trim());
            textElement?.SetAttribute("suffix", Suffix);

            base.Attached(element);
        }
    }
}