using DMS.Application;
using DMS.Base;
using DMS.OpenGL;
using DMS.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(ResourceManager resourceManager)
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			resourceManager.AddShader(dir + "vertex.vert", dir + "fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);

			resourceManager.ShaderChanged += ResourceManager_ShaderChanged;
			camera.FarClip = 50;
			camera.Distance = 1.8f;
			camera.TargetY = -0.3f;
			camera.FovY = 70;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		public CameraOrbit OrbitCamera { get { return camera; } }

		private void ResourceManager_ShaderChanged(Shader shader)
		{
			this.shader = shader;
			if (ReferenceEquals(shader, null)) return;
			Mesh mesh = Meshes.CreateCornellBox();
			geometry = VAOLoader.FromMesh(mesh, shader);

			Vector4[] materials = new Vector4[] { new Vector4(1, 1, 1, 0), Vector4.UnitX, Vector4.UnitY, Vector4.One };
			//texture.LoadPixels((IntPtr)materials, 4, 1, PixelInternalFormat.Rgba8, PixelFormat.Rgba, PixelType.Float);
		}

		public void Render()
		{
			if (ReferenceEquals(shader, null)) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
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

		private CameraOrbit camera = new CameraOrbit();
		private Shader shader;
		//private Texture texture = new Texture();
		private VAO geometry;
	}
}
