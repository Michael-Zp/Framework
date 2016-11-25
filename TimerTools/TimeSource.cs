using System.Diagnostics;
using System.Timers;

namespace TimeTools
{
	public class TimeSource : ITimeSource
	{
		public TimeSource(float length)
		{
			this.length = length;
			IsLooping = false;
			IsRunning = false;
			timer.Elapsed += TimeFinished;
			InitTimer(length);
		}

		private void InitTimer(float interval)
		{
			var isRunning = IsRunning;
			timer.Stop();
			timer.Interval = interval * 1000.0f;
			if(isRunning) timer.Start();
		}

		private void TimeFinished(object sender, ElapsedEventArgs e)
		{
			OnTimeFinished?.Invoke();
			if (IsLooping)
			{
				Position = 0.0f;
			}
			//if the position was changed during the last run the interval is screwed up
			timer.Interval = Length * 1000.0f;
		}

		public float Length
		{
			get { return length; }
			set { length = value; timer.Interval = value * 1000.0f; } 
		}

		public bool IsLooping { get; set; }

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
					OnTimeFinished?.Invoke();
					InitTimer(Length);
					startPosition = 0.0f;
				}
				else
				{
					InitTimer(Length - startPosition);
				}
			}
		}

		public bool IsRunning
		{
			get { return sw.IsRunning; }
			set	{ if (value) sw.Start(); else sw.Stop(); }
		}

		public event TimeFinishedHandler OnTimeFinished;

		public void Dispose()
		{
			timer.Dispose();
		}

		private Stopwatch sw = new Stopwatch();
		private float startPosition = 0.0f;
		private float length = 10.0f;
		private Timer timer = new Timer();
	}
}
