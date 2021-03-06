﻿using System;
using System.Linq;
using SciterCore.Attributes;

namespace SciterCore.Extensions
{
    internal static class BehaviourAttributeExtensions
    {
        public static string GetBehaviourName(this Type eventHandlerType, string behaviorName = null)
        {
            if (!typeof(SciterEventHandler).IsAssignableFrom(eventHandlerType) || typeof(SciterEventHandler) == eventHandlerType)
                throw new ArgumentOutOfRangeException(nameof(eventHandlerType), $@"'{nameof(eventHandlerType)}' must extend SciterEventHandler");

            if (!string.IsNullOrWhiteSpace(behaviorName))
            {
                return behaviorName;
            }

            var attribute = eventHandlerType.GetCustomAttributes(typeof(SciterBehaviorAttribute), false)?.Cast<SciterBehaviorAttribute>()?.FirstOrDefault();

            return attribute?.Name ?? eventHandlerType.Name;
        }

        public static string GetBehaviourName(this SciterEventHandler eventHandler, string behaviorName = null)
        {
            return eventHandler?.GetType().GetBehaviourName(behaviorName: behaviorName);
        }

    }
}
