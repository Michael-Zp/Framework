using System.Windows.Forms;

namespace Framework
{
	public partial class FormShaderError : Form
	{
		public FormShaderError()
		{
			InitializeComponent();
		}

		private void FormShaderError_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) Close(); 
		}
	}
}
