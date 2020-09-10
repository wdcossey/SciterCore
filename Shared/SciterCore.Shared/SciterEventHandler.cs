// Copyright 2016 Ramon F. Mendes
//
// This file is part of SciterSharp.
// 
// SciterSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// SciterSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with SciterSharp.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SciterCore.Interop;
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnusedParameter.Global

namespace SciterCore
{
	public abstract class SciterEventHandler
	{
		
#if DEBUG
		
		private volatile bool _isAttached = false;
		
		~SciterEventHandler()
		{
			Debug.Assert(!AttachedHandlers.Contains(this)); 
			Debug.Assert(_isAttached == false);
		}
		
#endif
		
		private static readonly List<SciterEventHandler> AttachedHandlers = new List<SciterEventHandler>();// we keep a copy of all attached instances to guard from GC removal

		public SciterEventHandler()
            : this(name: null)
		{

		}

		public SciterEventHandler(string name = null)
		{
			EventProc = EventProcMethod;
			Name = name ?? this.GetType().FullName;
		}
		
		public string Name { get; internal set; }
		
		public delegate bool WorkDelegate(IntPtr tag, IntPtr he, uint evtg, IntPtr prms);
		
		internal readonly WorkDelegate EventProc;// keep a copy of the delegate so it survives GC
		
		// Overrideables'
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		protected virtual EventGroups SubscriptionsRequest(SciterElement element)
		{
			return EventGroups.HandleAll;
		}

		protected virtual void Attached(SciterElement element)
		{
			
		}

		protected virtual void Detached(SciterElement element)
		{
			
		}

		protected virtual bool OnMouse(SciterElement element, MouseArgs args)
		{
			return false;
		}

		protected virtual bool OnKey(SciterElement element, KeyArgs args)
		{
			return false;
		}

		protected virtual bool OnFocus(SciterElement element, FocusArgs args)
		{
			return false;
		}

		protected virtual bool OnTimer(SciterElement element)
		{
			return false;
		}

		protected virtual bool OnTimer(SciterElement element, IntPtr extTimerId)
		{
			return false;
		}

		protected virtual bool OnSize(SciterElement element)
		{
			return false;
		}

		protected virtual bool OnDraw(SciterElement element, DrawArgs args)
		{
			return false;
		}

		protected virtual bool OnMethodCall(SciterElement element, SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodId)
		{
			return false;
		}

