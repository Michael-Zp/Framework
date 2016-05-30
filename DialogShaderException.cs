using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
	public class DialogShaderException
	{
		public static void Show(ShaderException e)
		{
			var form = new FormShaderError();
			form.textBox.Text = "Error in " + e.Type + Environment.NewLine + e.Message;
			form.textBox.Select(0, 0);
			form.ShowDialog();
		}
	}
}
