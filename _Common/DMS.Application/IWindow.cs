namespace DMS.Application
{
	public interface IWindow //todo: split into two interfaces
	{
		void Render();
		void Update(float updatePeriod);
	}
}
