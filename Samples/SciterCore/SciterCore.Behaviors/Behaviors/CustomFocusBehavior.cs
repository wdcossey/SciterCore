using System.Diagnostics;
using SciterCore.Attributes;

namespace SciterCore.Behaviors.Behaviors
{
    [SciterBehavior("focus-behavior")]
    public class CustomFocusBehavior : SciterEventHandler
    {
        protected override EventGroups SubscriptionsRequest(SciterElement element)
        {
            return EventGroups.HandleFocus;
        }

        protected override bool OnFocus(SciterElement element, FocusArgs args)
        {
            Debug.WriteLine($"focus-behavior: {args.Event}");

            SciterColor color;
            switch (args.Event)
            {
                case FocusEvents.Got:
                    color = SciterColor.CornflowerBlue;
                    break;
                case FocusEvents.Lost:
                    color = SciterColor.Crimson;
                    break;
                default:
                    return base.OnFocus(element, args);
            }

            element
                .SetStyleValue("background-color", $"{color.ToShortHtmlColor()}")
                .SetHtml(SciterElement.Create("div", $"{args.Event}").Html);
            return base.OnFocus(element, args);
        }
    }
}