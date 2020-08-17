namespace SciterTest.WinForms
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.sciterControl1 = new SciterCore.WinForms.SciterControl();
			this.sciterHost = new SciterCore.WinForms.SciterHostComponent();
			this.sciterArchive = new SciterCore.WinForms.SciterArchiveComponent();
			this.SuspendLayout();
			// 
			// sciterControl1
			// 
			this.sciterControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sciterControl1.Location = new System.Drawing.Point(0, 0);
			this.sciterControl1.Name = "sciterControl1";
			this.sciterControl1.Size = new System.Drawing.Size(647, 422);
			this.sciterControl1.TabIndex = 0;
			this.sciterControl1.LoadHtml += new System.EventHandler<SciterCore.WinForms.LoadHtmlEventArgs>(this.sciterControl1_LoadHtml);
			// 
			// sciterHost
			// 
			this.sciterHost.RootPage = "index.html";
			this.sciterHost.Archive = this.sciterArchive;
			this.sciterHost.Control = this.sciterControl1;
			// 
			// sciterArchive
			// 
			this.sciterArchive.BaseAddress = new System.Uri("archive://app/", System.UriKind.Absolute);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(647, 422);
			this.Controls.Add(this.sciterControl1);
			this.Name = "Form1";
			this.Text = "SciterCore::Framework::Forms";
			this.ResumeLayout(false);
		}

		private SciterCore.WinForms.SciterArchiveComponent sciterArchive;
		private SciterCore.WinForms.SciterControl sciterControl1;
		private SciterCore.WinForms.SciterHostComponent sciterHost;

		#endregion
	}
}

