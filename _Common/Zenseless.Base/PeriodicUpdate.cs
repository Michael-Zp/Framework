namespace Zenseless.Base
{
	/// <summary>
	/// Invokes a registered callback in regular intervals
	/// </summary>
	/// <seealso cref="Zenseless.Base.ITimedUpdate" />
	public class PeriodicUpdate : ITimedUpdate
	{
		/// <summary>
		/// Gets how often the period has elapsed.
		/// </summary>
		/// <value>
		/// The period elapsed count.
		/// </value>
		public uint PeriodElapsedCount { get; private set; } = 0;
		/// <summary>
		/// Gets the period relative time. The time that has elapsed since the current period has started.
		/// </summary>
		/// <value>
		/// The time that has elapsed since the current period has started.
		/// </value>
		public float PeriodRelativeTime { get; private set; } = 0;
		/// <summary>
		/// Gets a value indicating whether this <see cref="PeriodicUpdate"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled { get; private set; } = false;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender">The <see cref="PeriodicUpdate"/> instance that called sender.</param>
		/// <param name="absoluteTime">The absolute time.</param>
		public delegate void PeriodElapsedHandler(PeriodicUpdate sender, float absoluteTime);
		/// <summary>
		/// Occurs when [period elapsed].
		/// </summary>
		public event PeriodElapsedHandler PeriodElapsed;
		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>
		/// The interval.
		/// </value>
		public float Interval { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PeriodicUpdate"/> class.
		/// </summary>
		/// <param name="interval">The regular interval in which <see cref="PeriodElapsed"/> will be called.</param>
		public PeriodicUpdate(float interval)
		{
			Interval = interval;
		}

		/// <summary>
		/// Starts the specified start time.
		/// </summary>
		/// <param name="startTime">The start time.</param>
		public void Start(float startTime)
		{
			absoluteStartTime = startTime;
			Enabled = true;
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		public void Stop()
		{
			Enabled = false;
		}

		/// <summary>
		/// Updates the specified absolute time.
		/// </summary>
		/// <param name="absoluteTime">The absolute time.</param>
		public void Update(float absoluteTime)
		{
			if (!Enabled)
			{
				absoluteStartTime = absoluteTime;
				PeriodRelativeTime = 0.0f;
				return;
			}
			PeriodRelativeTime = absoluteTime - absoluteStartTime;
			if (PeriodRelativeTime > Interval)
			{
				PeriodElapsed?.Invoke(this, absoluteTime);
				absoluteStartTime = absoluteTime;
				PeriodRelativeTime = 0.0f;
				++PeriodElapsedCount;
			}
		}

		/// <summary>
		/// The absolute time in seconds
		/// </summary>
		private float absoluteStartTime = 0.0f;
	}
}
