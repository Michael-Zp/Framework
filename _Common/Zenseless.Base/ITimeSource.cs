using System;

namespace Zenseless.Base
{
	/// <summary>
	/// Delegate type declaration for the time source finished handler.
	/// </summary>
	public delegate void TimeFinishedHandler();

	/// <summary>
	/// Interface for a time source. Something like an abstract stop watch. 
	/// It is intended to abstract from media, like sound files or clocks
	/// A time source is something that has a length or running time.
	/// Can be started or stopped and allows seeking (like turning the clock) and looping.
	/// Looping is off and the time source is in stopped state by default.
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface ITimeSource : IDisposable
	{
		/// <summary>
		/// Gets or sets the current time in seconds.
		/// </summary>
		/// <value>
		/// The current time in seconds.
		/// </value>
		float CurrentTime { get; set; }

		/// <summary>
		/// Lopping means that after the time source was running for its length it will 
		/// continue to run from the beginning
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is looping; otherwise, <c>false</c>.
		/// </value>
		bool IsLooping { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is running and the position is changing.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
		/// </value>
		bool IsRunning { get; set; }

		/// <summary>
		/// Gets or sets the length in seconds.
		/// </summary>
		/// <value>
		/// The length in seconds.
		/// </value>
		float Length { get; set; }

		/// <summary>
		/// Occurs each time the time source is finished with running (length is reached).
		/// </summary>
		event TimeFinishedHandler TimeFinished;
	}
}