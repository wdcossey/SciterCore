using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SciterCore;
using SciterCore.Interop;
using SciterCore.WinForms;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.WinForms
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			sciterControl1.HandleCreated += SciterControl1_HandleCreated;
			
			sciterHost.GetArchiveItem += SciterHostOnGetArchiveItem;
			sciterHost.OnScriptCall += SciterHostOnOnScriptCall;
		}

		private void SciterHostOnOnScriptCall(object sender, ScriptCallEventArgs e)
		{
			e.Owner = this;
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

		private void SciterHostOnGetArchiveItem(object sender, GetArchiveItemEventArgs e)
		{
			switch (e.Path.OriginalString)
			{
				case "this://app/icons8-visual-studio-code-2019.svg" :
					e.Path = new Uri($"{e.BaseAddress}icons8-visual-studio-2019.svg");
					break;
			}
		}
		
		private void SciterControl1_HandleCreated(object sender, EventArgs e)
		{
			
		}

		private void sciterControl1_LoadHtml(object sender, SciterCore.WinForms.LoadHtmlEventArgs e)
        {
            e.Html =
                "<body>" +
                "<code>Add an event handler to the <b>HandleCreated</b> event for any needed initialization (e.g.: load the HTML)</code><br /><br />" +
                "<code>In the handler, use the <b>SciterWnd</b> property of this control to access the SciterWindow instance.</code>" +
                "</body>";
        }
	}
}