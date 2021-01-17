using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SciterCore.Convert;

namespace SciterCore.Windows.Tests.Unit
{
    public class SciterValueTests
    {
        [SetUp]
        public void SetUp()
        {
            
        }
        
        [Test]
        public void Value_Clear()
        {
            var actual = SciterValue
                .Create("some value to discard")
                .Clear();

            Assert.IsTrue(actual.IsUndefined);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Value_from_Boolean(bool input)
        {
            var actual = SciterValue.Create(input);
            
            Assert.IsTrue(actual.IsBool);
            Assert.AreEqual(input, actual.AsBoolean(!input));
        }
        
        [Test]
        public void Value_is_not_a_Boolean()
        {
            var actual = SciterValue.Create("string");
            
            Assert.IsFalse(actual.IsBool);
            Assert.AreEqual(false, actual.AsBoolean());
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public void Value_from_Int32(int input)
        {
            var actual = SciterValue.Create(input);
            
            Assert.IsTrue(actual.IsInt);
            Assert.AreEqual(input, actual.AsInt32());
        }

        [TestCase(uint.MinValue)]
        [TestCase(uint.MaxValue)]
        public void Value_from_UInt32(uint input)
        {
            var actual = SciterValue.Create(input);
            
            Assert.IsTrue(actual.IsInt);
            Assert.AreEqual(input, actual.AsUInt32());
        }

        [TestCase(double.MinValue)]
        [TestCase(double.MaxValue)]
        public void Value_from_Double(double input)
        {
            var actual = SciterValue.Create(input);
            
            Assert.IsTrue(actual.IsFloat);
            Assert.AreEqual(input, actual.AsDouble());
        }

        [TestCase(-360)]
        [TestCase(0)]
        [TestCase(360)]
        public void Value_from_Angle_as_Double(double input)
        {
            var actual = SciterValue.MakeAngle(input);
            
            Assert.IsTrue(actual.IsAngle);
            Assert.AreEqual(input, actual.AsAngle());
        }
        
        [Test]
        public void Value_is_not_an_Angle()
        {
            var actual = SciterValue.Create("string");
            
            Assert.IsFalse(actual.IsAngle);

            Assert.Throws<InvalidOperationException>(() => actual.AsAngle());
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1_000_000_000)]
        public void Value_from_Currency_as_Int64(long input)
        {
            var actual = SciterValue.MakeCurrency(input);
            
            Assert.IsTrue(actual.IsCurrency);
            Assert.AreEqual(input, actual.AsCurrency());
        }
        
        [Test]
        public void Value_is_not_an_Currency()
        {
            var actual = SciterValue.Create("string");
            
            Assert.IsFalse(actual.IsCurrency);

            Assert.Throws<InvalidOperationException>(() => actual.AsCurrency());
        }
        
        [TestCase("this is a string")]
        [TestCase("")]
        [TestCase(null)]
        public void Value_from_String(string input)
        {
            var actual = SciterValue.Create(input);
            
            Assert.IsTrue(input == null ? actual.IsUndefined : actual.IsString);
            Assert.AreEqual(input, actual.AsString());
        }

        [TestCase(new byte[] { 1, 2, 3, 4, 5 })]
        [TestCase(new byte[0])]
        [TestCase(null)]
        public void Value_from_ByteArray(byte[] input)
        {
            var actual = SciterValue.Create(input);
            
            Assert.IsTrue(actual.IsBytes);
            Assert.AreEqual(input ?? new byte[0], actual.AsBytes());
        }
        
        [Test]
        public void Value_is_not_a_ByteArray()
        {
            var actual = SciterValue.Create("Not a Byte[]");
            Assert.IsNull(actual.AsBytes());
        }
        
        [TestCase(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc)]
        [TestCase(2020, 1, 1, 12, 0, 0, DateTimeKind.Local)]
        public void Value_from_DateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
        {
            var input = new DateTime(year, month, day, hour, minute, second, kind);
            
            var actual = SciterValue.MakeDateTime(input);
            
            Assert.IsTrue(actual.IsDate);
            Assert.AreEqual(input, actual.AsDateTime(kind == DateTimeKind.Utc));
        }
        
        [TestCase(0.0d)]
        [TestCase(0.5d)]
        [TestCase(1000d)]
        public void Value_from_Duration(double duration)
        {
            var actual = SciterValue.MakeDuration(duration);
            
            Assert.IsTrue(actual.IsDuration);
            Assert.AreEqual(duration, actual.AsDuration());
        }
        
        [Test]
        public void Value_is_not_a_Duration()
        {
            var actual = SciterValue.Create("Not a Duration");
            Assert.Throws<InvalidOperationException>(() => actual.AsDuration());
        }

        [TestCase(255, 0, 255)]
        [TestCase(0, 0, 0)]
        [TestCase(255, 255, 255)]
        public void Value_from_SciterColor_RGB(byte r, byte g, byte b)
        {
            var color = SciterColor.Create(r, g, b);
            
            var actual = SciterValue.Create(color);
            
            Assert.IsTrue(actual.IsColor);
            Assert.AreEqual(color, actual.AsColor());
        }
        
        [TestCase(255, 0, 255, 0)]
        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 0, 0, 255)]
        [TestCase(255, 255, 255, 255)]
        [TestCase(127, 127, 127, 127)]
        public void Value_from_SciterColor_RGBA(byte r, byte g, byte b, byte a)
        {
            var color = SciterColor.Create(r, g, b, a);
            
            var actual = SciterValue.Create(color);
            
            Assert.IsTrue(actual.IsColor);
            Assert.AreEqual(color, actual.AsColor());
        }
        
