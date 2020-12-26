using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SciterCore;
using SciterCore.Attributes;
using SciterCore.Interop;
using SciterGraphics = SciterCore.SciterGraphics;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.NetCore.Behaviors
{
	[SciterBehavior("custom-exchange")]
	public class CustomExchangeBehavior : SciterEventHandler
	{
		protected override EventGroups SubscriptionsRequest(SciterElement element)
		{
			return EventGroups.HandleExchange;
		}
		
		protected override bool OnExchange(SciterElement element, ExchangeArgs args)
		{
			switch (args.Event)
			{
				case ExchangeEvent.DragEnter:
					element.SetAttributeValue("active", "true");
					return true;
				case ExchangeEvent.DragLeave:
					element.RemoveAttribute("active");
					return true;
				case ExchangeEvent.Drag:
					return true;
				case ExchangeEvent.Drop:
					element.RemoveAttribute("active");
					
					var fileList = new List<string>();
					
					if (args.Value.IsArray)
						fileList.AddRange(args.Value.AsEnumerable().Where(w => w.IsString).Select(s => s.AsString()));
					if (args.Value.IsString)
						fileList.Add(args.Value.AsString());
					
					element.SetHtml(string.Concat(fileList.Select(s => SciterElement.Create("text", s).Html)));
					return true;
				case ExchangeEvent.WillAcceptDrop:
                    
					// Use this for a drop filter!
					//var fileList = new List<string>();
					//if (args.Value.IsArray)
					//	fileList.AddRange(args.Value.AsEnumerable().Where(w => w.IsString).Select(s => s.AsString()));
					//if (args.Value.IsString)
					//	fileList.Add(args.Value.AsString());
					//return fileList.All(a => Path.GetExtension(a).Equals(".exe"));

					return true;
				
				default:
					return base.OnExchange(element, args);
			}
		}

		protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement, BehaviorEvents eventType, IntPtr reason,
			SciterValue data, string eventName)
		{
			switch (eventName)
			{
				case "waiting":
					if (data.IsString)
					{
						//element.SetHtml($"<text>{value.AsString()}</text>");
						//source.SetAttributeValue("waiting", "true");
					}
					else if (data.IsBool)
					{
						//source.SetAttributeValue("waiting", (string) null);
						//element.ClearText();
					}

					break;
			}
			
			return base.OnEvent(sourceElement, targetElement, eventType, reason, data, eventName);
		}
	}
}
