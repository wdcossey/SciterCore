using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SciterCore;
using SciterCore.Attributes;

namespace SciterTest.NetCore.Behaviors
{
    [SciterBehavior("card-drop-behavior")]
    internal class DragDropBehavior : SciterEventHandler
    {

        public DragDropBehavior()
        {
            
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