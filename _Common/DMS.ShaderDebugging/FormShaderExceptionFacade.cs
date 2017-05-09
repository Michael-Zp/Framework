using DMS.OpenGL;
using System.Windows.Forms;

namespace DMS.ShaderDebugging
{
	public class FormShaderExceptionFacade
	{
		public string ShowModal(ShaderException e)
		{
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
			}
			form.TopMost = true;
			closeOnFileChange = true;
			form.ShowDialog();
			closeOnFileChange = false;
			return form.SourceText;
		}

		public void Close()
		{
			if (!closeOnFileChange) return;
			form.Invoke((MethodInvoker)delegate
			{
				form.Close();
			});
		}

		private readonly FormShaderException form = new FormShaderException();
		private bool closeOnFileChange;
	}
}
