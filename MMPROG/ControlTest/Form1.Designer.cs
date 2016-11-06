namespace ControlTest
{
	partial class Form1
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.trackView1 = new ControlClassLibrary.TrackView();
			this.SuspendLayout();
			// 
			// trackView1
			// 
			this.trackView1.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.trackView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trackView1.Location = new System.Drawing.Point(0, 0);
			this.trackView1.Name = "trackView1";
			this.trackView1.Size = new System.Drawing.Size(284, 261);
			this.trackView1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.trackView1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private ControlClassLibrary.TrackView trackView1;
	}
}

