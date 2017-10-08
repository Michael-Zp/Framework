namespace ShaderForm
{
	partial class FormTracks
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
			this.soundPlayerBar1 = new ControlClassLibrary.SeekBar();
			this.SuspendLayout();
			// 
			// soundPlayerBar1
			// 
			this.soundPlayerBar1.AllowDrop = true;
			this.soundPlayerBar1.AutoSize = true;
			this.soundPlayerBar1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.soundPlayerBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.soundPlayerBar1.Location = new System.Drawing.Point(0, 233);
			this.soundPlayerBar1.Margin = new System.Windows.Forms.Padding(4);
			this.soundPlayerBar1.Name = "soundPlayerBar1";
			this.soundPlayerBar1.Playing = false;
			this.soundPlayerBar1.Position = 0.867F;
			this.soundPlayerBar1.Size = new System.Drawing.Size(284, 28);
			this.soundPlayerBar1.TabIndex = 4;
			this.soundPlayerBar1.TabStop = false;
			// 
			// FormTracks
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.soundPlayerBar1);
			this.Name = "FormTracks";
			this.Text = "FormTracks";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ControlClassLibrary.SeekBar soundPlayerBar1;
	}
}