namespace Zenseless.HLGL
{
	public interface IRenderContext
	{
		IStateManager StateManager { get; }
		IDrawConfiguration CreateDrawConfiguration();
		IRenderSurface CreateRenderSurface(int width, int height, bool hasDepthBuffer = false, byte components = 4, bool floatingPoint = false);
		IShader CreateShader();
		void DrawPoints(int count);
		IRenderSurface GetFrameBuffer();
	}
}