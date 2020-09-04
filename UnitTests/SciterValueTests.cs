using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SciterCore.UnitTests
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
            var actual = SciterValue.Create("some value to discard");
            actual.Clear();
            
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
        
        [TestCase("this is a string")]
        [TestCase("")]
        [TestCase(null)]
        public void Value_from_String(string input)
        {
            var actual = SciterValue.Create(input);
            
            Assert.IsTrue(input == null ? actual.IsUndefined : actual.IsString);
            Assert.AreEqual(input, actual.AsString(default(string)));
        }

        [TestCase(new byte[5] { 1, 2, 3, 4, 5 })]
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
            
            var actual = SciterValue.Create(input);
            
            Assert.IsTrue(actual.IsDate);
            Assert.AreEqual(input, actual.AsDateTime(kind == DateTimeKind.Utc));
        }
        
        [TestCase(255, 0, 255, 0)]
        [TestCase(0, 0, 0, 255)]
        [TestCase(255, 255, 255, 255)]
        public void Value_from_SciterColor(byte r, byte g, byte b, byte a)
        {
            var color = SciterColor.Create(255, 0, 255);
            
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
                SciterValue.Create(System.DateTime.Now),
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
            Assert.AreEqual(input[5].AsBoolean(false), actual.GetItem(5).AsBoolean(true));
        }
        
        [Test]
        public void Value_from_Object()
        {
            var list = new List<int> { 123 };
            var obj = new { list = list };
            
            var actual = SciterValue.Create(obj);

            Assert.IsTrue(actual.IsMap);
            Assert.IsTrue(actual["list"].IsArray);
            Assert.AreEqual(list[0], actual["list"][0].AsInt32());
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
                { "int", 1},
                { "string", "string"},
                { "double", 3d},
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
    }
}