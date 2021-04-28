using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SciterCore.Attributes;
using SciterCore.Interop;

namespace SciterCore.Playground.Behaviors
{
    [SciterBehavior("card-drop-behavior")]
    internal class DragDropBehavior : SciterEventHandler
    {
        protected override void Attached(SciterElement element)
        {
            element.Parent.OnCustomEvent((eventName, source, target, value) =>
            {
                switch (eventName)
                {
                    case "waiting":
                        if (value.IsString)
                        {
                            element.SetHtml($"<text>{value.AsString()}</text>");
                            source.SetAttributeValue("waiting", "true");
                        }
                        else if (value.IsBool)
                        {
                            source.SetAttributeValue("waiting", (string) null);
                            element.ClearText();
                        }

                        break;
                }
            });

            base.Attached(element);
        }

        protected override void Detached(SciterElement element)
        {
            base.Detached(element);
        }

        protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
        {
            return base.OnScriptCall(element, method, args);
        }

        protected override bool OnMethodCall(SciterElement element, SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodId)
        {
            return base.OnMethodCall(element, methodId);
        }

        protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement, BehaviorEvents type,
            IntPtr reason,
            SciterValue data, string eventName)
        {
            return base.OnEvent(sourceElement, targetElement, type, reason, data, eventName);
        }

        protected override bool OnExchange(SciterElement element, ExchangeArgs args)
        {
            switch (args.Event)
            {
                case ExchangeEvent.DragEnter:
                    element.AddClass("active-target");
                    return true;
                case ExchangeEvent.DragLeave:
                    element.RemoveClass("active-target");
                    return true;
                case ExchangeEvent.Drag:
                    return true;
                case ExchangeEvent.Drop:
                    element.RemoveClass("active-target");
                    Console.WriteLine($"{args.Value}");
                    return true;
                case ExchangeEvent.WillAcceptDrop:
                    
                    var fileList = new List<string>();
                    if (args.Value.IsArray)
                        fileList.AddRange(args.Value.AsEnumerable().Where(w => w.IsString).Select(s => s.AsString()));
                    if (args.Value.IsString)
                        fileList.Add(args.Value.AsString());

                    return fileList.All(a => Path.GetExtension(a).Equals(".exe"));
                default:
                    return base.OnExchange(element, args);
            }
        }
    }
}