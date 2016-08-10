using System;
using System.Windows.Forms;

namespace ShaderForm
{
	partial class FormMain
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
			this.glControl = new OpenTK.GLControl();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuFps = new System.Windows.Forms.ToolStripMenuItem();
			this.menuScreenshot = new System.Windows.Forms.ToolStripMenuItem();
			this.menuSizeSetting = new System.Windows.Forms.ToolStripComboBox();
			this.menuFullscreen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuDemo = new System.Windows.Forms.ToolStripMenuItem();
			this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuLoad = new System.Windows.Forms.ToolStripMenuItem();
			this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.menuShaders = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuShaderAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.menuTextures = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTextureAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.menuUniforms = new System.Windows.Forms.ToolStripMenuItem();
			this.addCameraUniformsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TextUniformAdd = new System.Windows.Forms.ToolStripTextBox();
			this.menuSound = new System.Windows.Forms.ToolStripMenuItem();
			this.panelSequence = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.sequenceBar1 = new ControlClassLibrary.SequenceBar();
			this.soundPlayerBar1 = new ControlClassLibrary.SeekBar();
			this.menuStrip.SuspendLayout();
			this.panelSequence.SuspendLayout();
			this.SuspendLayout();
			// 
			// glControl
			// 
			this.glControl.AllowDrop = true;
			this.glControl.BackColor = System.Drawing.Color.Black;
			this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glControl.Location = new System.Drawing.Point(0, 32);
			this.glControl.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.glControl.Name = "glControl";
			this.glControl.Size = new System.Drawing.Size(719, 446);
			this.glControl.TabIndex = 0;
			this.glControl.VSync = false;
			this.glControl.Load += new System.EventHandler(this.GlControl_Load);
			this.glControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.GlControl_DragDrop);
			this.glControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.GlControl_DragEnter);
			this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.GlControl_Paint);
			this.glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GlControl_MouseDown);
			this.glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GlControl_MouseMove);
			this.glControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GlControl_MouseUp);
			// 
			// menuStrip
			// 
			this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelp,
            this.menuFps,
            this.menuScreenshot,
            this.menuSizeSetting,
            this.menuFullscreen,
            this.menuDemo,
            this.menuShaders,
            this.menuTextures,
            this.menuUniforms,
            this.menuSound});
			this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
			this.menuStrip.ShowItemToolTips = true;
			this.menuStrip.Size = new System.Drawing.Size(719, 32);
			this.menuStrip.TabIndex = 2;
			this.menuStrip.Tag = "1";
			this.menuStrip.Text = "menuStrip1";
			// 
			// menuHelp
			// 
			this.menuHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.menuHelp.Name = "menuHelp";
			this.menuHelp.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.menuHelp.Size = new System.Drawing.Size(28, 28);
			this.menuHelp.Text = "?";
			// 
			// menuFps
			// 
			this.menuFps.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.menuFps.Name = "menuFps";
			this.menuFps.Size = new System.Drawing.Size(44, 28);
			this.menuFps.Text = "FPS";
			// 
			// menuScreenshot
			// 
			this.menuScreenshot.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.menuScreenshot.Name = "menuScreenshot";
			this.menuScreenshot.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.menuScreenshot.Size = new System.Drawing.Size(52, 28);
			this.menuScreenshot.Text = "Save";
			// 
			// menuSizeSetting
			// 
			this.menuSizeSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.menuSizeSetting.Items.AddRange(new object[] {
            "x0.25",
            "x0.5",
            "x1",
            "x2",
            "x4",
            "f128",
            "f256",
            "f512",
            "f1024",
            "f2048",
            "f4096",
            "f8192"});
			this.menuSizeSetting.Name = "menuSizeSetting";
			this.menuSizeSetting.Size = new System.Drawing.Size(99, 28);
			// 
			// menuFullscreen
			// 
			this.menuFullscreen.AutoToolTip = true;
			this.menuFullscreen.CheckOnClick = true;
			this.menuFullscreen.Name = "menuFullscreen";
			this.menuFullscreen.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.menuFullscreen.Size = new System.Drawing.Size(86, 28);
			this.menuFullscreen.Text = "Fullscreen";
			this.menuFullscreen.ToolTipText = "F11";
			this.menuFullscreen.CheckedChanged += new System.EventHandler(this.MenuFullscreen_CheckedChanged);
			// 
			// menuDemo
			// 
			this.menuDemo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.menuLoad,
            this.menuSave});
			this.menuDemo.Name = "menuDemo";
			this.menuDemo.Size = new System.Drawing.Size(62, 28);
			this.menuDemo.Text = "Demo";
			// 
			// clearToolStripMenuItem
			// 
			this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
			this.clearToolStripMenuItem.Size = new System.Drawing.Size(183, 26);
			this.clearToolStripMenuItem.Text = "Clear";
			this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
			// 
			// menuLoad
			// 
			this.menuLoad.Name = "menuLoad";
			this.menuLoad.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.menuLoad.Size = new System.Drawing.Size(183, 26);
			this.menuLoad.Text = "Load...";
			// 
			// menuSave
			// 
			this.menuSave.Name = "menuSave";
			this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.menuSave.Size = new System.Drawing.Size(183, 26);
			this.menuSave.Text = "Save...";
			// 
			// menuShaders
			// 
			this.menuShaders.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuShaderAdd});
			this.menuShaders.Name = "menuShaders";
			this.menuShaders.Size = new System.Drawing.Size(73, 28);
			this.menuShaders.Text = "Shaders";
			// 
			// MenuShaderAdd
			// 
			this.MenuShaderAdd.Name = "MenuShaderAdd";
			this.MenuShaderAdd.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
			this.MenuShaderAdd.Size = new System.Drawing.Size(178, 26);
			this.MenuShaderAdd.Text = "Add...";
			// 
			// menuTextures
			// 
			this.menuTextures.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuTextureAdd});
			this.menuTextures.Name = "menuTextures";
			this.menuTextures.Size = new System.Drawing.Size(76, 28);
			this.menuTextures.Text = "Textures";
			// 
			// MenuTextureAdd
			// 
			this.MenuTextureAdd.Name = "MenuTextureAdd";
			this.MenuTextureAdd.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
			this.MenuTextureAdd.Size = new System.Drawing.Size(175, 26);
			this.MenuTextureAdd.Text = "Add...";
			// 
			// menuUniforms
			// 
			this.menuUniforms.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCameraUniformsToolStripMenuItem,
            this.TextUniformAdd});
			this.menuUniforms.Name = "menuUniforms";
			this.menuUniforms.Size = new System.Drawing.Size(81, 28);
			this.menuUniforms.Text = "Uniforms";
			// 
			// addCameraUniformsToolStripMenuItem
			// 
			this.addCameraUniformsToolStripMenuItem.Name = "addCameraUniformsToolStripMenuItem";
			this.addCameraUniformsToolStripMenuItem.Size = new System.Drawing.Size(231, 26);
			this.addCameraUniformsToolStripMenuItem.Text = "Add Camera Uniforms";
			this.addCameraUniformsToolStripMenuItem.ToolTipText = "C";
			this.addCameraUniformsToolStripMenuItem.Click += new System.EventHandler(this.addCameraUniformsToolStripMenuItem_Click);
			// 
			// TextUniformAdd
			// 
			this.TextUniformAdd.MaxLength = 40;
			this.TextUniformAdd.Name = "TextUniformAdd";
			this.TextUniformAdd.Size = new System.Drawing.Size(100, 27);
			this.TextUniformAdd.ToolTipText = "Enter uniform name";
			this.TextUniformAdd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextUniformAdd_KeyDown);
			this.TextUniformAdd.TextChanged += new System.EventHandler(this.TextUniformAdd_TextChanged);
			// 
			// menuSound
			// 
			this.menuSound.Name = "menuSound";
			this.menuSound.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.menuSound.Size = new System.Drawing.Size(63, 28);
			this.menuSound.Text = "Sound";
			this.menuSound.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MenuSound_MouseDown);
			// 
			// panelSequence
			// 
			this.panelSequence.Controls.Add(this.button1);
			this.panelSequence.Controls.Add(this.sequenceBar1);
			this.panelSequence.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelSequence.Location = new System.Drawing.Point(0, 478);
			this.panelSequence.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.panelSequence.Name = "panelSequence";
			this.panelSequence.Size = new System.Drawing.Size(719, 42);
			this.panelSequence.TabIndex = 5;
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Image = global::ShaderForm.Properties.Resources.Restart_6322;
			this.button1.Location = new System.Drawing.Point(0, 0);
			this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(39, 42);
			this.button1.TabIndex = 6;
			this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Reload_Click);
			// 
			// sequenceBar1
			// 
			this.sequenceBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.sequenceBar1.Location = new System.Drawing.Point(39, 1);
			this.sequenceBar1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.sequenceBar1.Name = "sequenceBar1";
			this.sequenceBar1.Size = new System.Drawing.Size(679, 39);
			this.sequenceBar1.TabIndex = 5;
			this.sequenceBar1.OnChanged += new System.EventHandler(this.sequenceBar1_OnChanged);
			// 
			// soundPlayerBar1
			// 
			this.soundPlayerBar1.AllowDrop = true;
			this.soundPlayerBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.soundPlayerBar1.Location = new System.Drawing.Point(0, 520);
			this.soundPlayerBar1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.soundPlayerBar1.Name = "soundPlayerBar1";
			this.soundPlayerBar1.Playing = false;
			this.soundPlayerBar1.Position = 0F;
			this.soundPlayerBar1.Size = new System.Drawing.Size(719, 36);
			this.soundPlayerBar1.TabIndex = 3;
			this.soundPlayerBar1.TabStop = false;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(719, 556);
			this.Controls.Add(this.glControl);
			this.Controls.Add(this.panelSequence);
			this.Controls.Add(this.soundPlayerBar1);
			this.Controls.Add(this.menuStrip);
			this.DoubleBuffered = true;
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "FormMain";
			this.Text = "ShaderForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.panelSequence.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        #endregion

        private OpenTK.GLControl glControl;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem menuFps;
		private System.Windows.Forms.ToolStripComboBox menuSizeSetting;
		private ControlClassLibrary.SeekBar soundPlayerBar1;
		private System.Windows.Forms.ToolStripMenuItem menuTextures;
		private System.Windows.Forms.ToolStripMenuItem menuFullscreen;
		private System.Windows.Forms.ToolStripMenuItem menuScreenshot;
		private System.Windows.Forms.ToolStripMenuItem menuSound;
		private System.Windows.Forms.ToolStripMenuItem MenuTextureAdd;
		private System.Windows.Forms.ToolStripMenuItem menuShaders;
		private System.Windows.Forms.ToolStripMenuItem MenuShaderAdd;
		private System.Windows.Forms.ToolStripMenuItem menuDemo;
		private System.Windows.Forms.ToolStripMenuItem menuLoad;
		private System.Windows.Forms.ToolStripMenuItem menuSave;
		private System.Windows.Forms.Panel panelSequence;
		private ControlClassLibrary.SequenceBar sequenceBar1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ToolStripTextBox TextUniformAdd;
		private System.Windows.Forms.ToolStripMenuItem menuUniforms;
		private System.Windows.Forms.ToolStripMenuItem menuHelp;
		private ToolStripMenuItem clearToolStripMenuItem;
		private ToolStripMenuItem addCameraUniformsToolStripMenuItem;
	}
}