        [Test]
        public void Value_is_not_a_SciterColor()
        {
            var actual = SciterValue.Create("string");
            
            Assert.IsFalse(actual.IsColor);

            Assert.Throws<InvalidOperationException>(() => actual.AsColor());
        }
        
        [Test]
        public void Value_from_ValueArray()
        {
            var input = new List<SciterValue>
            {
                SciterValue.Create(1),
                SciterValue.Create(1u),
                SciterValue.Create(1d),
                SciterValue.Create("string"),
                SciterValue.Create(DateTime.Now),
                SciterValue.Create(true)
            };
            
            var actual = SciterValue.Create(input.ToArray());
            
            Assert.IsTrue(actual.IsArray);
            Assert.AreEqual(input.Count, actual.Length);
            Assert.AreEqual(input[0].AsInt32(-1), actual.GetItem(0).AsInt32());
            Assert.AreEqual(input[1].AsDouble(-1u), actual.GetItem(1).AsUInt32());
            Assert.AreEqual(input[2].AsDouble(-1d), actual.GetItem(2).AsDouble());
            Assert.AreEqual(input[3].AsString("exp"), actual.GetItem(3).AsString("actual"));
            Assert.AreEqual(input[4].AsDateTime(), actual.GetItem(4).AsDateTime());
            Assert.AreEqual(input[5].AsBoolean(), actual.GetItem(5).AsBoolean(true));
        }
        
        [Test]
        public void Value_from_Object()
        {
            var list = new List<int> { 1, 2, 3 };
            var obj = new { list = list };
            
            var actual = SciterValue.Create(obj);

            Assert.IsTrue(actual.IsMap);
            Assert.IsTrue(actual["list"].IsArray);
            Assert.AreEqual(list, actual["list"].AsEnumerable().Select(s => s.AsInt32()));
        }

        [Test]
        public void Value_from_IEnumerable()
        {
            var list = new List<int> { 1, 2, 3 };
            var actual = SciterValue.FromList(list);

            Assert.IsTrue(actual.IsArray);
            Assert.AreEqual(list, actual.AsEnumerable().Select(s => s.AsInt32()));
        }

        [Test]
        public void Value_is_not_an_IEnumerable()
        {
            var actual = SciterValue.Create("Not a IEnumerable<>");
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<InvalidOperationException>(() =>actual.AsEnumerable().ToList());
        }
        
        [Test]
        public void Value_from_Object_recursion_too_deep()
        {
            var obj = new { a = new { b = new { c = new { d = new { e = new { f = new { g = new { h = new { i = new { j = new { k = new { }}}}}}}}}}}};
            Assert.Throws<InvalidOperationException>(() => SciterValue.Create(obj));
        }

        [Test]
        public void Value_from_IConvertible_Dictionary()
        {
            var dictionary = new Dictionary<string, IConvertible>
            {
                { "null", null},
                { "bool", true},
                { "byte", (byte)1},
                { "int", 1},
                { "string", "string"},
                { "double", 0d},
                { "uint", 0u},
                { "decimal", 0m},
                { "dateTime", DateTime.UtcNow.ToFileTime()},
                { "color", SciterColor.Khaki}
            };
            
            var actual = SciterValue.Create(dictionary);

            Assert.IsTrue(actual.IsMap);

            Assert.AreEqual(dictionary, actual.AsDictionary());
        }

        [Test]
        public void Value_is_not_an_IConvertible_Dictionary()
        {
            var actual = SciterValue.Create("Not a Dictionary<>");
            Assert.Throws<InvalidOperationException>(() => actual.AsDictionary());
        }

        [Test]
        public void Value_from_Dictionary()
        {
            var dictionary = new Dictionary<string, SciterValue>
            {
                { "int", SciterValue.Create(1)},
                { "string", SciterValue.Create("string")},
                { "double", SciterValue.Create(3d)},
                { "color",SciterValue.Create(SciterColor.Khaki)}
            };
            
            var actual = SciterValue.Create(dictionary);

            Assert.IsTrue(actual.IsMap);
        }

        [Test]
        public void Value_from_JsonString()
        {
            var actual = SciterValue.FromJsonString("{ \"key\" : \"value\"}");
            Assert.IsTrue(actual.IsMap);
            Assert.AreEqual("value", actual["key"].AsString());
        }

        [TestCase("This is an error")]
        public void Value_from_MakeError(string input)
        {
            var actual = SciterValue.MakeError(input);
            Assert.IsTrue(actual.IsErrorString);
            Assert.AreEqual(input, actual.AsString());
        }

