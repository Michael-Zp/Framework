namespace Zenseless.HLGL
{
	public interface IStateTyped<TYPE> : IState
	{
		TYPE Value { get; set; }
	}
}
