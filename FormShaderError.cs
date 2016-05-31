using System.Collections.Generic;
using System.Windows.Forms;

namespace Framework
{
	public partial class FormShaderError : Form
	{
		public FormShaderError()
		{
			InitializeComponent();
		}

		public void UpdateErrors(IEnumerable<ShaderLogLine> logLines)
		{
			errors.AddRange(logLines);
			listBox.DataSource = errors;
		}

		public List<ShaderLogLine> Errors { get { return errors; } }

		private List<ShaderLogLine> errors = new List<ShaderLogLine>();

		private void FormShaderError_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) Close(); 
		}

		private void listBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{

			//listBox.SelectedIndex;
		}
	}
}
