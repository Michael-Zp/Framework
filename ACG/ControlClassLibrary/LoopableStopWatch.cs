using System.Diagnostics;
using System.Timers;
using Zenseless.Base;
using Zenseless.Sound;

namespace ControlClassLibrary
{
	/// <summary>
	/// A stop watch type clock with a certain length or running time.
	/// Can be started or stopped and allows seeking and looping.
	/// </summary>
	/// <seealso cref="Zenseless.Base.ITimedMedia" />
	public class LoopableStopWatch : Disposable, ITimedMedia
	{
		/// <summary>
		/// Gets or sets the current time in seconds.
		/// </summary>
		/// <value>
		/// The current time in seconds.
		/// </value>
		public float Position
		{
			get { return (float)sw.Elapsed.TotalSeconds + startPosition; }

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
		/// Lopping means that after the time source was running for its length it will
		/// continue to run from the beginning
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is looping; otherwise, <c>false</c>.
		/// </value>
		public bool IsLooping { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is running.
		/// Whether or not the clock is ticking.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
		/// </value>
		public bool IsRunning
		{
			get { return sw.IsRunning; }
			set { if (value) sw.Start(); else sw.Stop(); }
		}

		/// <summary>
		/// Gets or sets the length in seconds.
		/// </summary>
		/// <value>
		/// The length in seconds.
		/// </value>
		public float Length
		{
			get { return length; }
			set { length = value; timer.Interval = value * 1000.0f; }
		}

		/// <summary>
		/// Occurs each time the time source is finished with running (length is reached).
		/// </summary>
		public event FinishedHandler TimeFinished;

		/// <summary>
		/// Initializes a new instance of the <see cref="LoopableStopWatch"/> class.
		/// Looping is off and the watch is stopped by default.
		/// </summary>
		/// <param name="length">The length or running time.</param>
		public LoopableStopWatch(float length)
		{
			this.length = length;
			IsLooping = false;
			IsRunning = false;
			timer.Elapsed += OnTimeFinished;
			InitTimer(length);
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// Disposes the internal timer her.
		/// </summary>
		protected override void DisposeResources()
		{
			timer.Dispose();
		}

		private Stopwatch sw = new Stopwatch();
		private float startPosition = 0f;
		private float length = 0f;
		private Timer timer = new Timer();

		private void InitTimer(float interval)
		{
			var isRunning = IsRunning;
			timer.Stop();
			timer.Interval = interval * 1000.0f;
			if (isRunning) timer.Start();
		}

		private void OnTimeFinished(object sender, ElapsedEventArgs e)
		{
			TimeFinished?.Invoke(); //todo: is not called, unless position is set on soundBar1
			if (IsLooping)
			{
				Position = 0.0f;
			}
			//if the position was changed during the last run the interval is screwed up
			//timer.Interval = Length * 1000.0f;
		}
	}
}
