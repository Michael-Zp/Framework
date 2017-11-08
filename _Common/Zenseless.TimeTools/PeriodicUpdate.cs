namespace Zenseless.TimeTools
{
	/// <summary>
	/// Invokes a registered callback in regular intervalls
	/// </summary>
	/// <seealso cref="Zenseless.TimeTools.ITimedUpdate" />
	public class PeriodicUpdate : ITimedUpdate
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PeriodicUpdate"/> class.
		/// </summary>
		/// <param name="interval">The interval.</param>
		public PeriodicUpdate(float interval)
		{
			Interval = interval;
			PeriodElapsedCount = 0;
			Enabled = false;
			PeriodRelativeTime = 0;
		}

		/// <summary>
		/// Gets the period elapsed count.
		/// </summary>
		/// <value>
		/// The period elapsed count.
		/// </value>
		public uint PeriodElapsedCount { get; private set; }
		/// <summary>
		/// Gets the period relative time.
		/// </summary>
		/// <value>
		/// The period relative time.
		/// </value>
		public float PeriodRelativeTime { get; private set; }
		/// <summary>
		/// Gets a value indicating whether this <see cref="PeriodicUpdate"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender">The sender.</param>
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
		/// Starts the specified start time.
		/// </summary>
		/// <param name="startTime">The start time.</param>
		public void Start(float startTime)
		{
			absoluteTime = startTime;
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
				this.absoluteTime = absoluteTime;
				PeriodRelativeTime = 0.0f;
				return;
			}
			PeriodRelativeTime = absoluteTime - this.absoluteTime;
			if (PeriodRelativeTime > Interval)
			{
				PeriodElapsed?.Invoke(this, absoluteTime);
				this.absoluteTime = absoluteTime;
				PeriodRelativeTime = 0.0f;
				++PeriodElapsedCount;
			}
		}

		/// <summary>
		/// The absolute time
		/// </summary>
		private float absoluteTime = 0.0f;
	}
}