        [TestCase("")]
        [TestCase(null)]
        public void Null_or_empty_input_for_MakeError_returns_null(string input)
        {
            var actual = SciterValue.MakeError(input);
            Assert.IsNull(actual);
        }
        
        [TestCase("sym")]
        public void Value_from_MakeSymbol(string input)
        {
            var actual = SciterValue.MakeSymbol(input);
            Assert.IsTrue(actual.IsSymbol);
            Assert.AreEqual(input, actual.AsString());
        }
        
        [TestCase("")]
        [TestCase(null)]
        public void Null_or_empty_input_for_MakeSymbol_returns_null(string input)
        {
            var actual = SciterValue.MakeSymbol(input);
            Assert.IsNull(actual);
        }
        
        [Test]
        public void Value_get_Keys()
        {
            var dictionary = new Dictionary<string, IConvertible>
            {
                { "key1", null},
                { "key2", true},
                { "key3", (byte)1},
                { "key4", 1}
            };
            
            var actual = SciterValue.Create(dictionary);

            Assert.IsTrue(actual.IsMap);
            
            Assert.AreEqual(dictionary.Keys, actual.Keys.Select(s => s.AsString()));
        }
        
        private class TestClass
        {
            public object key1 { get; set; }
            public bool key2 { get; set; }
            public byte key3 { get; set; }
            public int key4 { get; set; }
            
            [SciterPropertyName("key5")]
            public double DoubleProperty { get; set; }
            
            [SciterPropertyName("key6")]
            public float SingleProperty { get; set; }
            
            [SciterPropertyName("key7")]
            public long LongProperty { get; set; }
        }
        
        [Test]
        public void Value_MapTo_v1()
        {
            var obj = new
            {
                key1 = (object)null,
                key2 = true,
                key3 = (byte)1,
                key4 = 1,
                key5 = 1d,
                key6 = 1f,
                key7 = 1L
            };
            
            var actual = SciterValue.Create(obj);

            Assert.IsTrue(actual.IsMap);

            var mapToClass = actual.MapTo<TestClass>();
            
            Assert.NotNull(mapToClass);
            Assert.AreEqual(mapToClass.key1, null);
            Assert.AreEqual(mapToClass.key2, true);
            Assert.AreEqual(mapToClass.key3, (byte)1);
            Assert.AreEqual(mapToClass.key4, 1);
            Assert.AreEqual(mapToClass.DoubleProperty, 1d);
            Assert.AreEqual(mapToClass.SingleProperty, 1f);
            Assert.AreEqual(mapToClass.LongProperty, 1L);
        }
        
        private class TestNestedClass : TestClass
        {
            public TestClass key8 { get; set; }
            public TestNestedClass key9 { get; set; }
        }
        
        [Test]
        public void Value_MapTo_v2()
        {
            var obj = new
            {
                key1 = (object)null,
                key2 = true,
                key3 = (byte)1,
                key4 = 1,
                key5 = 1d,
                key6 = 1f,
                key7 = 1L,
                key8 = new
                {
                    key1 = (object)null,
                    key2 = true,
                    key3 = (byte)2,
                    key4 = 2,
                    key5 = 2d,
                    SingleProperty = 2f,
                    LongProperty = 2L,
                },
                key9 = new 
                {
                    key1 = (object)null,
                    key2 = true,
                    key3 = (byte)3,
                    key4 = 3,
                    DoubleProperty = 3d,
                    key6 = 3f,
                    key7 = 3L,
                    key8 = new 
                    {
                        key1 = (object)null,
                        key2 = true,
                        key3 = (byte)4,
                        key4 = 4,
                        DoubleProperty = 4d,
                        SingleProperty = 4f,
                        LongProperty = 4L,
                    },
                    key9 = new
                    {
                        key1 = (object)null,
                        key2 = true,
                        key3 = (byte)5,
                        key4 = 5,
                        DoubleProperty = 5d,
                        SingleProperty = 5f,
                        LongProperty = 5L,
                        key8 = new 
                        {
                            key1 = (object)null,
                            key2 = true,
                            key3 = (byte)6,
                            key4 = 6,
                            key5 = 6d,
                            SingleProperty = 6f,
                            key7 = 6L,
                        },
                    }
                }
            };
            
            var actual = SciterValue.Create(obj);

            Assert.IsTrue(actual.IsMap);
            
            var mapToClass = actual.MapTo<TestNestedClass>();
            
            Assert.NotNull(mapToClass);
            Assert.AreEqual(mapToClass.key8.SingleProperty, 2L);
            Assert.AreEqual(mapToClass.key9.DoubleProperty, 3d);
            Assert.AreEqual(mapToClass.key9.key8.SingleProperty, 4d);
            Assert.AreEqual(mapToClass.key9.key9.DoubleProperty, 5d);
            Assert.AreEqual(mapToClass.key9.key9.key8.LongProperty, 6L);

        }
        
    }
}