namespace DMS.OpenGL
{
	public interface IWindow //todo: split into two interfaces
	{
		void Render();
		void Update(float updatePeriod);
	}
}
