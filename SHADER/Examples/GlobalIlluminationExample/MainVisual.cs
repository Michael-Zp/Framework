using Zenseless.OpenGL;
using Zenseless.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Zenseless.HLGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			camera.FarClip = 50;
			camera.Distance = 1.8f;
			camera.TargetY = -0.3f;
			camera.FovY = 70;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		public static readonly string ShaderName = nameof(shader);
		public CameraOrbit OrbitCamera { get { return camera; } }

		public void ShaderChanged(string name, IShader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
			if (ReferenceEquals(shader, null)) return;
			Mesh mesh = Meshes.CreateCornellBox();
			geometry = VAOLoader.FromMesh(mesh, shader);
			bufferMaterials.Set(Meshes.CreateCornellBoxMaterial(), BufferUsageHint.StaticDraw);
		}

		public void Render()
		{
			if (ReferenceEquals(shader, null)) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			GL.Uniform3(shader.GetResourceLocation(ShaderResourceType.Uniform, "ambient"), new Vector3(0.1f));
			GL.Uniform3(shader.GetResourceLocation(ShaderResourceType.Uniform, "lightPosition"), new Vector3(0, 0.9f, -0.5f));
			GL.Uniform3(shader.GetResourceLocation(ShaderResourceType.Uniform, "lightColor"), new Vector3(0.8f));
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			GL.Uniform3(shader.GetResourceLocation(ShaderResourceType.Uniform, "cameraPosition"), camera.CalcPosition().ToOpenTK());
			var bindingIndex = shader.GetResourceLocation(ShaderResourceType.UniformBuffer, "bufferMaterials");
			bufferMaterials.ActivateBind(bindingIndex);
			geometry.Draw();
			bufferMaterials.Deactivate();
			shader.Deactivate();
		}

		private CameraOrbit camera = new CameraOrbit();
		private BufferObject bufferMaterials = new BufferObject(BufferTarget.UniformBuffer);
		private IShader shader;
		private VAO geometry;
	}
}
