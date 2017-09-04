namespace Zenseless.HLGL
{
	public interface IStateManager
	{
		INTERFACE Get<INTERFACE, KEYTYPE>()
			where INTERFACE : class, IState
			where KEYTYPE : IState;
		void Register<INTERFACE, KEYTYPE>(IState stateImplementation)
			where INTERFACE : IState
			where KEYTYPE : INTERFACE;
	}
}