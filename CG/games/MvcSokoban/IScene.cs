using OpenTK.Input;

namespace MvcSokoban
{
	public interface IScene
	{
		bool HandleInput(Key key);
		void Render();
	}
}