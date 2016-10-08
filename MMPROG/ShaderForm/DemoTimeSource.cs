using ControlClassLibrary;
using Framework;
using MediaPlayer;
using System;
using System.Diagnostics;
using System.IO;

namespace ShaderForm
{
	public class DemoTimeSource : ITimeSource
	{
		public string SoundFileName { get; private set; }

		public float Length { get { return timeSource.Length; } }

		public bool IsLooping
		{
			get
			{
				return timeSource.IsLooping;
			}

			set
			{
				timeSource.IsLooping = value;
			}
		}

		public bool IsRunning
		{
			get
			{
				return timeSource.IsRunning;
			}

			set
			{
				timeSource.IsRunning = value;
			}
		}

		public float Position
		{
			get
			{
				return timeSource.Position;
			}

			set
			{
				timeSource.Position = value;
			}
		}

		public event EventHandler OnLoaded;
		public event TimeFinishedHandler OnTimeFinished;

		public DemoTimeSource(bool isLooping)
		{
			SoundFileName = string.Empty;
			timeSource = new TimeSource(100.0f);
			timeSource.IsLooping = isLooping;
			timeSource.OnTimeFinished += CallOnTimeFinished;
		}

		public static ITimeSource FromMediaFile(string fileName)
		{
			try
			{
				if (File.Exists(fileName))
				{
					var absoluteFileName = Path.GetFullPath(fileName);
					return new MediaFacade(absoluteFileName);
				}
				return null;
			}
			catch
			{
				return null;
			}
		}

		public void Load(ITimeSource newTimeSource)
		{
			Debug.Assert(null != timeSource);
			if (null == newTimeSource)
			{
				Clear();
			}
			else
			{
				newTimeSource.IsLooping = IsLooping;
				newTimeSource.OnTimeFinished += CallOnTimeFinished;
				timeSource.Dispose();
				timeSource = newTimeSource;
				OnLoaded?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Clear()
		{
			Debug.Assert(null != timeSource);
			//keep looping state
			bool isLooping = timeSource.IsLooping;
			//remove old
			timeSource.Dispose();
			//create new
			timeSource = new TimeSource(100.0f);
			SoundFileName = string.Empty;
			timeSource.IsLooping = isLooping;
			timeSource.OnTimeFinished += CallOnTimeFinished;
			OnLoaded?.Invoke(this, EventArgs.Empty);
		}

		public void Dispose()
		{
			Debug.Assert(null != timeSource);
			timeSource.Dispose();
		}

		private ITimeSource timeSource;

		private void CallOnTimeFinished()
		{
			OnTimeFinished?.Invoke();
		}
	}
}
