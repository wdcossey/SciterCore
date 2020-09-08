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
using System.Threading;
using System.Threading.Tasks;
using SciterCore.Interop;

namespace SciterCore
{
	public abstract class SciterEventHandler
	{
#if DEBUG
		private volatile bool _is_attached = false;
		~SciterEventHandler()
		{
			Debug.Assert(!_attached_handlers.Contains(this)); 
			Debug.Assert(_is_attached == false);
		}
#endif
		
		private static List<SciterEventHandler> _attached_handlers = new List<SciterEventHandler>();// we keep a copy of all attached instances to guard from GC removal

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
		
		// Overridables
		protected virtual void Subscription(
			SciterElement element, 
			out SciterBehaviors.EVENT_GROUPS event_groups)
		{
			event_groups = SciterBehaviors.EVENT_GROUPS.HANDLE_ALL;
		}

		protected virtual void Attached(
			SciterElement element)
		{
			
		}

		protected virtual void Detached(
			SciterElement element)
		{
			
		}

		protected virtual bool OnMouse(
			SciterElement element,
			MouseEventArgs args)
		{
			return false;
		}

		protected virtual bool OnKey(
			SciterElement element,
			KeyEventArgs args)
		{
			return false;
		}

		protected virtual bool OnFocus(
			SciterElement element,
			SciterBehaviors.FOCUS_PARAMS args)
		{
			return false;
		}

		protected virtual bool OnTimer(
			SciterElement element)
		{
			return false;
		}

		protected virtual bool OnTimer(
			SciterElement element,
			IntPtr extTimerId)
		{
			return false;
		}

		protected virtual bool OnSize(
			SciterElement element)
		{
			return false;
		}

		protected virtual bool OnDraw(
			SciterElement element,
			DrawEventArgs args)
		{
			return false;
		}

		protected virtual bool OnMethodCall(
			SciterElement element,
			SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodID)
		{
			return false;
		}
		
		protected virtual ScriptEventResult OnScriptCall(
			SciterElement element, 
			MethodInfo method, 
			SciterValue[] args)
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
						((Task<SciterValue>) method.Invoke(this, new object[] {element, args})).ContinueWith(task =>
						{
							if (task.IsFaulted)
								return;

							callbackFunc.Call(task.Result, SciterValue.Null);
						});
					}
					catch (Exception e)
					{
						callbackFunc.Call(SciterValue.Null, SciterValue.MakeError(e?.ToString()));
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
						object[] call_parameters = new object[] { element, args, null };
						bool res = (bool)method.Invoke(this, call_parameters);
						Debug.Assert(call_parameters[2] == null || call_parameters[2].GetType().IsAssignableFrom(typeof(SciterValue)));
						return ScriptEventResult.Successful(call_parameters[2] as SciterValue);
					}
				}
			}

			// not handled
			return ScriptEventResult.Failed();
		}

		protected virtual bool OnEvent(
			SciterElement sourceElement,
			SciterElement targetElement,
			SciterBehaviors.BEHAVIOR_EVENTS type,
			IntPtr reason,
			SciterValue data)
		{
			return false;
		}

		protected virtual bool OnDataArrived(
			SciterElement element,
			SciterBehaviors.DATA_ARRIVED_PARAMS prms)
		{
			return false;
		}
		
		protected virtual bool OnScroll(
			SciterElement element,
			SciterBehaviors.SCROLL_PARAMS prms)
		{
			return false;
		}

		protected virtual bool OnGesture(
			SciterElement element, 
			SciterBehaviors.GESTURE_PARAMS prms)
		{
			return false;
		}

		protected virtual bool OnExchange(
			SciterElement element,
			ExchangeEventArgs args)
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
						SciterBehaviors.EVENT_GROUPS groups;
						Subscription(se, out groups);
						Marshal.WriteInt32(prms, (int)groups);
						return true;
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_INITIALIZATION:
					{
						SciterBehaviors.INITIALIZATION_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.INITIALIZATION_PARAMS>(ptr: prms);
						if(p.cmd == SciterBehaviors.INITIALIZATION_EVENTS.BEHAVIOR_ATTACH)
						{
#if DEBUG
							Debug.Assert(_is_attached == false);
							_is_attached = true;
#endif
							_attached_handlers.Add(this);
							Attached(se);
						}
						else if(p.cmd == SciterBehaviors.INITIALIZATION_EVENTS.BEHAVIOR_DETACH)
						{
#if DEBUG
							Debug.Assert(_is_attached == true);
							_is_attached = false;
#endif
							_attached_handlers.Remove(this);
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
						var @params = Marshal.PtrToStructure<SciterBehaviors.MOUSE_PARAMS>(prms);
						return OnMouse(se, @params.Convert());
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_KEY:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.KEY_PARAMS>(prms);
						return OnKey(se, @params.Convert());
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_FOCUS:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.FOCUS_PARAMS>(prms);
						return OnFocus(se, @params);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_DRAW:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.DRAW_PARAMS>(prms);
						return OnDraw(se, @params.Convert());
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
						return OnEvent(se, se2, @params.cmd, @params.reason, new SciterValue(@params.data));
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
						var @params = Marshal.PtrToStructure<SciterBehaviors.SCROLL_PARAMS>(prms);
						return OnScroll(se, @params);
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
						SciterBehaviors.SCRIPTING_METHOD_PARAMS_Wraper pw = new SciterBehaviors.SCRIPTING_METHOD_PARAMS_Wraper(p);

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

				case SciterBehaviors.EVENT_GROUPS.HANDLE_TISCRIPT_METHOD_CALL:
					/*
					COMMENTED BECAUSE THIS EVENT IS NEVER USED, AND JUST ADDS MORE CONFUSION
					INSTEAD, IT'S BETTER TO HANDLE EVENT_GROUPS.HANDLE_SCRIPTING_METHOD_CALL/OnScriptCall
						{
							SciterXBehaviors.TISCRIPT_METHOD_PARAMS p = Marshal.PtrToStructure<SciterXBehaviors.TISCRIPT_METHOD_PARAMS>(prms);
							bool res = OnScriptCall(se, p);
							return res;
						}
					*/
					return false;

				case SciterBehaviors.EVENT_GROUPS.HANDLE_EXCHANGE:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.EXCHANGE_PARAMS>(prms);
						return OnExchange(se, @params.Convert());
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_GESTURE:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.GESTURE_PARAMS>(prms);
						return OnGesture(se, @params);
					}

				default:
					Debug.Assert(false);
					return false;
			}
		}
	}
	
	
}