using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DMS.ShaderDebugging
{
	public partial class FormShaderException : Form
	{
		public FormShaderException()
		{
			InitializeComponent();
			listBox.DataSource = errors;
			listBox.MouseWheel += OnMouseWheel;
			richTextBox.MouseWheel += OnMouseWheel;
		}

		public BindingList<ShaderLogLine> Errors { get { return errors; } }

		public string SourceText { get { return richTextBox.Text; } set { richTextBox.Text = value; } }

		public float FontSize
		{
			get
			{
				return listBox.Font.Size;
			}
			set
			{
				var size = Math.Max(6, value);
				var font = new Font(listBox.Font.FontFamily, size);
				listBox.Font = font;
				richTextBox.Font = font;
			}
		}

		private BindingList<ShaderLogLine> errors = new BindingList<ShaderLogLine>();

		private void OnMouseWheel(object sender, MouseEventArgs e)
		{
			if (Keys.Control == ModifierKeys)
			{
				FontSize += Math.Sign(e.Delta) * 2;
			}
		}

		private void FormShaderError_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape: Close(); break;
			}
		}

		private void listBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			richTextBox.Select(0, richTextBox.Text.Length);
			richTextBox.SelectionBackColor = Color.White;
			richTextBox.SelectionColor = Color.Black;
			try
			{
				var logLine = errors[listBox.SelectedIndex];
				var nr = logLine.LineNumber - 1;
				int start = richTextBox.GetFirstCharIndexFromLine(nr);
				int length = richTextBox.Lines[nr].Length;
				richTextBox.Select(start, length);
				richTextBox.SelectionBackColor = Color.DarkRed;
				richTextBox.SelectionColor = Color.White;
				richTextBox.ScrollToCaret();
			}
			catch { }
		}

		private void FormShaderException_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Visible = false;
			this.SaveLayout();
			RegistryLoader.SaveValue(Name, "fontSize", FontSize);
		}

		private void FormShaderException_Load(object sender, EventArgs e)
		{
			this.LoadLayout();
			FontSize = (float)Convert.ToDouble(RegistryLoader.LoadValue(Name, "fontSize", 12.0f));
		}
	}
}
