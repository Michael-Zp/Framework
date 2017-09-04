namespace Zenseless.HLGL
{
	public interface ICreator<TYPE> : IState
	{
		TypedHandle<TYPE> Create();
	}
}
