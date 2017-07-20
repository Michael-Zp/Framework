namespace DMS.HLGL
{
	public interface IStateTyped<TYPE> : IState
	{
		TYPE Value { get; set; }
	}
}
