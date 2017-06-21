using DMS.Geometry;
using DMS.HLGL;
using DMS.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			drawParametersBase.Textures.Add(new NamedTexture("diffuse", TextureLoader.FromBitmap(Resourcen.capsule0)));
			drawParametersBase.BackfaceCulling = true;
			drawParametersBase.ZBufferTest = true;
			drawParametersFB.Textures.Add(new NamedTexture("tex", imageBase.Texture));
			drawParametersFB.BackfaceCulling = false;
			drawParametersFB.ZBufferTest = false;
			drawParametersFB.Shader = ShaderLoader.FromStrings(TextureToFrameBuffer.VertexShaderScreenQuad, TextureToFrameBuffer.FragmentShaderCopy);
		}

		public static readonly string ShaderName = "shader";

		public void ShaderChanged(string name, Shader shader)
		{
			if (ShaderName != name) return;
			drawParametersBase.Shader = shader;
			if (ReferenceEquals(shader, null)) return;
			//load geometry
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			var geometry = VAOLoader.FromMesh(mesh, shader);
			drawParametersBase.Vao = geometry;
		}

		public void Render()
		{
			imageBase.Clear();
			imageBase.Draw(drawParametersBase);
			frameBuffer.Draw(drawParametersFB);
		}

		private Image frameBuffer = new Image();
		private Image imageBase = new Image(512, 512, true);
		private DrawParameters drawParametersBase = new DrawParameters();
		private DrawParameters drawParametersFB = new DrawParameters();
	}
}
