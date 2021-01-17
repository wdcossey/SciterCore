using System;
using NUnit.Framework;
using SciterCore.Windows.Tests.Unit.TestHelpers;

namespace SciterCore.Windows.Tests.Unit
{
    [TestFixture]
    public class SciterHostTests
    {
        private IntPtr _fakePointer;

        [SetUp]
        public void SetUp()
        {
            _fakePointer = new IntPtr(new Random().Next(100000, 999999));
        }
        
        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void SetupWindow_With_An_Null_SciterWindow_Throws_ArgumentNullException()
        {
            using (var sciterHost = new TestableSciterHost())
            {
                Assert.Throws<ArgumentNullException>(() => sciterHost.SetupWindow((SciterWindow)null));
            }
        }

        [Test]
        public void SetupWindow_More_Than_Once_Throws_InvalidOperationException()
        {
            using (var sciterHost = new TestableSciterHost())
            using (var window = new SciterWindow(_fakePointer))
            {
                sciterHost.SetupWindow(window);
                Assert.Throws<InvalidOperationException>(() => sciterHost.SetupWindow(window));
            }
        }

        [Test]
        public void SetupWindow_With_IntPtr_And_An_Invalid_Handle_Throws_ArgumentOutOfRangeException()
        {
            using (var sciterHost = new TestableSciterHost())
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => sciterHost.SetupWindow(IntPtr.Zero));
            }
        }

        [Test]
        public void SetupWindow_With_IntPtr_More_Than_Once_Throws_InvalidOperationException()
        {
            using (var sciterHost = new TestableSciterHost())
            {
                sciterHost.SetupWindow(_fakePointer);
                Assert.Throws<InvalidOperationException>(() => sciterHost.SetupWindow(_fakePointer));
            }
        }
    }
}