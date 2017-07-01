using DMS.Geometry;
using DMS.HLGL;
using System.Collections.Generic;
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
			suzanne.AddInputTexture(Resources.TextureDiffuse);
			suzanne.UpdateMeshShader(Obj2Mesh.FromObj(Resourcen.suzanne), Resources.ShaderDefault);
			suzanne.ZBufferTest = true;

			copyQuad.BackfaceCulling = false;
			copyQuad.AddInputTexture("tex", imageBase);
			copyQuad.UpdateMeshShader(null, Resources.ShaderCopy);
			copyQuad.ZBufferTest = false;

			var delta = 3f;
			var extend = 10f * delta;
			var translates = new List<Vector3>();
			for (float x = -extend; x <= extend; x += delta)
			{
				for (float y = -extend; y <= extend; y += delta)
				{
					for (float z = -extend; z <= extend; z += delta)
					{
						translates.Add(new Vector3(x, y, z));
					}
				}
			}
			suzanne.UpdateInstanceAttribute("translate", translates.ToArray());
			suzanne.InstanceCount = translates.Count;
		}

		public void Render()
		{
			imageBase.Clear();

			uniforms.camera = Matrix4x4.Transpose(Camera.CalcMatrix());
			suzanne.UpdateUniforms(nameof(Uniforms), uniforms);
			imageBase.Draw(suzanne);

			frameBuffer.Draw(copyQuad);
		}

		private struct Uniforms
		{
			public Matrix4x4 camera;
		};
		struct Translate { public Vector4 translate; };
		private Translate t = new Translate() { translate = Vector4.Zero };

		private Image frameBuffer = new Image();
		private Image imageBase = new Image(1024, 1024, true);
		private Uniforms uniforms = new Uniforms();
		private DrawConfiguration suzanne = new DrawConfiguration();
		private DrawConfiguration copyQuad = new DrawConfiguration();
	}
}
