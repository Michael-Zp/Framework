using DMS.OpenGL;
using DMS.Geometry;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using DMS.ShaderDebugging;
using DMS.Base;
using System.IO;
using System;

namespace Example
{
	public class MainVisual : IWindow
	{
		public CameraOrbit OrbitCamera { get { return camera; } }

		public MainVisual()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.vert", dir + "fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);
			geometry = CreateMesh(shaderWatcher.Shader);
			Vector4[] materials = new Vector4[] { new Vector4(1, 1, 1, 0), Vector4.UnitX, Vector4.UnitY, Vector4.One };
			//texture.LoadPixels((IntPtr)materials, 4, 1, PixelInternalFormat.Rgba8, PixelFormat.Rgba, PixelType.Float);

			camera.FarClip = 50;
			camera.Distance = 1.8f;
			camera.TargetY = -0.3f;
			camera.FovY = 70;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				geometry = CreateMesh(shaderWatcher.Shader);
			}
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			var shader = shaderWatcher.Shader;
			shader.Activate();
			GL.Uniform3(shader.GetUniformLocation("ambient"), new Vector3(0.1f));
			GL.Uniform3(shader.GetUniformLocation("lightPosition"), new Vector3(0, 0.9f, -0.5f));
			GL.Uniform3(shader.GetUniformLocation("lightColor"), new Vector3(1, 1, 1));
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), camera.CalcPosition().ToOpenTK());
			geometry.Draw();
			shader.Deactivate();
		}

		public void Update(float updatePeriod)
		{
		}

		private CameraOrbit camera = new CameraOrbit();
		private ShaderFileDebugger shaderWatcher;
		private Texture texture = new Texture();
		private VAO geometry;

		private static VAO CreateMesh(Shader shader)
		{
			Mesh mesh = Meshes.CreateCornellBox();
			return VAOLoader.FromMesh(mesh, shader);
		}
	}
}
