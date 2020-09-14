using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SciterCore.Attributes;

namespace SciterCore
{
	
	/*
		match signature:
		void MethodName()
		void MethodName(SciterElement element)
		void MethodName(SciterValue[] args)
		void MethodName(SciterElement element, SciterValue[] args)
		void MethodName(SciterValue arg1, SciterValue arg2, ...)
		void MethodName(SciterElement element, SciterValue arg1, SciterValue arg2, ...)
		
		SciterValue MethodName()',
		SciterValue MethodName(SciterElement element)
		SciterValue MethodName(SciterValue[] args)
		SciterValue MethodName(SciterElement element, SciterValue[] args)
		SciterValue MethodName(SciterValue arg1, SciterValue arg2, ...)
		SciterValue MethodName(SciterElement element, SciterValue arg1, SciterValue arg2, ...)
		
		Task MethodName()
		Task MethodName(SciterElement element)
		Task MethodName(SciterValue[] args)
		Task MethodName(SciterElement element, SciterValue[] args)
		Task MethodName(SciterValue arg1, SciterValue arg2, ...)
		Task MethodName(SciterElement element, SciterValue arg1, SciterValue arg2, ...)
		
		Task<SciterValue> MethodName()
		Task<SciterValue> MethodName(SciterElement element)
		Task<SciterValue> MethodName(SciterValue[] args)
		Task<SciterValue> MethodName(SciterElement element, SciterValue[] args)
		Task<SciterValue> MethodName(SciterValue arg1, SciterValue arg2, ...)
		Task<SciterValue> MethodName(SciterElement element, SciterValue arg1, SciterValue arg2, ...)
	*/
	
	internal class ScriptExecutioner
    {
	    
	    private enum ReturnType
	    {
		    Unsupported,
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
	    private readonly bool _isWrappedCallback;
	    private readonly SciterValue _callbackValue;

	    private ScriptExecutioner(object owner, SciterElement element, MethodInfo methodInfo, SciterValue[] arguments)
	    {
		    _owner = owner;
		    _element = element;
		    _methodInfo = methodInfo;
		    _arguments = arguments;
		    
		    _methodParameters = _methodInfo.GetParameters();
		    
		    _isAwaitable = _methodInfo.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
		    
		    _isWrappedCallback = _methodInfo.GetCustomAttribute<SciterCallbackWrapperAttribute>() != null;
		    _callbackValue = _isWrappedCallback ? _arguments.Last() : null;

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
		    
		    if (typeof(SciterValue).IsAssignableFrom(_methodInfo.ReturnType) || 
		        (_methodInfo.ReturnType.IsGenericType &&
		         _methodInfo.ReturnType.GenericTypeArguments.Length == 1 && 
		         typeof(SciterValue).IsAssignableFrom(_methodInfo.ReturnType.GenericTypeArguments[0])))
		    {
			    return ReturnType.SciterValue;
		    }

		    return ReturnType.Unsupported;
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
			if (_returnType == ReturnType.Unsupported)
			{
				//Can't use anything that's not void: SciterValue, Task<void> or Task<SciterValue>
				return ScriptEventResult.Failed();
			}
			
			if (_methodInfo != null)
			{
				var parameters = BuildParameters();
				{
					try
					{
						if (_isWrappedCallback && _isAwaitable)
						{
							((Task) _methodInfo.Invoke(_owner, parameters))?.ContinueWith(
								task =>
								{
									if (task.IsFaulted)
										return;

									if (!task.GetType().IsGenericType) 
										return;

									_callbackValue?.Invoke((task as Task<SciterValue>)?.Result ?? SciterValue.Null, SciterValue.Null);
								});
							
							return ScriptEventResult.Successful();
						}

						var ret = _methodInfo.Invoke(_owner, parameters);

						// Awaitable Tasks should return Successful immediately, don't block the main Thread waiting for completion! 
						if (_isAwaitable)
							return ScriptEventResult.Successful();

						var value = (ret as SciterValue) ?? SciterValue.Null;
						return ScriptEventResult.Successful(value);
						
					}
					catch (TargetInvocationException e)
					{
						return ExceptionCallbackResult(e.InnerException ?? e);
					}
					catch (Exception e)
					{
						return ExceptionCallbackResult(e);
					}
				}
			}

			// not handled
			return ScriptEventResult.Failed();
		}

        private ScriptEventResult ExceptionCallbackResult(Exception e)
        {
	        if (!_isWrappedCallback || !_isAwaitable)
		        return ScriptEventResult.Successful(SciterValue.MakeError(e?.Message));
						
	        //TODO: Clean this up, maybe change the Dictionary<> implementation?
	        var properties = (e)
		        .GetType()
		        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
		        .Where(w => typeof(IConvertible).IsAssignableFrom(w.PropertyType))
		        .ToDictionary(key => key.Name, value => value.GetValue(e) as IConvertible);
	        //.ToDictionary(key => key.Name, value => SciterValue.Create(value.GetValue(e.InnerException)));
	        properties.Add(nameof(Type), e?.GetType().FullName);
                                                    
	        _callbackValue?.Invoke(SciterValue.Null, SciterValue.Create(properties));
	        
	        return ScriptEventResult.Successful();
        }
    }
}