using Zenseless.OpenGL;
using Zenseless.Geometry;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Zenseless.HLGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			camera.FarClip = 50;
			camera.Distance = 5;
			camera.FovY = 30;

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
			UpdateGeometry(shader);
		}

		public void Render()
		{
			if (ReferenceEquals(shader, null)) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.Uniform3(shader.GetResourceLocation(ShaderResourceType.Uniform, "light1Direction"), new Vector3(-1, -1, -1).Normalized());
			GL.Uniform4(shader.GetResourceLocation(ShaderResourceType.Uniform, "light1Color"), new Color4(1f, 1f, 1f, 1f));
			GL.Uniform3(shader.GetResourceLocation(ShaderResourceType.Uniform, "light2Position"), new Vector3(-1, -1, 1));
			GL.Uniform4(shader.GetResourceLocation(ShaderResourceType.Uniform, "light2Color"), new Color4(1f, .1f, .1f, 1f));
			GL.Uniform3(shader.GetResourceLocation(ShaderResourceType.Uniform, "light3Position"), new Vector3(-2, 2, 2));
			GL.Uniform3(shader.GetResourceLocation(ShaderResourceType.Uniform, "light3Direction"), new Vector3(1, -1, -1).Normalized());
			GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "light3Angle"), Zenseless.Geometry.MathHelper.DegreesToRadians(10f));
			GL.Uniform4(shader.GetResourceLocation(ShaderResourceType.Uniform, "light3Color"), new Color4(0, 0, 1f, 1f));
			GL.Uniform4(shader.GetResourceLocation(ShaderResourceType.Uniform, "ambientLightColor"), new Color4(.3f, .3f, .1f, 1f));
			GL.Uniform4(shader.GetResourceLocation(ShaderResourceType.Uniform, "materialColor"), new Color4(.7f, .7f, .7f, 1f));
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			GL.Uniform3(shader.GetResourceLocation(ShaderResourceType.Uniform, "cameraPosition"), camera.CalcPosition().ToOpenTK());
			geometry.Draw();
			shader.Deactivate();
		}

		public static readonly string ShaderName = nameof(shader);
		private CameraOrbit camera = new CameraOrbit();
		private IShader shader;

		private VAO geometry;

		private void UpdateGeometry(IShader shader)
		{
			var mesh = new DefaultMesh();
			var sphere = Meshes.CreateSphere(1, 4);
			sphere.SetConstantUV(new System.Numerics.Vector2(0, 0));
			mesh.Add(sphere);
			var suzanne = Obj2Mesh.FromObj(Resourcen.suzanne);
			mesh.Add(suzanne.Transform(System.Numerics.Matrix4x4.CreateTranslation(2, 2, -2)));
			geometry = VAOLoader.FromMesh(mesh, shader);
		}
	}
}
