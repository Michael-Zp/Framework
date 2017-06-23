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
			drawParametersBase.AddTexture(new NamedTexture("diffuse", TextureLoader.FromBitmap(Resourcen.capsule0)));
			drawParametersBase.UpdateMesh(Obj2Mesh.FromObj(Resourcen.suzanne));
			drawParametersBase.UpdateShader(Resources.Shader);
			drawParametersBase.ZBufferTest = true;

			drawParametersFB.BackfaceCulling = false;
			drawParametersFB.AddTexture(new NamedTexture("tex", imageBase.Texture));
			drawParametersFB.UpdateShader(Resources.ShaderCopy);
			drawParametersFB.ZBufferTest = false;
		}

		public void Render()
		{
			imageBase.Clear();
			//todo: draw object multiple times
			imageBase.Draw(drawParametersBase);
			frameBuffer.Draw(drawParametersFB);
		}

		private Image frameBuffer = new Image();
		private Image imageBase = new Image(128, 128, true);
		private Configuration drawParametersBase = new Configuration();
		private Configuration drawParametersFB = new Configuration();
	}
}
