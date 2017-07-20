namespace DMS.HLGL
{
	public interface IContext
	{
		IStateManager StateManager { get; }

		void ClearColor();
		void ClearColorDepth();
	}
}