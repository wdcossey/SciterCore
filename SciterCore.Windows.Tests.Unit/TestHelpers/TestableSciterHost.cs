namespace SciterCore.Windows.Tests.Unit.TestHelpers
{
    public class TestableSciterHost : SciterHost
    {

        public TestableSciterHost()
        {
            
        }
        
        public TestableSciterHost(SciterWindow window) 
            : base(window)
        {
            
        }
    }
}