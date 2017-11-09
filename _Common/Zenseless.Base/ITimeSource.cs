using System;

namespace Zenseless.Base
{
	/// <summary>
	/// Delegate type declaration for the time source finished handler.
	/// </summary>
	public delegate void TimeFinishedHandler();

	/// <summary>
	/// Interface for a time source. It is intended to abstract from media, like sound files or clocks
	/// A time source is something that has a length and can be started or stopped and allows seeking.
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface ITimeSource : IDisposable
	{
		/// <summary>
		/// Gets or sets the length in seconds.
		/// </summary>
		/// <value>
		/// The length in seconds.
		/// </value>
		float Length { get; set; }
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
		/// Gets or sets the position in seconds.
		/// </summary>
		/// <value>
		/// The position in seconds.
		/// </value>
		float Position { get; set; }

		/// <summary>
		/// Occurs each time the time source is finished with running (length is reached).
		/// </summary>
		event TimeFinishedHandler TimeFinished;
	}
}