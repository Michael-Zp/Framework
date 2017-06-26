using DMS.Geometry;
using DMS.HLGL;
using System.Numerics;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			suzanne.BackfaceCulling = true;
			suzanne.AddTexture(Resources.TextureDiffuse);
			suzanne.UpdateMeshShader(Obj2Mesh.FromObj(Resourcen.suzanne), Resources.Shader);
			suzanne.ZBufferTest = true;

			copyQuad.BackfaceCulling = false;
			copyQuad.AddTexture(new NamedTexture("tex", imageBase.Texture));
			copyQuad.UpdateMeshShader(null, Resources.ShaderCopy);
			copyQuad.ZBufferTest = false;
		}

		public void Render()
		{
			imageBase.Clear();
			//todo: draw object multiple times -> shader parameter
			suzanne.UpdateParameters(new Vector3(1, 0, 0));
			imageBase.Draw(suzanne);
			frameBuffer.Draw(copyQuad);
		}

		private Image frameBuffer = new Image();
		private Image imageBase = new Image(128, 128, true);
		private DrawConfiguration suzanne = new DrawConfiguration();
		private DrawConfiguration copyQuad = new DrawConfiguration();
	}
}
