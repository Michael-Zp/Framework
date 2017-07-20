namespace DMS.HLGL
{
	public interface IContext
	{
		IStateManager StateManager { get; }

		void DrawPoints(int count);
		void ClearColor();
		void ClearColorDepth();
	}
}