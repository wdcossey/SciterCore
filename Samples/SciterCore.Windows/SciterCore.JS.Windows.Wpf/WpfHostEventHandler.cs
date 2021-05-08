using System;
using System.Threading.Tasks;

namespace SciterCore.JS.Windows.Wpf
{
    public class WpfHostEventHandler: SciterEventHandler
    {
        public void GetDotNetVersion(SciterElement element, SciterValue onCompleted)
        {
            var value = SciterValue.Create(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
            onCompleted.Invoke(value);
        }
        
        public Task GetRuntimeInfo(SciterElement element, SciterValue onCompleted, SciterValue onError)
        {
            try
            {
                var value = SciterValue.Create(
                    new {
                        FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                        ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
                        OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
                        OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                        SystemVersion = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion()
                    });
			
                onCompleted.Invoke(value);
            }
            catch (Exception e)
            {
                onError.Invoke(SciterValue.MakeError(e.Message));
            }
		
            return Task.CompletedTask;
        }
    }
}