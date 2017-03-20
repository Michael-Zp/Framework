using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;
using System.Text;

namespace Example
{
	class MyWindow : IWindow
	{
		//private ShaderFileDebugger shaderWatcher;
		private Shader shader;
		private QueryObject glTimerRender = new QueryObject();

		public MyWindow()
		{
			//var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			//shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
			//	, Resourcen.vertex, Resourcen.fragment);

			var sVertex = Encoding.UTF8.GetString(Resourcen.vertex);
			var sFragment = Encoding.UTF8.GetString(Resourcen.fragment);
			try
			{
				shader = ShaderLoader.FromStrings(sVertex, sFragment);
			}
			catch(ShaderException e)
			{
				Console.Write(e.Type);
				Console.Write(": ");
				Console.WriteLine(e.Message);
				Console.WriteLine(e.Log);
			}
		}

		public void Update(float updatePeriod)
		{
		}

		public void Render()
		{
			//if (shaderWatcher.CheckForShaderChange())
			//{
			//	//update geometry when shader changes
			//}
			//var shader = shaderWatcher.Shader;
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
		public static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyWindow());
		}
	}
}