		protected virtual ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
		{
			if(method != null)
			{
				//Check if the method returns Task<SciterValue> and has a callback in the args
				if (method.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null &&
				    args.Any(a => a.IsFunction || a.IsObjectFunction) &&
				    method.ReturnType.IsGenericType &&
				    method.ReturnType.GenericTypeArguments.Length == 1 && typeof(SciterValue).IsAssignableFrom(method.ReturnType.GenericTypeArguments[0]))
				{
					// Safe to call `First` here as the check is done above.
					var callbackFunc = args.First(f => f.IsObjectFunction || f.IsFunction);

					try
					{
						var methodParameters = method.GetParameters();

						if (methodParameters.Length <= 0 &&
						    !typeof(SciterElement).IsAssignableFrom(methodParameters[0].ParameterType))
						{
							return ScriptEventResult.Failed();
						}

						if (methodParameters.Length == 2 &&
						    methodParameters[1].ParameterType.IsArray &&
						    typeof(SciterValue).IsAssignableFrom(methodParameters[1].ParameterType.GetElementType()))
						{
							((Task<SciterValue>) method.Invoke(this, new object[] {element, args}))?.ContinueWith(
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
								element
							};
							
							parameters.AddRange(args);
							
							method.Invoke(this, parameters.ToArray());
						}
					}
					catch (TargetInvocationException e)
					{
						//TODO: Clean this up, maybe change the Dictionary<> implementation?
						Dictionary<string, IConvertible> properties = (e.InnerException ?? e)
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
				
				// This base class tries to handle it by searching for a method with the same 'name'
				var mparams = method.GetParameters();

				// match signature:
				// 'void MethodName()' or 'SciterValue MethodName()'
				{
					if(mparams.Length == 0 && 
						(method.ReturnType == typeof(void) || method.ReturnType == typeof(SciterValue)))
					{
						//TODO: Add error handling!
						var ret = method.Invoke(this, null);

						SciterValue value = null;
						
						if(method.ReturnType == typeof(SciterValue))
							value = (SciterValue)ret;
						
						return ScriptEventResult.Successful(value);
					}
				}

				// match signature:
				// 'void MethodName(SciterValue[] args)' or 'SciterValue MethodName(SciterValue[] args)'
				{
					if(mparams.Length==1 && mparams[0].ParameterType.Name == "SciterValue[]" &&
						(method.ReturnType == typeof(void) || method.ReturnType == typeof(SciterValue)))
					{
						var parameters = new object[] { args };
						
						//TODO: Add error handling!
						var ret = method.Invoke(this, parameters);
						
						SciterValue value = null;
						
						if(method.ReturnType == typeof(SciterValue))
							value = (SciterValue)ret;
						
						return ScriptEventResult.Successful(value);
					}
				}

				// match signature:
				// bool MethodName(SciterElement el, SciterValue[] args, out SciterValue result)
				{
					if(method.ReturnType == typeof(bool) && mparams.Length == 3
						&& mparams[0].ParameterType.Name == "SciterElement"
						&& mparams[1].ParameterType.Name == "SciterValue[]"
						&& mparams[2].ParameterType.Name == "SciterValue&")
					{
						object[] parameters = new object[] { element, args, null };
						
						//TODO: Add error handling!
						bool res = (bool)(method?.Invoke(this, parameters) ?? false);
						
						Debug.Assert(parameters[2] == null || parameters[2].GetType().IsAssignableFrom(typeof(SciterValue)));
						return ScriptEventResult.Successful(parameters[2] as SciterValue);
					}
				}
			}

			// not handled
			return ScriptEventResult.Failed();
		}

		protected virtual bool OnEvent(SciterElement sourceElement, SciterElement targetElement,
			SciterBehaviors.BEHAVIOR_EVENTS type, IntPtr reason, SciterValue data)
		{
			return false;
		}

		protected virtual bool OnDataArrived(SciterElement element, SciterBehaviors.DATA_ARRIVED_PARAMS prms)
		{
			return false;
		}
		
		protected virtual bool OnScroll(SciterElement element, ScrollEventArgs args)
		{
			return false;
		}

		protected virtual bool OnGesture(SciterElement element, GestureArgs args)
		{
			return false;
		}

		protected virtual bool OnExchange(SciterElement element, ExchangeArgs args)
		{
			return false;
		}

		// EventProc
		private bool EventProcMethod(IntPtr tag, IntPtr he, uint evtg, IntPtr prms)
		{
			SciterElement se = null;
			if(he != IntPtr.Zero)
				se = new SciterElement(he);

			switch((SciterBehaviors.EVENT_GROUPS)evtg)
			{
				case SciterBehaviors.EVENT_GROUPS.SUBSCRIPTIONS_REQUEST:
					{
						var groups = SubscriptionsRequest(se);
						Marshal.WriteInt32(prms, (int)groups);
						return true;
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_INITIALIZATION:
					{
						SciterBehaviors.INITIALIZATION_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.INITIALIZATION_PARAMS>(ptr: prms);
						if(p.cmd == SciterBehaviors.INITIALIZATION_EVENTS.BEHAVIOR_ATTACH)
						{
#if DEBUG
							Debug.Assert(_isAttached == false);
							_isAttached = true;
#endif
							AttachedHandlers.Add(this);
							Attached(se);
						}
						else if(p.cmd == SciterBehaviors.INITIALIZATION_EVENTS.BEHAVIOR_DETACH)
						{
#if DEBUG
							Debug.Assert(_isAttached == true);
							_isAttached = false;
#endif
							AttachedHandlers.Remove(this);
							Detached(se);
						}
						return true;
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_SOM:
				{
					//SOM_PARAMS *p = (SOM_PARAMS *)prms;
                    SciterBehaviors.SOM_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.SOM_PARAMS>(ptr: prms);

                    if (p.cmd == SciterBehaviors.SOM_EVENTS.SOM_GET_PASSPORT)
                    {
	                    //	p->data.passport = pThis->asset_get_passport();
                    }
                    else if (p.cmd == SciterBehaviors.SOM_EVENTS.SOM_GET_ASSET)
                    {
	                    //	p->data.asset = static_cast<som_asset_t*>(pThis); // note: no add_ref
                    }

                    return false;
				}
				
				case SciterBehaviors.EVENT_GROUPS.HANDLE_MOUSE:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.MOUSE_PARAMS>(prms).ToEventArgs();
						return OnMouse(element: se, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_KEY:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.KEY_PARAMS>(prms).ToEventArgs();
						return OnKey(element: se, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_FOCUS:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.FOCUS_PARAMS>(prms).ToEventArgs();
						return OnFocus(element: se, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_DRAW:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.DRAW_PARAMS>(prms).ToEventArgs();
						return OnDraw(element: se, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_TIMER:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.TIMER_PARAMS>(prms);
						if(@params.timerId != IntPtr.Zero)
							return OnTimer(se, @params.timerId);
						return OnTimer(se);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_BEHAVIOR_EVENT:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.BEHAVIOR_EVENT_PARAMS>(prms);
						SciterElement se2 = @params.he != IntPtr.Zero ? new SciterElement(@params.he) : null;
						return OnEvent(se, se2, 
							@params.cmd, @params.reason, new SciterValue(@params.data));
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_METHOD_CALL:
					{
						var @params = Marshal.PtrToStructure<SciterXDom.METHOD_PARAMS>(prms);
						return OnMethodCall(se, @params.methodID);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_DATA_ARRIVED:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.DATA_ARRIVED_PARAMS>(prms);
						return OnDataArrived(se, @params);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_SCROLL:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.SCROLL_PARAMS>(prms).ToEventArgs();
						return OnScroll(element: se, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_SIZE:
					return OnSize(se);

				case SciterBehaviors.EVENT_GROUPS.HANDLE_SCRIPTING_METHOD_CALL:
					{
						IntPtr RESULT_OFFSET = Marshal.OffsetOf(typeof(SciterBehaviors.SCRIPTING_METHOD_PARAMS), nameof(SciterBehaviors.SCRIPTING_METHOD_PARAMS.result));
#if OSX
						if(IntPtr.Size == 4)
							Debug.Assert(RESULT_OFFSET.ToInt32() == 12);
#else
						if(IntPtr.Size == 4)
							Debug.Assert(RESULT_OFFSET.ToInt32() == 16);// yep 16, strange but is what VS C++ compiler says
#endif
						else if(IntPtr.Size == 8)
							Debug.Assert(RESULT_OFFSET.ToInt32() == 24);

						SciterBehaviors.SCRIPTING_METHOD_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.SCRIPTING_METHOD_PARAMS>(prms);
						SciterBehaviors.SCRIPTING_METHOD_PARAMS_WRAPPER pw = new SciterBehaviors.SCRIPTING_METHOD_PARAMS_WRAPPER(p);

						var methodInfo = GetType().GetMethod(pw.name);

						if (methodInfo == null)
							return false;

						var scriptResult = OnScriptCall(se, methodInfo, pw.args);
						
						if (scriptResult.IsSuccessful && scriptResult.Value != null)
						{
							//pw.result = scriptResult.Value;
							Interop.SciterValue.VALUE vres = scriptResult.Value.ToVALUE();
							IntPtr vptr = IntPtr.Add(prms, RESULT_OFFSET.ToInt32());
							Marshal.StructureToPtr(vres, vptr, false);
						}

						return scriptResult.IsSuccessful;
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_EXCHANGE:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.EXCHANGE_PARAMS>(prms).ToEventArgs();
						return OnExchange(element: se, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_GESTURE:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.GESTURE_PARAMS>(prms).ToEventArgs();
						return OnGesture(element: se, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_TISCRIPT_METHOD_CALL: //OBSOLETE
				default:
					Debug.Assert(false);
					return false;
			}
		}
	}
	
	
}