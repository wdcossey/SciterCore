using System;
using SciterCore;
using SciterCore.Interop;
using SciterValue = SciterCore.SciterValue;


namespace SciterTest.CoreForms.Extensions
{
    internal static class SciterBehaviorEventExtensions
    {
        /// <summary>
        /// Maps the <see cref="SciterCore.SciterValue"/> to the specified <see cref="Type"/>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static SciterBehaviors.BEHAVIOR_EVENT_PARAMS FromEventArgs(this SciterBehaviorArgs args)
        {
            return new SciterBehaviors.BEHAVIOR_EVENT_PARAMS
            {
                cmd = (SciterBehaviors.BEHAVIOR_EVENTS)(uint)@args.Command,
                data = args.Data.GetValueOrDefault() ,
                heTarget = args.Target.Handle,
                he = args.Source.Handle,
                reason = args.Reason,
                name = args.Name
            };
        }

        private static SciterCore.Interop.SciterValue.VALUE GetValueOrDefault(this SciterValue value)
        {
            if (value != null) 
                return value.ToVALUE();
            
            Sciter.SciterApi.ValueInit(out var @default);
            return @default;
        }
    }
}