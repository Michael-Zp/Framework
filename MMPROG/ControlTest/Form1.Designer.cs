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
			this.draggableLayoutPanel1 = new ControlClassLibrary.MovableControlsLayoutPanel();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.draggableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// draggableLayoutPanel1
			// 
			this.draggableLayoutPanel1.AllowOverlap = false;
			this.draggableLayoutPanel1.Controls.Add(this.listBox1);
			this.draggableLayoutPanel1.Controls.Add(this.comboBox1);
			this.draggableLayoutPanel1.Controls.Add(this.checkBox1);
			this.draggableLayoutPanel1.Controls.Add(this.button1);
			this.draggableLayoutPanel1.Controls.Add(this.button4);
			this.draggableLayoutPanel1.Controls.Add(this.button2);
			this.draggableLayoutPanel1.Controls.Add(this.button3);
			this.draggableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.draggableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.draggableLayoutPanel1.Name = "draggableLayoutPanel1";
			this.draggableLayoutPanel1.RestrictToPanel = true;
			this.draggableLayoutPanel1.Size = new System.Drawing.Size(848, 617);
			this.draggableLayoutPanel1.TabIndex = 1;
			this.draggableLayoutPanel1.ControlBoundsChanging += new System.EventHandler<ControlClassLibrary.NewControlBoundsArgs>(this.draggableLayoutPanel1_ControlBoundsChanging);
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(652, 367);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(120, 95);
			this.listBox1.TabIndex = 6;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(436, 271);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 5;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(213, 345);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(80, 17);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(158, 172);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(167, 54);
			this.button1.TabIndex = 3;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(43, 27);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(133, 37);
			this.button4.TabIndex = 3;
			this.button4.Text = "button4";
			this.button4.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(43, 186);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(109, 61);
			this.button2.TabIndex = 2;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(34, 105);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(254, 22);
			this.button3.TabIndex = 2;
			this.button3.Text = "button3";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(848, 617);
			this.Controls.Add(this.draggableLayoutPanel1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.draggableLayoutPanel1.ResumeLayout(false);
			this.draggableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private ControlClassLibrary.MovableControlsLayoutPanel draggableLayoutPanel1;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.CheckBox checkBox1;
	}
}

