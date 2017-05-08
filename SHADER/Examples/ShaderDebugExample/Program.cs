using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Text;

namespace Example
{
	class MyWindow : IWindow
	{
		private Shader shader;
		private QueryObject glTimerRender = new QueryObject();

		public MyWindow()
		{
			var sVertex = Encoding.UTF8.GetString(Resourcen.vertex);
			var sFragment = Encoding.UTF8.GetString(Resourcen.fragment);
			try
			{
				shader = ShaderLoader.FromStrings(sVertex, sFragment);
			}
			catch(ShaderException e)
			{
				PrintShaderException(e);
			}
		}

		private static void PrintShaderException(ShaderException e)
		{
			Console.Write(e.Type);
			Console.Write(": ");
			Console.WriteLine(e.Message);
			Console.WriteLine(e.Log);
		}

		public void Update(float updatePeriod)
		{
		}

		public void Render()
		{
			glTimerRender.Activate(QueryTarget.TimeElapsed);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shader.Activate();
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shader.Deactivate();
			glTimerRender.Deactivate();
			Console.Write("Rendertime:");
			Console.Write(glTimerRender.ResultLong / 1e6);
			Console.WriteLine("msec");
		}

		[STAThread]
		private static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyWindow());
		}
	}
}