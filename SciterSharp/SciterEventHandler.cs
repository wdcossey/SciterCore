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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
		{
			_proc = EventProc; 
			Name = this.GetType().FullName;
		}

		public SciterEventHandler(string name)
		{
			Name = name;
		}
		
		public string Name { get; set; }
		
		public readonly SciterBehaviors.FPTR_ElementEventProc _proc;// keep a copy of the delegate so it survives GC
		
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
			SciterBehaviors.MOUSE_PARAMS prms)
		{
			return false;
		}

		protected virtual bool OnKey(
			SciterElement element,
			SciterBehaviors.KEY_PARAMS prms)
		{
			return false;
		}

		protected virtual bool OnFocus(
			SciterElement element,
			SciterBehaviors.FOCUS_PARAMS prms)
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
			SciterBehaviors.DRAW_PARAMS prms)
		{
			return false;
		}

		protected virtual bool OnMethodCall(
			SciterElement element,
			SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodID)
		{
			return false;
		}
		
		protected virtual bool OnScriptCall(
			SciterElement element, 
			string name, 
			SciterValue[] args, 
			out SciterValue result)
		{
			result = null;

			var method = GetType().GetMethod(name);
			if(method != null)
			{
				// This base class tries to handle it by searching for a method with the same 'name'
				var mparams = method.GetParameters();

				// match signature:
				// 'void MethodName()' or 'SciterValue MethodName()'
				{
					if(mparams.Length == 0 && 
						(method.ReturnType == typeof(void) || method.ReturnType == typeof(SciterValue)))
					{
						var ret = method.Invoke(this, null);
						if(method.ReturnType == typeof(SciterValue))
							result = (SciterValue)ret;
						return true;
					}
				}

				// match signature:
				// 'void MethodName(SciterValue[] args)' or 'SciterValue MethodName(SciterValue[] args)'
				{
					if(mparams.Length==1 && mparams[0].ParameterType.Name == "SciterValue[]" &&
						(method.ReturnType == typeof(void) || method.ReturnType == typeof(SciterValue)))
					{
						object[] call_parameters = new object[] { args };
						var ret = method.Invoke(this, call_parameters);
						if(method.ReturnType == typeof(SciterValue))
							result = (SciterValue)ret;
						return true;
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
						result = call_parameters[2] as SciterValue;
						return res;
					}
				}
			}

			// not handled
			return false;
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
			SciterBehaviors.EXCHANGE_PARAMS prms)
		{
			return false;
		}

		// EventProc
		private bool EventProc(IntPtr tag, IntPtr he, uint evtg, IntPtr prms)
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
						SciterBehaviors.INITIALIZATION_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.INITIALIZATION_PARAMS>(prms);
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

				case SciterBehaviors.EVENT_GROUPS.HANDLE_MOUSE:
					{
						SciterBehaviors.MOUSE_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.MOUSE_PARAMS>(prms);
						return OnMouse(se, p);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_KEY:
					{
						SciterBehaviors.KEY_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.KEY_PARAMS>(prms);
						return OnKey(se, p);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_FOCUS:
					{
						SciterBehaviors.FOCUS_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.FOCUS_PARAMS>(prms);
						return OnFocus(se, p);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_DRAW:
					{
						SciterBehaviors.DRAW_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.DRAW_PARAMS>(prms);
						return OnDraw(se, p);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_TIMER:
					{
						SciterBehaviors.TIMER_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.TIMER_PARAMS>(prms);
						if(p.timerId != IntPtr.Zero)
							return OnTimer(se, p.timerId);
						return OnTimer(se);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_BEHAVIOR_EVENT:
					{
						SciterBehaviors.BEHAVIOR_EVENT_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.BEHAVIOR_EVENT_PARAMS>(prms);
						SciterElement se2 = p.he != IntPtr.Zero ? new SciterElement(p.he) : null;
						return OnEvent(se, se2, p.cmd, p.reason, new SciterValue(p.data));
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_METHOD_CALL:
					{
						SciterXDom.METHOD_PARAMS p = Marshal.PtrToStructure<SciterXDom.METHOD_PARAMS>(prms);
						return OnMethodCall(se, p.methodID);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_DATA_ARRIVED:
					{
						SciterBehaviors.DATA_ARRIVED_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.DATA_ARRIVED_PARAMS>(prms);
						return OnDataArrived(se, p);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_SCROLL:
					{
						SciterBehaviors.SCROLL_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.SCROLL_PARAMS>(prms);
						return OnScroll(se, p);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_SIZE:
					return OnSize(se);

				case SciterBehaviors.EVENT_GROUPS.HANDLE_SCRIPTING_METHOD_CALL:
					{
						IntPtr RESULT_OFFSET = Marshal.OffsetOf(typeof(SciterBehaviors.SCRIPTING_METHOD_PARAMS), "result");
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

						bool bOK = OnScriptCall(se, pw.name, pw.args, out pw.result);
						if(bOK && pw.result != null)
						{
							Interop.SciterValue.VALUE vres = pw.result.ToVALUE();
							IntPtr vptr = IntPtr.Add(prms, RESULT_OFFSET.ToInt32());
							Marshal.StructureToPtr(vres, vptr, false);
						}

						return bOK;
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
						SciterBehaviors.EXCHANGE_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.EXCHANGE_PARAMS>(prms);
						return OnExchange(se, p);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_GESTURE:
					{
						SciterBehaviors.GESTURE_PARAMS p = Marshal.PtrToStructure<SciterBehaviors.GESTURE_PARAMS>(prms);
						return OnGesture(se, p);
					}

				default:
					Debug.Assert(false);
					return false;
			}
		}
	}
}