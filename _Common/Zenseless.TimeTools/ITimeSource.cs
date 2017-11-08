﻿using System;

namespace Zenseless.TimeTools
{
	/// <summary>
	/// 
	/// </summary>
	public delegate void TimeFinishedHandler();

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface ITimeSource : IDisposable
	{
		/// <summary>
		/// Gets or sets the length.
		/// </summary>
		/// <value>
		/// The length.
		/// </value>
		float Length { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is looping.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is looping; otherwise, <c>false</c>.
		/// </value>
		bool IsLooping { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is running.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
		/// </value>
		bool IsRunning { get; set; }
		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		float Position { get; set; }

		/// <summary>
		/// Occurs when [time finished].
		/// </summary>
		event TimeFinishedHandler TimeFinished;
	}
}