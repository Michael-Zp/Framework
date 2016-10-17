using System.Diagnostics;

namespace TimerTools
{
	public class FPSCounter
	{
		private Stopwatch sw = new Stopwatch();
		private float smoothingInterval;
		private uint frames = 0;
		private long lastTime = 0;

		public FPSCounter(float smoothingInterval)
		{
			FPS = 1;
			this.smoothingInterval = smoothingInterval;
			sw.Start();
		}

		public float FPS { get; private set; }

		public void NewFrame()
		{
			++frames;
			long newTime = sw.ElapsedMilliseconds;
			long diff = newTime - lastTime;
			if (diff > 500)
			{
				FPS = (1000.0f * frames) / diff;
				lastTime = newTime;
				frames = 0;
			}
		}
	}
}