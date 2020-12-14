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
using SciterCore.Attributes;
using SciterCore.Interop;
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnusedParameter.Global

namespace SciterCore
{
	public abstract class SciterEventHandler
	{
		
#if DEBUG
		
		private volatile bool _isAttached = false;

		protected SciterElement Element { get; set; } = null;
		
		protected SciterHost Host { get; private set;  } = null;
		
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

		internal SciterEventHandler UpdateHost(SciterHost sciterHost)
		{
			Host = sciterHost;
			return this;
		}
		
		#region Protected Virtual

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
			return ScriptExecutioner
				.Create(this, element, method, args)
				.Execute();
		}
		
		/// <summary>
		/// This method is typically used with WinForms applications
		/// </summary>
		/// <param name="element"></param>
		/// <param name="methodName"></param>
		/// <param name="args"></param>
		// TODO: Should be specific to WinForms?
		protected virtual ScriptEventResult OnScriptCall(SciterElement element, string methodName, SciterValue[] args)
		{
			return ScriptEventResult.Failed();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceElement"><para>Source element e.g. in SELECTION_CHANGED it is new selected &lt;option&gt;, in MENU_ITEM_CLICK it is menu item (LI) element</para></param>
		/// <param name="targetElement"><para>Target element, in MENU_ITEM_CLICK this is owner element that caused this menu - e.g. context menu owner<br/>In scripting this field named as Event.owner</para></param>
		/// <param name="eventType"></param>
		/// <param name="reason"><para>CLICK_REASON or EDIT_CHANGED_REASON - UI action causing change.<br/>In case of custom event notifications this may be any application specific value.</para></param>
		/// <param name="data"><para>Auxiliary data accompanied with the event. E.g. FORM_SUBMIT event is using this field to pass collection of values.</para></param>
		/// <param name="eventName"><para>name of custom event (when <paramref name="eventType"/> == <see cref="SciterBehaviors.BEHAVIOR_EVENTS.CUSTOM"/>)</para></param>
		/// <returns></returns>
		protected virtual bool OnEvent(SciterElement sourceElement, SciterElement targetElement,
			BehaviorEvents eventType, IntPtr reason, SciterValue data, string eventName)
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

		#endregion
		
		// EventProc

		private bool EventProcMethod(IntPtr tag, IntPtr he, uint evtg, IntPtr prms)
		{

			SciterElement source = null;
			if(he != IntPtr.Zero)
				source = new SciterElement(he);

			switch((SciterBehaviors.EVENT_GROUPS)evtg)
			{
				case SciterBehaviors.EVENT_GROUPS.SUBSCRIPTIONS_REQUEST:
					{
						var groups = SubscriptionsRequest(source);
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
							Attached(source);
						}
						else if(p.cmd == SciterBehaviors.INITIALIZATION_EVENTS.BEHAVIOR_DETACH)
						{
#if DEBUG
							Debug.Assert(_isAttached == true);
							_isAttached = false;
#endif
							
							Detached(source);
							AttachedHandlers.Remove(this);
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
						return OnMouse(element: source, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_KEY:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.KEY_PARAMS>(prms).ToEventArgs();
						return OnKey(element: source, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_FOCUS:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.FOCUS_PARAMS>(prms).ToEventArgs();
						return OnFocus(element: source, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_DRAW:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.DRAW_PARAMS>(prms).ToEventArgs();
						return OnDraw(element: source, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_TIMER:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.TIMER_PARAMS>(prms);
						if(@params.timerId != IntPtr.Zero)
							return OnTimer(source, @params.timerId);
						return OnTimer(source);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_BEHAVIOR_EVENT:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.BEHAVIOR_EVENT_PARAMS>(prms);
						SciterElement target = @params.he != IntPtr.Zero ? new SciterElement(@params.he) : null;
						
						switch (@params.cmd)
						{
							case SciterBehaviors.BEHAVIOR_EVENTS.DOCUMENT_CREATED:
								Element = target;
								break;
							case SciterBehaviors.BEHAVIOR_EVENTS.DOCUMENT_CLOSE:
								Element = null;
								break;
						}
						
						//
						return OnEvent(sourceElement: source, targetElement: target, 
							eventType: (BehaviorEvents)(int)@params.cmd, reason: @params.reason, 
							data: new SciterValue(@params.data), eventName: @params.name);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_METHOD_CALL:
					{
						var @params = Marshal.PtrToStructure<SciterXDom.METHOD_PARAMS>(prms);
						return OnMethodCall(source, @params.methodID);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_DATA_ARRIVED:
					{
						var @params = Marshal.PtrToStructure<SciterBehaviors.DATA_ARRIVED_PARAMS>(prms);
						return OnDataArrived(source, @params);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_SCROLL:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.SCROLL_PARAMS>(prms).ToEventArgs();
						return OnScroll(element: source, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_SIZE:
					return OnSize(source);

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
						
						var scriptResult = OnScriptCall(source, pw.name, pw.args);

						//TODO: Clean this up!
						if (scriptResult.IsSuccessful)
							return scriptResult.IsSuccessful;
						
						var methodInfo = GetType().GetMethod(pw.name) ?? GetType().GetMethods().SingleOrDefault(s => s.GetCustomAttributes<SciterFunctionNameAttribute>().Any(a => a.FunctionName.Equals(pw.name)));

						if (methodInfo == null)
							return false;

						scriptResult = OnScriptCall(source, methodInfo, pw.args);
						
						if (scriptResult.IsSuccessful)
						{
							//pw.result = scriptResult.Value;
							var vres = (scriptResult.Value ?? SciterValue.Null).ToVALUE();
							var vptr = IntPtr.Add(prms, RESULT_OFFSET.ToInt32());
							Marshal.StructureToPtr(vres, vptr, false);
						}

						return scriptResult.IsSuccessful;
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_EXCHANGE:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.EXCHANGE_PARAMS>(prms).ToEventArgs();
						return OnExchange(element: source, args: args);
					}

				case SciterBehaviors.EVENT_GROUPS.HANDLE_GESTURE:
					{
						var args = Marshal.PtrToStructure<SciterBehaviors.GESTURE_PARAMS>(prms).ToEventArgs();
						return OnGesture(element: source, args: args);
					}

				//case SciterBehaviors.EVENT_GROUPS.HANDLE_TISCRIPT_METHOD_CALL: //Obsolete
				default:
					Debug.Assert(false);
					return false;
			}
		}
	}
	
	
}