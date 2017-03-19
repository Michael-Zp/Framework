using DMS.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Text;

namespace Example
{
	class MyWindow : IWindow
	{
		private const int particelCount = 100000;
		private Shader shader;
		private Stopwatch timeSource = new Stopwatch();
		private VAO vao;

		public MyWindow()
		{
			var sVertex = Encoding.UTF8.GetString(Resourcen.vertex);
			var sFragment = Encoding.UTF8.GetString(Resourcen.fragment);
			shader = ShaderLoader.FromStrings(sVertex, sFragment);

			//VAO = CreateParticles();
			vao = CreateParticles(shader);
			timeSource.Start();
		}

		private static VAO CreateParticles(Shader shader)
		{
			var vao = new VAO();
			//generate position arrray on CPU
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			var positions = new Vector2[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				positions[i] = new Vector2(RndCoord(), RndCoord());
			}
			//copy positions to GPU
			vao.SetAttribute(shader.GetAttributeLocation("in_position"), positions, VertexAttribPointerType.Float, 2);
			//generate velocity arrray on CPU
			Func<float> RndSpeed = () => (Rnd01() - 0.5f) * 0.1f;
			var velocities = new Vector2[particelCount];
			for (int i = 0; i < particelCount; ++i)
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
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.PointSize(1.0f);
			shader.Activate();
			//ATTENTION: always give the time as a float if the uniform in the shader is a float
			GL.Uniform1(shader.GetUniformLocation("time"), (float)timeSource.Elapsed.TotalSeconds);
			vao.Activate();
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);
			vao.Deactive();
			shader.Deactivate();
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