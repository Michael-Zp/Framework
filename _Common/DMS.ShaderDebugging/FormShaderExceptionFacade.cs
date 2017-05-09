using DMS.OpenGL;
using System.Diagnostics;
using System.Windows.Forms;

namespace DMS.ShaderDebugging
{
	public class FormShaderExceptionFacade
	{
		public string ShowModal(ShaderException e)
		{
			form = new FormShaderException();
			form.Text = e.Message;
			var compileException = e as ShaderCompileException;
			if (ReferenceEquals(null, compileException))
			{
				form.SourceText = string.Empty;
			}
			else
			{
				form.SourceText = compileException.ShaderCode;
			}
			//load error list after source code is loaded for highlighting of error to work
			form.Errors.Clear();
			var log = new ShaderLog(e.ShaderLog);
			foreach (var logLine in log.Lines)
			{
				form.Errors.Add(logLine);
				Debug.Print(@"D:\Daten\FH Ravensburg\Framework\_Common\DMS.ShaderDebugging\FormShaderException.cs(71,41,71,41): error CS1002: ; erwartet.");

			}
			form.Select(0);
			form.TopMost = true;
			closeOnFileChange = true;
			form.ShowDialog();
			closeOnFileChange = false;
			var sourceText = form.SourceText;
			form = null;
			return sourceText;
		}

		public void Close()
		{
			if (ReferenceEquals(null, form)) return;
			if (!closeOnFileChange) return;
			form.Invoke((MethodInvoker)delegate
			{
				form.Close();
			});
		}

		private FormShaderException form = null;
		private bool closeOnFileChange = false;
	}
}
