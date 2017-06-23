using DMS.Geometry;
using DMS.HLGL;
using DMS.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			drawParametersBase.BackfaceCulling = true;
			drawParametersBase.Textures.Add(new NamedTexture("diffuse", TextureLoader.FromBitmap(Resourcen.capsule0)));
			drawParametersBase.UpdateMesh(Obj2Mesh.FromObj(Resourcen.suzanne));
			drawParametersBase.UpdateShader(Resources.Shader);
			drawParametersBase.ZBufferTest = true;

			drawParametersFB.BackfaceCulling = false;
			drawParametersFB.Textures.Add(new NamedTexture("tex", imageBase.Texture));
			drawParametersFB.UpdateShader(Resources.ShaderCopy);
			drawParametersFB.ZBufferTest = false;
		}

		public void Render()
		{
			imageBase.Clear();
			imageBase.Draw(drawParametersBase);
			frameBuffer.Draw(drawParametersFB);
		}

		private Image frameBuffer = new Image();
		private Image imageBase = new Image(64, 64, true);
		private DrawParameters drawParametersBase = new DrawParameters();
		private DrawParameters drawParametersFB = new DrawParameters();
	}
}
