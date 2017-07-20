namespace DMS.HLGL
{
	public interface IStateCommand<TYPE> : IState
	{
		TYPE Value { get; set; }
	}
}
