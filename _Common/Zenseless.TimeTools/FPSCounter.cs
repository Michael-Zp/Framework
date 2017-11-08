using System.Diagnostics;

namespace Zenseless.TimeTools
{
	/// <summary>
	/// 
	/// </summary>
	public class FPSCounter
	{
		/// <summary>
		/// The sw
		/// </summary>
		private Stopwatch sw = new Stopwatch();
		/// <summary>
		/// The frames
		/// </summary>
		private uint frames = 0;
		/// <summary>
		/// The last time
		/// </summary>
		private long lastTime = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="FPSCounter"/> class.
		/// </summary>
		public FPSCounter()
		{
			FPS = 1;
			sw.Start();
		}

		/// <summary>
		/// Gets the FPS.
		/// </summary>
		/// <value>
		/// The FPS.
		/// </value>
		public float FPS { get; private set; }

		/// <summary>
		/// News the frame.
		/// </summary>
		public void NewFrame()
		{
			++frames;
			long newTime = sw.ElapsedMilliseconds;
			long diff = newTime - lastTime;
			if (diff > 1000)
			{
				FPS = (1000.0f * frames) / diff;
				lastTime = newTime;
				frames = 0;
			}
		}
	}
}