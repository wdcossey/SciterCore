using System.Windows.Forms;

namespace SciterCore.WinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
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
            this.sciterControl1 = new SciterControl();
            this.SuspendLayout();
            this.sciterControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sciterControl1.Location = new System.Drawing.Point(0, 0);
            this.sciterControl1.Name = "sciterControl1";
            this.sciterControl1.Size = new System.Drawing.Size(738, 415);
            this.sciterControl1.TabIndex = 0;
            this.sciterControl1.Text = "sciterControl1";
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sciterControl1);
            this.Text = "SciterCore::WinForms";
            this.ResumeLayout(false);
        }

        private SciterControl sciterControl1;

        #endregion
    }
}

