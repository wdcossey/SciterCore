using System;
using System.IO;
using System.Linq;
using System.Reflection;
using SciterCore;
using SciterCore.Attributes;
using SciterCore.Interop;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.NetCore.Behaviors
{
    public class VirtualTreeEventHandler : SciterEventHandler
    {
        private SciterValue _dataSource;

        protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement, BehaviorEvents type, IntPtr reason,
            SciterValue data, string eventName)
        {
            return base.OnEvent(sourceElement, targetElement, type, reason, data, eventName);
        }
        
        protected override bool OnMethodCall(SciterElement element, SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodId)
        {
            return base.OnMethodCall(element, methodId);
        }

        protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
        {
            return base.OnScriptCall(element, method, args);
        }

        public void dataSource(SciterElement element, SciterValue[] args)
        {
            //_logger.LogInformation($"{nameof(SynchronousArgumentFunction)} was executed!\n\t{string.Join(",", args.Select(s => s.AsInt32()))}");
        }
    }
    

    [SciterBehavior("virtual-tree")]
    internal class VirtualTreeBehavior : SciterEventHandler
    {
        protected override void Attached(SciterElement element)
        {
            //<option filename="{path}"><text>{caption}</text></option>
            var cDrive = element
                .AppendChildElement("option")
                .SetAttributeValue("filename", "c:\\")
                .SetState(ElementState.Collapsed)
                .AppendElement("text", "Drive C:");
            
            //expandOption(cDrive);
            
            var dDrive = element
                .AppendChildElement("option")
                .SetAttributeValue("filename", Path.GetDirectoryName(GetType().Assembly.Location))
                .SetState(ElementState.Collapsed)
                .AppendElement("text", Path.GetFileName(Path.GetDirectoryName(GetType().Assembly.Location)));

            //element.AttachEventHandler<VirtualTreeEventHandler>();
            //expandOption(dDrive);
            
            base.Attached(element);
        }

        protected override EventGroups SubscriptionsRequest(SciterElement element)
        {
            return EventGroups.HandleAll;
        }

        protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement, BehaviorEvents type, IntPtr reason,
            SciterValue data, string eventName)
        {
            switch (type)
            {
                
                case BehaviorEvents.ElementExpanded:
                    expandOption(targetElement);
                    targetElement.CallMethod("optionExpanded", targetElement.Value);
                    return true;

                case BehaviorEvents.ElementCollapsed:

                    collapseOption(targetElement);
                    targetElement.CallMethod("optionCollapsed", targetElement.Value);
                    return true;
            }
            
            return base.OnEvent(sourceElement, targetElement, type, reason, data, eventName);
        }

        protected override bool OnMethodCall(SciterElement element, SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodId)
        {
            return base.OnMethodCall(element, methodId);
        }

        protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
        {
            return base.OnScriptCall(element, method, args);
        }

        /*
         function collapseOption(opt)
  {
    //stdout.println("collapse");
    while(opt.length > 1)
      opt.last.remove();
    opt.state.collapsed = true;       
  }
  */
        private void collapseOption(SciterElement opt)
        {
            while(opt.ChildCount > 1)
                opt.Children.LastOrDefault().Delete();
            
            opt.SetState(ElementState.Collapsed);    
            
            //_logger.LogInformation($"{nameof(SynchronousArgumentFunction)} was executed!\n\t{string.Join(",", args.Select(s => s.AsInt32()))}");
        }
        
        /*
  function expandOption(opt)
  {
    //stdout.println("expand");
    function appendChild(caption, path, isFolder) { (this super).appendOption(opt, caption, path, isFolder? false: undefined); }
    this.ds.eachChild(opt.attributes["filename"], appendChild);
    opt.state.expanded = true;
  }
         */
        private void expandOption(SciterElement opt)
        {
            try
            {
                foreach (var file in Directory.GetDirectories(opt["filename"], "*.*", SearchOption.TopDirectoryOnly))
                {
                    appendChild(opt, Path.GetFileName(file), file, true);
                }
            
                foreach (var file in Directory.GetFiles(opt["filename"], "*.*", SearchOption.TopDirectoryOnly))
                {
                    appendChild(opt, Path.GetFileName(file), file, false);
                }

                opt.SetState(ElementState.Expanded);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
           
        }
        
        private void appendChild(SciterElement parent, string caption, string path, bool isFolder)
        {
            var node = parent.AppendChildElement("option")
                .SetAttributeValue("filename", path)
                .AppendElement("text", callback: (e) =>
                {
                    e.SetText(caption);
                });

            //node.SetState(isFolder ? ElementState.Expanded : ElementState.Collapsed);    

            if (isFolder)
            {
                node.SetState(ElementState.Collapsed);    
            }
            //(this super).appendOption(opt, caption, path, isFolder? false: undefined);
        }
        
        public void dataSource(SciterElement element, SciterValue[] args)
        {
            //_logger.LogInformation($"{nameof(SynchronousArgumentFunction)} was executed!\n\t{string.Join(",", args.Select(s => s.AsInt32()))}");
        }
    }
}