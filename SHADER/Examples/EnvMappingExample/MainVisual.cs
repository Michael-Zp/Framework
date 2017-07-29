using DMS.Geometry;
using DMS.HLGL;
using DMS.OpenGL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			envMap = TextureLoader.FromBitmap(Resourcen.beach);
			envMap.WrapFunction = TextureWrapFunction.MirroredRepeat;
			envMap.Filter = TextureFilterMode.Linear;

			camera.NearClip = 0.01f;
			camera.FarClip = 50;
			camera.Distance = 0;
			camera.FovY = 70;

			GL.ClearColor(Color4.White);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void ShaderChanged(string name, IShader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
			if (ReferenceEquals(shader, null)) return;
			var sphere = Meshes.CreateSphere(1, 4);
			var envSphere = sphere.SwitchTriangleMeshWinding();
			//var refSphere = sphere.
			geometry = VAOLoader.FromMesh(envSphere, shader);
		}

		public void Render()
		{
			if (ReferenceEquals(shader, null)) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			envMap.Activate();
			camera.FovY = MathHelper.Clamp(camera.FovY, 0.1f, 175f);
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), camera.CalcPosition().ToOpenTK());
			geometry.Draw();
			envMap.Deactivate();
			shader.Deactivate();
		}

		public static readonly string ShaderName = nameof(shader);
		private CameraOrbit camera = new CameraOrbit();

		private IShader shader;
		private ITexture envMap;
		private VAO geometry;
	}
}
