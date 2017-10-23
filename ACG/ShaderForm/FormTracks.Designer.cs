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
			this.soundPlayerBar = new ControlClassLibrary.SeekBar();
			this.SuspendLayout();
			// 
			// soundPlayerBar
			// 
			this.soundPlayerBar.AllowDrop = true;
			this.soundPlayerBar.AutoSize = true;
			this.soundPlayerBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.soundPlayerBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.soundPlayerBar.Location = new System.Drawing.Point(0, 431);
			this.soundPlayerBar.Margin = new System.Windows.Forms.Padding(7);
			this.soundPlayerBar.Name = "soundPlayerBar";
			this.soundPlayerBar.Playing = false;
			this.soundPlayerBar.Position = 0.867F;
			this.soundPlayerBar.Size = new System.Drawing.Size(521, 51);
			this.soundPlayerBar.TabIndex = 4;
			this.soundPlayerBar.TabStop = false;
			// 
			// FormTracks
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(521, 482);
			this.Controls.Add(this.soundPlayerBar);
			this.Margin = new System.Windows.Forms.Padding(6);
			this.Name = "FormTracks";
			this.Text = "FormTracks";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public ControlClassLibrary.SeekBar soundPlayerBar;
	}
}