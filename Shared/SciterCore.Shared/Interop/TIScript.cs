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
using System.Runtime.InteropServices;

namespace SciterCore.Interop
{
	public static class TIScript
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct SCITER_TI_SCRIPT_API
		{
			public CREATE_VM create_vm;
			public DESTROY_VM destroy_vm;
			public DUMMY invoke_gc;
			public DUMMY set_std_streams;
			public GET_CURRENT_VM get_current_vm;
			public GET_GLOBAL_NS get_global_ns;
			public DUMMY get_current_ns;

			public DUMMY is_int;
			public DUMMY is_float;
			public DUMMY is_symbol;
			public IS_STRING is_string;
			public DUMMY is_array;
			public DUMMY is_object;
			public DUMMY is_native_object;
			public DUMMY is_function;
			public DUMMY is_native_function;
			public DUMMY is_instance_of;
			public IS_UNDEFINED is_undefined;
			public DUMMY is_nothing;
			public DUMMY is_null;
			public DUMMY is_true;
			public DUMMY is_false;
			public DUMMY is_class;
			public DUMMY is_error;
			public DUMMY is_bytes;
			public DUMMY is_datetime;

			public DUMMY get_int_value;
			public DUMMY get_float_value;
			public DUMMY get_bool_value;
			public DUMMY get_symbol_value;
			public DUMMY get_string_value;
			public DUMMY get_bytes;
			public DUMMY get_datetime;

			public DUMMY nothing_value;
			public DUMMY undefined_value;
			public DUMMY null_value;
			public DUMMY bool_value;
			public DUMMY int_value;
			public DUMMY float_value;
			public DUMMY string_value;
			public DUMMY symbol_value;
			public DUMMY bytes_value;
			public DUMMY datetime_value;

			public DUMMY to_string;

			// define native class
			public DUMMY define_class;

			// object
			public DUMMY create_object;
			public DUMMY set_prop;
			public DUMMY get_prop;
			public DUMMY for_each_prop;
			public DUMMY get_instance_data;
			public DUMMY set_instance_data;

			// array
			public DUMMY create_array;
			public DUMMY set_elem;
			public DUMMY get_elem;
			public DUMMY set_array_size;
			public DUMMY get_array_size;

			// eval
			public DUMMY eval;
			public EVAL_STRING eval_string;

			// call function (method)
			public DUMMY call;

			// compiled bytecodes
			public DUMMY compile;
			public DUMMY loadbc;

			// throw error
			public DUMMY throw_error;

			// arguments access
			public DUMMY get_arg_count;
			public DUMMY get_arg_n;

			// path here is global "path" of the object, something like
			// "one"
			// "one.two", etc.
			public GET_VALUE_BY_PATH get_value_by_path;

			// pins
			public DUMMY pin;
			public DUMMY unpin;

			// create native_function_value and native_property_value,
			// use this if you want to add native functions/properties in runtime to exisiting classes or namespaces (including global ns)
			public DUMMY native_function_value;
			public DUMMY native_property_value;

			// Schedule execution of the pfunc(prm) in the thread owning this VM.
			// Used when you need to call scripting methods from threads other than main (GUI) thread
			// It is safe to call tiscript functions inside the pfunc.
			// returns 'true' if scheduling of the call was accepted, 'false' when failure (VM has no dispatcher attached).
			public DUMMY post;

			public DUMMY set_remote_std_streams;

			// support of multi-return values from native fucntions, n here is a number 1..64
			public DUMMY make_val_list;

			// returns number of props in object, elements in array, or bytes in byte array.
			public GET_LENGTH get_length;
			// for( var val in coll ) {...}
			public DUMMY get_next;
			// for( var (key,val) in coll ) {...}
			public DUMMY get_next_key_value;

			// associate extra data pointer with the VM
			public DUMMY set_extra_data;
			public DUMMY get_extra_data;


			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void DUMMY();

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate IntPtr CREATE_VM(uint features = 0xffffffff, uint heapSize = 1024 * 1024, uint stackSize = 64 * 1024);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void DESTROY_VM(IntPtr pvm);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate IntPtr GET_CURRENT_VM();

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate tiscript_value GET_GLOBAL_NS(IntPtr tiScriptVmPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate bool IS_STRING(ref tiscript_value v);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate bool IS_UNDEFINED(ref tiscript_value v);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate bool EVAL_STRING(IntPtr tiScriptVmPtr, tiscript_value ns, [MarshalAs(UnmanagedType.LPWStr)]string script, uint scriptLength, out tiscript_value pretval);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate bool GET_VALUE_BY_PATH(IntPtr tiScriptVmPtr, out tiscript_value ns, [MarshalAs(UnmanagedType.LPStr)]string path);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate int GET_LENGTH(IntPtr tiScriptVmPtr, tiscript_value obj);
			
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct tiscript_value
		{
			ulong value;

			public bool IsString { get { return Sciter.ScriptApi.is_string(ref this); } }
			public bool IsUndefined { get { return Sciter.ScriptApi.is_undefined(ref this); } }
			public int Length { get { return Sciter.ScriptApi.get_length(Sciter.ScriptApi.get_current_vm(), this); } }
		}
	}
}