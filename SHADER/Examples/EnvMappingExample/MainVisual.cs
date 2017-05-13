using DMS.Application;
using DMS.Geometry;
using DMS.OpenGL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using DMS.ShaderDebugging;
using DMS.Base;
using System.IO;

namespace Example
{
	public class MainVisual
	{
		public CameraOrbit OrbitCamera { get { return camera; } }

		public MainVisual()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.vert", dir + "fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);

			envMap = TextureLoader.FromBitmap(Resourcen.beach);
			envMap.WrapMode(TextureWrapMode.MirroredRepeat);
			envMap.FilterLinear();

			camera.NearClip = 0.01f;
			camera.FarClip = 50;
			camera.Distance = 0;
			camera.FovY = 70;

			GL.ClearColor(Color4.White);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader is (re)loaded
				UpdateGeometry(shaderWatcher.Shader);
			}
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			var shader = shaderWatcher.Shader;
			shader.Activate();
			envMap.Activate();
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), camera.CalcPosition().ToOpenTK());
			geometry.Draw();
			envMap.Deactivate();
			shader.Deactivate();
		}

		public void Update(float updatePeriod)
		{
			camera.FovY = MathHelper.Clamp(camera.FovY, 0.1f, 175f);
		}

		private CameraOrbit camera = new CameraOrbit();
		private ShaderFileDebugger shaderWatcher;
		private Texture envMap;
		private VAO geometry;

		private void UpdateGeometry(Shader shader)
		{
			var sphere = Meshes.CreateSphere(1, 4);
			geometry = VAOLoader.FromMesh(sphere.SwitchTriangleMeshWinding(), shader);
		}
	}
}
