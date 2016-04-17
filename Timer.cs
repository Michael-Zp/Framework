namespace Framework
{
	public class Timer : ITimedUpdate
	{
		public Timer(float interval, bool enabled)
		{
			this.Interval = interval;
			this.Count = 0;
			this.Enabled = enabled;
		}

		public uint Count { get; private set; }
		public bool Enabled { get; set; }
		public delegate void TimerElapsed();
		public event TimerElapsed OnTimerElapsed;
		public float Interval { get; set; }

		public void Update(float absoluteTime)
		{
			if (!Enabled)
			{
				lastElapsedTime = absoluteTime;
				return;
			}
			float delta = absoluteTime - lastElapsedTime;
			if (delta > Interval)
			{
				OnTimerElapsed?.Invoke();
				lastElapsedTime = absoluteTime;
				++Count;
			}
		}

		private float lastElapsedTime = 0.0f;
	}
}
