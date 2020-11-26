using SciterCore;
using SciterCore.Attributes;

namespace SciterTest.NetCore.Behaviors
{
    [SciterBehavior("window-behavior")]
    public class CustomWindowEventHandler: SciterEventHandler
    {
        protected override void Attached(SciterElement element)
        {
            base.Attached(element);
        }
        
        protected override EventGroups SubscriptionsRequest(SciterElement element)
        {
            return EventGroups.HandleAll;
        }
        
        public void SynchronousFunction()
        {
           // _logger.LogInformation($"{nameof(SynchronousFunction)} was executed!");
        }
    }
}