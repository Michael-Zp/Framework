using System.Diagnostics;
using System.Timers;

namespace Zenseless.TimeTools
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.TimeTools.ITimeSource" />
	public class TimeSource : ITimeSource
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TimeSource"/> class.
		/// </summary>
		/// <param name="length">The length.</param>
		public TimeSource(float length)
		{
			this.length = length;
			IsLooping = false;
			IsRunning = false;
			timer.Elapsed += OnTimeFinished;
			InitTimer(length);
		}

		/// <summary>
		/// Initializes the timer.
		/// </summary>
		/// <param name="interval">The interval.</param>
		private void InitTimer(float interval)
		{
			var isRunning = IsRunning;
			timer.Stop();
			timer.Interval = interval * 1000.0f;
			if(isRunning) timer.Start();
		}

		/// <summary>
		/// Called when [time finished].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
		private void OnTimeFinished(object sender, ElapsedEventArgs e)
		{
			TimeFinished?.Invoke(); //todo: is not called, unless position is set on soundBar1
			if (IsLooping)
			{
				Position = 0.0f;
			}
			//if the position was changed during the last run the interval is screwed up
			timer.Interval = Length * 1000.0f;
		}

		/// <summary>
		/// Gets or sets the length.
		/// </summary>
		/// <value>
		/// The length.
		/// </value>
		public float Length
		{
			get { return length; }
			set { length = value; timer.Interval = value * 1000.0f; } 
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is looping.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is looping; otherwise, <c>false</c>.
		/// </value>
		public bool IsLooping { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		public float Position
		{
			get { return sw.ElapsedMilliseconds * 0.001f + startPosition; }

			set
			{
				startPosition = value;
				if (IsRunning)
				{
					sw.Restart();
				}
				else
				{
					sw.Reset();
				}
				if (startPosition >= Length)
				{
					TimeFinished?.Invoke();
					InitTimer(Length);
					startPosition = 0.0f;
				}
				else
				{
					InitTimer(Length - startPosition);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is running.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
		/// </value>
		public bool IsRunning
		{
			get { return sw.IsRunning; }
			set	{ if (value) sw.Start(); else sw.Stop(); }
		}

		/// <summary>
		/// Occurs when [time finished].
		/// </summary>
		public event TimeFinishedHandler TimeFinished;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			timer.Dispose();
		}

		/// <summary>
		/// The sw
		/// </summary>
		private Stopwatch sw = new Stopwatch();
		/// <summary>
		/// The start position
		/// </summary>
		private float startPosition = 0.0f;
		/// <summary>
		/// The length
		/// </summary>
		private float length = 10.0f;
		/// <summary>
		/// The timer
		/// </summary>
		private Timer timer = new Timer();
	}
}
