using DMS.Geometry;
using DMS.HLGL;
using DMS.OpenGL;
using OpenTK.Graphics.OpenGL4;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			//GL.Enable(EnableCap.DepthTest);
			//texDiffuse = TextureLoader.FromBitmap(Resourcen.capsule0);
		}

		public static readonly string ShaderName = nameof(shader);

		public void ShaderChanged(string name, IShader shader)
		{
			if (ShaderName != name) return;
			this.shader = shader;
			if (ReferenceEquals(shader, null)) return;
			//load geometry
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			geometry = VAOLoader.FromMesh(mesh, shader);
		}

		public void Render()
		{
			if (ReferenceEquals(shader, null)) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			//texDiffuse.Activate();
			geometry.Draw();
			//texDiffuse.Deactivate();
			shader.Deactivate();
		}

		private IShader shader;
		private VAO geometry;
		//private Texture texDiffuse;
	}
}
