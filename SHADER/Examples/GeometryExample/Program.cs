using DMS.OpenGL;
using DMS.ShaderDebugging;
using DMS.System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Example
{
	class MyWindow : IWindow
	{
		private const int pointCount = 10000;
		private ShaderFileDebugger shaderWatcher;
		private QueryObject glTimerRender = new QueryObject();
		private Stopwatch timeSource = new Stopwatch();
		private VAO geometry;

		public MyWindow()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);

			var sVertex = Encoding.UTF8.GetString(Resourcen.vertex);
			var sFragment = Encoding.UTF8.GetString(Resourcen.fragment);
			//try
			//{
			//	shader = ShaderLoader.FromStrings(sVertex, sFragment);
			//}
			//catch(ShaderException e)
			//{
			//	Console.Write(e.Type);
			//	Console.Write(": ");
			//	Console.WriteLine(e.Message);
			//	Console.WriteLine(e.Log);
			//}
			geometry = CreateParticles(shaderWatcher.Shader);
			GL.Enable(EnableCap.ProgramPointSize);
			GL.Enable(EnableCap.PointSprite);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
			timeSource.Start();
		}

		private static VAO CreateParticles(Shader shader)
		{
			var vao = new VAO();
			//generate position arrray on CPU
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			var positions = new Vector2[pointCount];
			for (int i = 0; i < pointCount; ++i)
			{
				positions[i] = new Vector2(RndCoord(), RndCoord());
			}
			//copy positions to GPU
			vao.SetAttribute(shader.GetAttributeLocation("in_position"), positions, VertexAttribPointerType.Float, 2);
			//generate velocity arrray on CPU
			Func<float> RndSpeed = () => (Rnd01() - 0.5f) * 0.1f;
			var velocities = new Vector2[pointCount];
			for (int i = 0; i < pointCount; ++i)
			{
				velocities[i] = new Vector2(RndSpeed(), RndSpeed());
			}
			//copy velocities to GPU
			vao.SetAttribute(shader.GetAttributeLocation("in_velocity"), velocities, VertexAttribPointerType.Float, 2);
			return vao;
		}

		public void Update(float updatePeriod)
		{
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				geometry = CreateParticles(shaderWatcher.Shader);
			}
			var shader = shaderWatcher.Shader;
			glTimerRender.Activate(QueryTarget.TimeElapsed);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			shader.Activate();
			////ATTENTION: always give the time as a float if the uniform in the shader is a float
			GL.Uniform1(shader.GetUniformLocation("time"), (float)timeSource.Elapsed.TotalSeconds);
			geometry.Activate();
			GL.DrawArrays(PrimitiveType.Points, 0, pointCount);
			geometry.Deactive();
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
			app.GameWindow.WindowState = WindowState.Fullscreen;
			app.Run(new MyWindow());
		}
	}
}