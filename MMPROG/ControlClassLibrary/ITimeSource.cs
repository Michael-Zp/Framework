using System;

namespace ControlClassLibrary
{
	public delegate void TimeFinishedHandler();

	public interface ITimeSource : IDisposable
	{
		float Length { get; }
		bool IsLooping { get; set; }
		bool IsRunning { get; set; }
		float Position { get; set; }

		event TimeFinishedHandler OnTimeFinished;
	}
}