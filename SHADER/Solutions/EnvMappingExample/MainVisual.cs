using DMS.Application;
using DMS.Geometry;
using DMS.HLGL;
using DMS.OpenGL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Example
{
	public class MainVisual
	{
		public MainVisual(IShaderProvider shaderProvider)
		{
			this.shaderProvider = shaderProvider;
			envMap = TextureLoader.FromBitmap(Resourcen.beach);
			envMap.WrapFunction = TextureWrapFunction.MirroredRepeat;
			envMap.Filter = TextureFilterMode.Linear;
			background = new VisualBackground(envMap);

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
			if (ReferenceEquals(shader, null)) return;
			var sphere = Meshes.CreateSphere(1, 4);
			var envSphere = sphere.SwitchTriangleMeshWinding();
			envSphere.Add(sphere);
			geometry = VAOLoader.FromMesh(envSphere, shader);
		}

		public void Render()
		{
			var shader = shaderProvider.GetShader(ShaderName);
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

		public const string ShaderName = "shader";
		private CameraOrbit camera = new CameraOrbit();

		private IShaderProvider shaderProvider;
		private ITexture envMap;
		private VAO geometry;
		private VisualBackground background;
	}
}
