using DMS.HLGL;
using System.Numerics;

namespace Example
{
	public class MainVisual
	{
		public Vector2 MousePosition { get { return mouseState.position; } set { mouseState.position = value; } }

		public MainVisual()
		{
			imageObstacles.Clear();

			drawObstacles.UpdateMeshShader(null, Resources.ShaderPaintObstacles);

			copyQuad.AddInputTexture("tex", imageObstacles);
			copyQuad.UpdateMeshShader(null, Resources.ShaderCopy);
		}

		public void Render()
		{
			drawObstacles.UpdateUniforms(nameof(MouseState), mouseState);
			imageObstacles.Draw(drawObstacles);

			frameBuffer.Clear();
			frameBuffer.Draw(copyQuad);
		}

		private struct MouseState
		{
			public Vector2 position;
		};

		private Image frameBuffer = new Image();
		private Image imageObstacles = new Image(512, 512);
		private MouseState mouseState = new MouseState();
		private DrawConfiguration drawObstacles = new DrawConfiguration();
		private DrawConfiguration copyQuad = new DrawConfiguration();
	}
}
