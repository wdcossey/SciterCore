using System.Runtime.InteropServices;
using SciterCore.Attributes;

namespace SciterCore.JS.HelloSciter.Behaviors
{
    [SciterBehavior("runtime-info")]
    public class RuntimeInformationBehavior : SciterEventHandler
    {
        protected override EventGroups SubscriptionsRequest(SciterElement element)
        {
            return EventGroups.HandleBehaviorEvent;
        }

        protected override void Attached(SciterElement element)
        {
            element.SelectFirst("#frameworkDescription").SetText(RuntimeInformation.FrameworkDescription);
            element.SelectFirst("#processArchitecture").SetText(RuntimeInformation.ProcessArchitecture.ToString());
            element.SelectFirst("#osArchitecture").SetText(RuntimeInformation.OSArchitecture.ToString());
            element.SelectFirst("#osDescription").SetText(RuntimeInformation.OSDescription);
            element.SelectFirst("#osContent").SetAttributeValue("state", "visible");
            base.Attached(element);
        }

        protected override void Detached(SciterElement element)
        {
            
            base.Detached(element);
        }

    }
}