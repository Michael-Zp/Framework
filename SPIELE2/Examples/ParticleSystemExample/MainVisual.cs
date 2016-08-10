using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public CameraOrbit OrbitCamera { get { return camera; } }

		public MainVisual()
		{
			plane = new VisualPlane();

			visualSmoke = new VisualSmoke(Vector3.Zero, new Vector3(.2f, 0, 0));
			//todo: add particles that bounce off plane
			visualWaterfall = new VisualWaterfall(new Vector3(-.5f, 1, -.5f));

			camera.FarClip = 20;
			camera.Distance = 2;
			camera.FovY = 70;
			camera.Tilt = 15;

			GL.Enable(EnableCap.DepthTest);
		}

		public void Update(float time)
		{
			visualSmoke.Update(time);
			visualWaterfall.Update(time);
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var cam = camera.CalcMatrix();
			plane.Draw(cam);
			visualSmoke.Render(cam);
			visualWaterfall.Render(cam);
		}

		private CameraOrbit camera = new CameraOrbit();

		private VisualPlane plane;
		private readonly VisualSmoke visualSmoke;
		private readonly VisualWaterfall visualWaterfall;
	}
}
