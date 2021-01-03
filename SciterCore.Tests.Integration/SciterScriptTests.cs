#if TI_SCRIPT
using System;
using NUnit.Framework;
using SciterCore.Interop;

namespace SciterCore.Tests.Integration
{
    [TestFixture]
    public class SciterScriptTests
    {
        private SciterWindow _sciterWindow;
        private IntPtr _currentVm;

        [SetUp]
        public void Setup()
        {
            _sciterWindow = 
                new SciterWindow()
                    .CreateMainWindow(320, 240)
                    .SetTitle(nameof(SciterGraphicsTests));

            _currentVm = Sciter.ScriptApi.GetCurrentVM();
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void Get_the_current_VM()
        {
	        var windowVm = Sciter.Api.SciterGetVM(_sciterWindow.Handle);
	        var apiVm = Sciter.ScriptApi.GetCurrentVM();
	        Assert.AreEqual(windowVm, apiVm);
				
	        
				/*
				//var a = Sciter.ScriptApi.GetSymbolValue(y, out var psz);
				//var b = Sciter.ScriptApi.GetStringValue(y, out var pdata, out var length);
				//var c = Sciter.ScriptApi.GetBytes(y, out var bytes, out var pbLen);
				//var d = Sciter.ScriptApi.ToString(x, y);

			
				*/
        }

        [Test]
        public void Creating_a_new_VM_does_not_throw()
        {
	        Assert.DoesNotThrow(() =>
	        {
		        var newVm = Sciter.ScriptApi.CreateVM();
		        Assert.AreNotEqual(newVm, IntPtr.Zero);
		        Sciter.ScriptApi.DestroyVM(newVm);
	        });
        }

        [Test]
        public void Calling_garbage_collection_does_not_throw()
        {
	        Assert.DoesNotThrow(() => Sciter.ScriptApi.InvokeGC(_currentVm));
        }

        [Test]
        public void Validate_global_Namespace()
        {
	        var globalNamespace = Sciter.ScriptApi.GetGlobalNS(_currentVm);
	        Assert.AreNotEqual(globalNamespace, IntPtr.Zero);
        }

        [Test]
        public void Validate_current_Namespace()
        {
	        var currentNamespace = Sciter.ScriptApi.GetCurrentNS(_currentVm);
	        Assert.AreNotEqual(currentNamespace, IntPtr.Zero);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Set_and_get_script_boolean_value(bool expected)
        {
	        var result = Sciter.ScriptApi.GetBoolValue(Sciter.ScriptApi.BoolValue(expected), out var actual);
	        Assert.IsTrue(result);
	        Assert.AreEqual(expected, actual);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void Set_and_get_script_int_value(int expected)
        {
	        var result = Sciter.ScriptApi.GetIntValue(Sciter.ScriptApi.IntValue(expected), out var actual);
	        Assert.IsTrue(result);
	        Assert.AreEqual(expected, actual);
        }

        [TestCase(-1f)]
        [TestCase(0f)]
        [TestCase(1f)]
        [TestCase(double.MaxValue)]
        [TestCase(double.MinValue)]
        public void Set_and_get_script_float_value(double expected)
        {
	        var result = Sciter.ScriptApi.GetFloatValue(Sciter.ScriptApi.FloatValue(expected), out var actual);
	        Assert.IsTrue(result);
	        Assert.AreEqual(expected, actual);
        }

        [TestCase("Test string")]
        [TestCase("\"\t\r\n")]
        [TestCase("")]
        public void Set_and_get_script_string_value(string expected)
        {
	        
	        var result = Sciter.ScriptApi.GetStringValue(
		        Sciter.ScriptApi.StringValue(_currentVm, expected), 
		        out var actual, 
		        out var actualLength);
	        
	        Assert.IsTrue(result);
	        Assert.AreEqual(expected, actual);
	        Assert.AreEqual(expected?.Length, actualLength);
        }

        [TestCase("Test string")]
        [TestCase("\"\t\r\n")]
        [TestCase("")]
        public void Set_and_get_script_symbol_value(string expected)
        {
	        var result = Sciter.ScriptApi.GetSymbolValue(
		        Sciter.ScriptApi.SymbolValue(expected), 
		        out var actual);
	        
	        Assert.IsTrue(result);
	        Assert.AreEqual(expected, actual);
        }

        [TestCase(new byte[6] {byte.MinValue, 1, 2, 3, 4, byte.MaxValue})]
        [TestCase(new byte[6] {byte.MaxValue, 4, 3, 2, 1, byte.MinValue})]
        public void Set_and_get_script_bytes_value(byte[] expected)
        {
	        //System.Text.Encoding.Unicode.GetBytes("this is my string value!")
	        var result = Sciter.ScriptApi.GetBytes(
		        Sciter.ScriptApi.BytesValue(_currentVm, expected), 
		        out var actual, 
		        out var actualLength);
	        
	        Assert.IsTrue(result);
	        Assert.AreEqual(expected, actual);
	        Assert.AreEqual(expected?.Length, actualLength);
        }

        [TestCase("input string!")]
        [TestCase("\"\t\r\n")]
        public void Set_and_get_script_bytes_value_from_string(string expectedString)
        {
	        var expected = System.Text.Encoding.Unicode.GetBytes(expectedString);
	        
	        var result = Sciter.ScriptApi.GetBytes(
		        Sciter.ScriptApi.BytesValue(_currentVm, expected), 
		        out var actual, 
		        out var actualLength);
	        
	        Assert.IsTrue(result);
	        Assert.AreEqual(expected, actual);
	        Assert.AreEqual(expected?.Length, actualLength);
        }
        
        [Test]
        public void Set_and_get_script_datetime_value_utc()
        {
	        var expected = new DateTime(1960, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	        
	        var result = Sciter.ScriptApi.GetDateTime(
		        _currentVm, 
		        Sciter.ScriptApi.DateTimeValue(_currentVm, expected), 
		        out var actual);
	        
	        Assert.IsTrue(result);
	        Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void Nothing_value()
        {
	        var actual = Sciter.ScriptApi.NothingValue();
	        Assert.IsTrue(actual.IsNothing);
	        Assert.IsTrue(Sciter.ScriptApi.IsNothing(actual));
        }
        
        [Test]
        public void Null_value()
        {
	        var actual = Sciter.ScriptApi.NullValue();
	        Assert.IsTrue(actual.IsNull);
	        Assert.IsTrue(Sciter.ScriptApi.IsNull(actual));
        }
        
        [Test]
        public void Undefined_value()
        {
	        var actual = Sciter.ScriptApi.UndefinedValue();
	        Assert.IsTrue(actual.IsUndefined);
	        Assert.IsTrue(Sciter.ScriptApi.IsUndefined(actual));
        }
        
/*
		private SciterScript.Callbacks.tiscript_method bleh = (ptr) => Sciter.ScriptApi.StringValue(ptr, "bleh");
		
		//var vmPtr = Sciter.ScriptApi.GetCurrentVM();
        //var globalNS = Sciter.ScriptApi.GetGlobalNS(vmPtr);
        //Sciter.ScriptApi.EvalString(vmPtr, globalNS, "System.OS", out var stringValue);
                
        [Test]
        public void METHOD1()
        {
	        //var vmPtr = Sciter.Api.SciterGetVM(this.Host.Window.Handle);
            var vmPtr = Sciter.ScriptApi.GetCurrentVM();

			var globalNS = Sciter.ScriptApi.GetGlobalNS(vmPtr);

			var pinValue = new SciterScript.PinnedScriptValue(vmPtr, globalNS);
			
			Sciter.ScriptApi.Pin(vmPtr, pinValue);
			
			Sciter.ScriptApi.Unpin(pinValue);
			
			//Sciter.ScriptApi.Eval(vmPtr, Sciter.ScriptApi.GetGlobalNS(vmPtr), IntPtr.Zero, true, out var bleh2);

			var gpvp = Sciter.ScriptApi.DefineGlobalProperty(vmPtr, new SciterScript.tiscript_prop_def()
			{
				name = "helloWorld",
				getter = (ptr, value) => Sciter.ScriptApi.SymbolValue("bleh"),
				setter = (ptr, value, tiscriptValue) => { throw new NotImplementedException(); },
				
			}, globalNS);

			var mDef = new SciterScript.tiscript_method_def()
			{
				name = "funcMe",
				dispatch = IntPtr.Zero,
				tag = IntPtr.Zero,
				handler = this.bleh,
				payload = IntPtr.Zero,
			};
			
			var gpvf = Sciter.ScriptApi.DefineGlobalFunction(vmPtr, mDef, globalNS);
			
			//var bFunc = Sciter.ScriptApi.CallFunction(vmPtr, globalNS, "funcMe", out var funcValue);

			return result;
			
			
			var a = Sciter.ScriptApi.CallFunction(vmPtr, globalNS, "callmeFromCSharp", out var stringValue,
                Sciter.ScriptApi.StringValue(vmPtr, "http://localhost"));
			
            Sciter.ScriptApi.GetSymbolValue(stringValue, out var bleh);
            Sciter.ScriptApi.GetStringValue(stringValue, out var sStr, out var sLen);
			
			var b = Sciter.ScriptApi.CallFunction(vmPtr, globalNS, "testing", out stringValue);
			
            Sciter.ScriptApi.GetSymbolValue(stringValue, out bleh);
            Sciter.ScriptApi.GetStringValue(stringValue, out sStr, out sLen);

            var valueByPath = Sciter.ScriptApi.GetValueByPath(vmPtr, out var ns, "self.ns");
            
            var bLoop = Sciter.ScriptApi.ForEachProp(vmPtr, globalNS, ForEachCallback, IntPtr.Zero);

            var bEvalString = Sciter.ScriptApi.EvalString(vmPtr, globalNS, input.AsString(), out var retValue);
            
			//while (bLoop)
			//{
			//     bLoop = Sciter.ScriptApi.ForEachProp(vmPtr, globalNS, ForEachCallback, IntPtr.Zero);
			//}

			var argCount = Sciter.ScriptApi.GetArgCount(vmPtr);
			
			for (int i = 0; i < argCount - 1; i++)
            {
				var argN = Sciter.ScriptApi.GetArgN(vmPtr, i);

                if (Sciter.ScriptApi.IsClass(vmPtr, argN))
                {
	                
	                Sciter.ScriptApi.ForEachProp(vmPtr, argN, (ptr, key, value, tag) =>
	                {
		                //var prop = Sciter.ScriptApi.GetProp(vmPtr, argN, key);
		                
		                Sciter.ScriptApi.GetSymbolValue(key, out var keySymbol);
		                _logger.LogTrace($"Class:Key: {keySymbol}");
		                
		                Sciter.ScriptApi.GetSymbolValue(value, out var valSymbol);
		                _logger.LogTrace($"Class:Value: {valSymbol}");


		                return true;
	                }, IntPtr.Zero);
	                
                    _logger.LogTrace($"GetArraySize: {Sciter.ScriptApi.GetArraySize(vmPtr, argN)}");
                }

				Sciter.ScriptApi.GetSymbolValue(argN, out bleh);
				_logger.LogTrace(bleh);

                Sciter.ScriptApi.GetStringValue(argN, out bleh, out var pLength);
				_logger.LogTrace(bleh);

                Sciter.ScriptApi.GetIntValue(argN, out var intVal);
				_logger.LogTrace($"{intVal}");
			}

			//Sciter.ScriptApi.ThrowError(vmPtr, "bleh");
			//Sciter.ScriptApi.ThrowError(vmPtr, "hmmmm");
        }
        
        private bool ForEachCallback(IntPtr vmptr, SciterScript.ScriptValue key, SciterScript.ScriptValue value, IntPtr tag)
        {
			Sciter.ScriptApi.GetSymbolValue(key, out var keySymbol);

            string valueString = string.Empty;

            //if (value.IsSymbol)
            Sciter.ScriptApi.GetSymbolValue(value, out valueString);
            //else if (value.IsString)
            //    Sciter.ScriptApi.GetStringValue(value, out valueString, out _);
            //else if (value.IsInt)
            //{
            //    Sciter.ScriptApi.GetIntValue(value, out var intValue);
            //    valueString = $"{intValue}";
            //}
            //else if (value.IsFloat)
            //{
            //    Sciter.ScriptApi.GetFloatValue(value, out var fValue);
            //    valueString = $"{fValue}";
            //}

            _logger.LogTrace($"ForEachProp: {keySymbol} ({value.Length}): {valueString}");

            return false;
		}
*/
    }
}
#endif