namespace DMS.HLGL
{
	public interface IContext
	{
		IStateManager StateManager { get; }

		//CreateImage();
		void DrawPoints(int count);
		void ClearColor();
		void ClearColorDepth();
		IImage GetFrameBuffer();
		IDrawConfiguration CreateDrawConfiguration();
		IImage CreateImage(int width, int height, bool hasDepthBuffer = false, byte components = 4, bool floatingPoint = false);
	}
}