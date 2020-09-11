using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SciterCore
{
	internal class ScriptExecutioner
    {
	    
	    private enum ReturnType
	    {
		    Unknown,
			Void,
			SciterValue,
	    }
	    
	    private readonly object _owner; 
	    private readonly SciterElement _element;
	    private readonly MethodInfo _methodInfo;
	    private readonly SciterValue[] _arguments;
	    
	    private readonly ReturnType _returnType;
	    private readonly bool _isAwaitable;
	    private readonly IList<ParameterInfo> _methodParameters;

	    private ScriptExecutioner(object owner, SciterElement element, MethodInfo methodInfo, SciterValue[] arguments)
	    {
		    _owner = owner;
		    _element = element;
		    _methodInfo = methodInfo;
		    _arguments = arguments;
		    
		    _methodParameters = _methodInfo.GetParameters();
		    
		    _isAwaitable = _methodInfo.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;

		    _returnType = GetReturnType();
	    }

	    private ReturnType GetReturnType()
	    {
		    if (typeof(void).IsAssignableFrom(_methodInfo.ReturnType) || 
		        
		         _methodInfo.ReturnType.IsGenericType &&
		         _methodInfo.ReturnType.GenericTypeArguments.Length == 1 && 
		         typeof(void).IsAssignableFrom(_methodInfo.ReturnType.GenericTypeArguments[0]) ||
		         typeof(Task).IsAssignableFrom(_methodInfo.ReturnType) && !_methodInfo.ReturnType.IsGenericType)
		    {
			    return ReturnType.Void;
		    }

		    //if (typeof(bool).IsAssignableFrom(_methodInfo.ReturnType) || 
		    //    (_methodInfo.ReturnType.IsGenericType &&
		    //     _methodInfo.ReturnType.GenericTypeArguments.Length == 1 && 
		    //     typeof(bool).IsAssignableFrom(_methodInfo.ReturnType.GenericTypeArguments[0])))
		    //{
			//    return ReturnType.Boolean;
		    //}

		    if (typeof(SciterValue).IsAssignableFrom(_methodInfo.ReturnType) || 
		        (_methodInfo.ReturnType.IsGenericType &&
		         _methodInfo.ReturnType.GenericTypeArguments.Length == 1 && 
		         typeof(SciterValue).IsAssignableFrom(_methodInfo.ReturnType.GenericTypeArguments[0])))
		    {
			    return ReturnType.SciterValue;
		    }

		    return ReturnType.Unknown;
	    }

	    private object[] BuildParameters()
	    {
		    var result = new List<object>();

		    // ReSharper disable once InvertIf
		    if (_methodParameters.Count > 0)
		    {
			    var parameterIndex = 0;
			    
			    var elementParameter =
				    _methodParameters.FirstOrDefault(fd => typeof(SciterElement).IsAssignableFrom(fd.ParameterType));

			    if (elementParameter != null)
			    {
				    result.Add(_element);
				    parameterIndex++;
			    }

			    // ReSharper disable once InvertIf
			    if (_arguments?.Any() == true)
			    {
				    if (_methodParameters[parameterIndex].ParameterType.IsArray &&
				        typeof(SciterValue).IsAssignableFrom(_methodParameters[parameterIndex].ParameterType.GetElementType()))
				    {
					    result.Add(_arguments);
				    }
				    else
				    {
					    result.AddRange(_arguments.Take(_methodParameters.Count));
				    }
			    }
		    }

		    return result.ToArray();
	    }
	    
	    public static ScriptExecutioner Create(object owner, SciterElement sciterElement, MethodInfo methodInfo, SciterValue[] arguments)
	    {
		    return new ScriptExecutioner(owner, sciterElement, methodInfo, arguments);
	    }
	    
        public ScriptEventResult Execute()
		{
			if (_methodInfo != null)
			{
				//Does the method have an Awaiter (a Task)?
				//var hasAwaiter = _methodInfo.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
				
				//Does the method return `SciterValue` ([a]synchronously)
				//var returnsSciterValue = typeof(SciterValue).IsAssignableFrom(_methodInfo.ReturnType) || _methodInfo.ReturnType.IsGenericType &&
				//	_methodInfo.ReturnType.GenericTypeArguments.Length == 1 && typeof(SciterValue).IsAssignableFrom(_methodInfo.ReturnType.GenericTypeArguments[0]);

				var parameters = BuildParameters();
				
				//if (_isAwaitable && 
				//    _returnType == ReturnType.SciterValue &&
				//    _arguments.Any(a => a.IsFunction || a.IsObjectFunction))
				//{
				//	// Safe to call `First` here as the check is done above.
				//	var callbackFunc = _arguments.First(f => f.IsObjectFunction || f.IsFunction);
//
				//	try
				//	{
				//		if (_methodParameters.Count <= 0 &&
				//		    !typeof(SciterElement).IsAssignableFrom(_methodParameters[0].ParameterType))
				//		{
				//			return ScriptEventResult.Failed();
				//		}
//
				//		if (_methodParameters.Count == 2 &&
				//		    _methodParameters[1].ParameterType.IsArray &&
				//		    typeof(SciterValue).IsAssignableFrom(_methodParameters[1].ParameterType.GetElementType()))
				//		{
				//			((Task<SciterValue>) _methodInfo.Invoke(_owner, parameters))?.ContinueWith(
				//				task =>
				//				{
				//					if (task.IsFaulted)
				//						return;
//
				//					callbackFunc.Call(task.Result, SciterValue.Null);
				//				});
				//		}
				//		else if (_methodParameters.Count > 2)
				//		{
				//			_methodInfo.Invoke(_owner, parameters.ToArray());
				//		}
				//	}
				//	catch (TargetInvocationException e)
				//	{
				//		//TODO: Clean this up, maybe change the Dictionary<> implementation?
				//		var properties = (e.InnerException ?? e)
				//			.GetType()
				//			.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				//			.Where(w => typeof(IConvertible).IsAssignableFrom(w.PropertyType))
				//			.ToDictionary(key => key.Name, value => value.GetValue((e.InnerException ?? e)) as IConvertible);
				//			//.ToDictionary(key => key.Name, value => SciterValue.Create(value.GetValue(e.InnerException)));
				//		properties.Add(nameof(Type), (e.InnerException ?? e).GetType().FullName);
				//		
				//		callbackFunc.Call(SciterValue.Null, SciterValue.Create(properties));
				//	}
				//	catch (Exception e)
				//	{
				//		callbackFunc.Call(SciterValue.Null, SciterValue.Create(new { Type = e.GetType().FullName, e.Message, e.StackTrace, e.Source }));
				//	}
				//	
				//	// Tasks should return Successful, the callback function will be fired when the Task completes. 
				//	return ScriptEventResult.Successful();
				//}
				
				// match signature:
				// void MethodName()
				// void MethodName(SciterElement element)
				// void MethodName(SciterValue[] args)
				// void MethodName(SciterElement element, SciterValue[] args)
				// void MethodName(SciterValue arg1, SciterValue arg2, ...)
				// void MethodName(SciterElement element, SciterValue arg1, SciterValue arg2, ...)
				
				// SciterValue MethodName()',
				// SciterValue MethodName(SciterElement element)
				// SciterValue MethodName(SciterValue[] args)
				// SciterValue MethodName(SciterElement element, SciterValue[] args)
				// SciterValue MethodName(SciterValue arg1, SciterValue arg2, ...)
				// SciterValue MethodName(SciterElement element, SciterValue arg1, SciterValue arg2, ...)
				
				// Task MethodName()
				// Task MethodName(SciterElement element)
				// Task MethodName(SciterValue[] args)
				// Task MethodName(SciterElement element, SciterValue[] args)
				// Task MethodName(SciterValue arg1, SciterValue arg2, ...)
				// Task MethodName(SciterElement element, SciterValue arg1, SciterValue arg2, ...)
				
				// Task<SciterValue> MethodName()'
				// Task<SciterValue> MethodName(SciterElement element)
				// Task<SciterValue> MethodName(SciterValue[] args)
				// Task<SciterValue> MethodName(SciterElement element, SciterValue[] args)
				// Task<SciterValue> MethodName(SciterValue arg1, SciterValue arg2, ...)
				// Task<SciterValue> MethodName(SciterElement element, SciterValue arg1, SciterValue arg2, ...)
				
				{
					try
					{ 
						//var parameters = BuildParameters();

						//TODO: Add error handling!
						var ret = _methodInfo.Invoke(_owner, parameters);

						// Awaitable Tasks should return Successful immediately, don't block the main Thread waiting for completion! 
						if (_isAwaitable)
							return ScriptEventResult.Successful();

						var value = (ret as SciterValue) ?? SciterValue.Null;
						return ScriptEventResult.Successful(value);
						
					}
					catch (TargetInvocationException e)
					{
						//TODO: Clean this up, maybe change the Dictionary<> implementation?
						var properties = (e.InnerException ?? e)
							.GetType()
							.GetProperties(BindingFlags.Instance | BindingFlags.Public)
							.Where(w => typeof(IConvertible).IsAssignableFrom(w.PropertyType))
							.ToDictionary(key => key.Name, value => value.GetValue((e.InnerException ?? e)) as IConvertible);
						//.ToDictionary(key => key.Name, value => SciterValue.Create(value.GetValue(e.InnerException)));
						properties.Add(nameof(Type), (e.InnerException ?? e).GetType().FullName);
						
						return ScriptEventResult.Successful(SciterValue.MakeError(e.InnerException?.Message ?? e.ToString()));
						
						//callbackFunc.Call(SciterValue.Null, SciterValue.Create(properties));
					}
					catch (Exception e)
					{
						return ScriptEventResult.Successful(SciterValue.MakeError(e.Message));
						//callbackFunc.Call(SciterValue.Null, SciterValue.Create(new { Type = e.GetType().FullName, e.Message, e.StackTrace, e.Source }));
					}
				}

			}

			// not handled
			return ScriptEventResult.Failed();
		}
    }
}