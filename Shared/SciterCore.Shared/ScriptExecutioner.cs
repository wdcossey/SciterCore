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
			Boolean,
			SciterValue,
	    }
	    
	    private readonly object _owner; 
	    private readonly SciterElement _element;
	    private readonly MethodInfo _methodInfo;
	    private readonly SciterValue[] _arguments;
	    
	    private readonly ReturnType _returnType;
	    private readonly bool _isAwaitable;

	    private ScriptExecutioner(object owner, SciterElement element, MethodInfo methodInfo, SciterValue[] arguments)
	    {
		    _owner = owner;
		    _element = element;
		    _methodInfo = methodInfo;
		    _arguments = arguments;
		    
		    _isAwaitable = _methodInfo.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
		    
		    if (typeof(void).IsAssignableFrom(_methodInfo.ReturnType) || 
		        
		        (_methodInfo.ReturnType.IsGenericType &&
			    _methodInfo.ReturnType.GenericTypeArguments.Length == 1 && typeof(void).IsAssignableFrom(_methodInfo.ReturnType.GenericTypeArguments[0])) ||
		        
		        (typeof(Task).IsAssignableFrom(_methodInfo.ReturnType) && !_methodInfo.ReturnType.IsGenericType))
		    {
			    _returnType = ReturnType.Void;
		    }
		    else if (typeof(bool).IsAssignableFrom(_methodInfo.ReturnType) || (_methodInfo.ReturnType.IsGenericType &&
			    _methodInfo.ReturnType.GenericTypeArguments.Length == 1 && typeof(bool).IsAssignableFrom(_methodInfo.ReturnType.GenericTypeArguments[0])))
		    {
			    _returnType = ReturnType.Boolean;
		    }
		    else if (typeof(SciterValue).IsAssignableFrom(_methodInfo.ReturnType) || (_methodInfo.ReturnType.IsGenericType &&
		                                                                              _methodInfo.ReturnType.GenericTypeArguments.Length == 1 && typeof(SciterValue).IsAssignableFrom(_methodInfo.ReturnType.GenericTypeArguments[0])))
		    {
			    _returnType = ReturnType.SciterValue;
		    }
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

				var methodParameters = _methodInfo.GetParameters();
				
				if (_isAwaitable && 
				    _returnType == ReturnType.SciterValue &&
				    _arguments.Any(a => a.IsFunction || a.IsObjectFunction))
				{
					// Safe to call `First` here as the check is done above.
					var callbackFunc = _arguments.First(f => f.IsObjectFunction || f.IsFunction);

					try
					{
						if (methodParameters.Length <= 0 &&
						    !typeof(SciterElement).IsAssignableFrom(methodParameters[0].ParameterType))
						{
							return ScriptEventResult.Failed();
						}

						if (methodParameters.Length == 2 &&
						    methodParameters[1].ParameterType.IsArray &&
						    typeof(SciterValue).IsAssignableFrom(methodParameters[1].ParameterType.GetElementType()))
						{
							((Task<SciterValue>) _methodInfo.Invoke(_owner, new object[] {_element, _arguments}))?.ContinueWith(
								task =>
								{
									if (task.IsFaulted)
										return;

									callbackFunc.Call(task.Result, SciterValue.Null);
								});
						}
						else if (methodParameters.Length > 2)
						{
							var parameters = new List<object>
							{
								_element
							};
							
							parameters.AddRange(_arguments);
							
							_methodInfo.Invoke(_owner, parameters.ToArray());
						}
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
						
						callbackFunc.Call(SciterValue.Null, SciterValue.Create(properties));
					}
					catch (Exception e)
					{
						callbackFunc.Call(SciterValue.Null, SciterValue.Create(new { Type = e.GetType().FullName, e.Message, e.StackTrace, e.Source }));
					}
					
					// Tasks should return Successful, the callback function will be fired when the Task completes. 
					return ScriptEventResult.Successful();
				}
				
				// match signature:
				// 'void MethodName()' or 'SciterValue MethodName()'
				{
					if(!methodParameters.Any() && _returnType != ReturnType.Unknown)
					{
						//TODO: Add error handling!
						var ret = _methodInfo.Invoke(_owner, null);

						// Awaitable Tasks should return Successful immediately, don't block the main Thread waiting for completion! 
						if (_isAwaitable)
							return ScriptEventResult.Successful();

						var value = _returnType switch
						{
							ReturnType.Boolean => SciterValue.Create((ret as bool?) == true),
							ReturnType.SciterValue => ret as SciterValue,
							_ => SciterValue.Null
						};

						return ScriptEventResult.Successful(value);
					}
				}

				// match signature:
				// 'void MethodName(SciterValue[] args)' or 'SciterValue MethodName(SciterValue[] args)'
				{
					if(methodParameters.Length == 1 && 
					   methodParameters[0].ParameterType.IsArray && 
					   typeof(SciterValue).IsAssignableFrom(methodParameters[0].ParameterType.GetElementType()) &&
					   (_returnType != ReturnType.Unknown))
					{
						var parameters = new object[] { _arguments };
						
						//TODO: Add error handling!
						var ret = _methodInfo.Invoke(_owner, parameters);
						
						SciterValue value = null;
						
						if(_returnType == ReturnType.SciterValue)
							value = (SciterValue)ret;
						
						return ScriptEventResult.Successful(value);
					}
				}

				// match signature:
				// bool MethodName(SciterElement el, SciterValue[] args, out SciterValue result)
				{
					if (typeof(bool).IsAssignableFrom(_methodInfo.ReturnType) && 
					   methodParameters.Length == 3 && 
					   methodParameters[0].ParameterType.Name == "SciterElement" && 
					   methodParameters[1].ParameterType.Name == "SciterValue[]" && 
					   methodParameters[2].ParameterType.Name == "SciterValue&")
					{
						object[] parameters = new object[] { _element, _arguments, null };
						
						//TODO: Add error handling!
						bool res = (bool)(_methodInfo?.Invoke(_owner, parameters) ?? false);
						
						Debug.Assert(parameters[2] == null || parameters[2].GetType().IsAssignableFrom(typeof(SciterValue)));
						return ScriptEventResult.Successful(parameters[2] as SciterValue);
					}
				}
			}

			// not handled
			return ScriptEventResult.Failed();
		}
    }
}