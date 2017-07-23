using DMS.Application;
using DMS.Geometry;
using DMS.HLGL;
using System.Collections.Generic;
using System.Numerics;

namespace Example
{
	public class MainVisual
	{
		public CameraOrbit Camera { get; private set; } = new CameraOrbit();

		public MainVisual(IContext context)
		{
			Camera.FarClip = 50f;
			Camera.Distance = 1.5f;
			Camera.FovY = 90f;
			Camera.Azimuth = 90;
			Camera.Elevation = 20;

			//frameBuffer = context.GetFrameBuffer();
			//var context = imageBase.Context;
			//suzanne = context.CreateDrawConfiguration();
			suzanne.BackfaceCulling = true;
			suzanne.SetInputTexture(Resources.TextureDiffuse);
			//model from https://sketchfab.com/models/e925320e1d5744d9ae661aeff61e7aef
			var mesh = Obj2Mesh.FromObj(Resourcen.chalet1).Transform(Matrix4x4.CreateRotationX(-0.5f * MathHelper.PI));
			suzanne.UpdateMeshShader(mesh, Resources.ShaderDefault);
			suzanne.ZBufferTest = true;

			copyQuad.BackfaceCulling = false;
			copyQuad.SetInputTexture("tex", imageBase);
			copyQuad.UpdateMeshShader(null, Resources.ShaderCopy);
			copyQuad.ZBufferTest = false;

			var delta = 2f;
			var extend = 1f * delta;
			var translates = new List<Vector3>();
			for (float x = -extend; x <= extend; x += delta)
			{
				for (float z = -extend; z <= extend; z += delta)
				{
					translates.Add(new Vector3(x, 0, z));
				}
			}
			suzanne.UpdateInstanceAttribute("translate", translates.ToArray());
			suzanne.InstanceCount = translates.Count;
		}

		internal void Resize(int width, int height)
		{
			imageBase = new Image(width, height, true);
			copyQuad.SetInputTexture("tex", imageBase);
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
		private Image imageBase = new Image(512, 512, true);
		private Uniforms uniforms = new Uniforms();
		private DrawConfiguration suzanne = new DrawConfiguration();
		private DrawConfiguration copyQuad = new DrawConfiguration();
	}
}
