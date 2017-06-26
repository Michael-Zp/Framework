using DMS.Geometry;
using DMS.HLGL;
using System.Numerics;

namespace Example
{
	public class MainVisual
	{
		public CameraOrbit Camera { get; private set; } = new CameraOrbit();

		public MainVisual()
		{
			Camera.FarClip = 50f;
			Camera.Distance = 4f;
			Camera.FovY = 90f;

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

			parameters.camera = Matrix4x4.Transpose(Camera.CalcMatrix());
			var delta = 3f;
			for (float x = -delta; x <= delta; x += delta)
			{
				for (float y = -delta; y <= delta; y += delta)
				{
					parameters.translate = new Vector3(x, y, 0);
					suzanne.UpdateParameters(parameters);
					imageBase.Draw(suzanne);
				}
			}

			frameBuffer.Draw(copyQuad);
		}

		private struct ShaderParameter
		{
			public Matrix4x4 camera;
			public Vector3 translate;
		};

		private Image frameBuffer = new Image();
		private Image imageBase = new Image(1024, 1024, true);
		private ShaderParameter parameters = new ShaderParameter() { translate = Vector3.Zero };
		private DrawConfiguration suzanne = new DrawConfiguration();
		private DrawConfiguration copyQuad = new DrawConfiguration();
	}
}
