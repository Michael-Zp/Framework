using Zenseless.Sound;
using System;
using System.Diagnostics;
using System.IO;
using ControlClassLibrary;

namespace ShaderForm.Demo
{
	public class DemoTimeSource : ITimedMedia
	{
		public string SoundFileName { get; private set; }

		public float Length
		{
			get { return timeSource.Length; }
			set { timeSource.Length = value; }
		}

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

		public event EventHandler Loaded;
		public event FinishedHandler TimeFinished;

		public DemoTimeSource(bool isLooping)
		{
			SoundFileName = string.Empty;
			timeSource = new LoopableStopWatch(100.0f)
			{
				IsLooping = isLooping
			};
			timeSource.TimeFinished += CallOnTimeFinished;
		}

		public static ITimedMedia FromMediaFile(string fileName)
		{
			try
			{
				if (File.Exists(fileName))
				{
					var absoluteFileName = Path.GetFullPath(fileName);
					return new SoundTimeSource(absoluteFileName);
				}
				return null;
			}
			catch
			{
				return null;
			}
		}

		public void Load(ITimedMedia newTimeSource, string soundFileName)
		{
			Debug.Assert(!ReferenceEquals(null,  timeSource));
			if (ReferenceEquals(null,  newTimeSource))
			{
				Clear();
			}
			else
			{
				newTimeSource.IsLooping = IsLooping;
				newTimeSource.TimeFinished += CallOnTimeFinished;
				timeSource.Dispose();
				timeSource = newTimeSource;
				SoundFileName = soundFileName;
				Loaded?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Clear()
		{
			Debug.Assert(!ReferenceEquals(null,  timeSource));
			//keep looping state
			bool isLooping = timeSource.IsLooping;
			//remove old
			timeSource.Dispose();
			//create new
			timeSource = new LoopableStopWatch(100.0f);
			SoundFileName = string.Empty;
			timeSource.IsLooping = isLooping;
			timeSource.TimeFinished += CallOnTimeFinished;
			Loaded?.Invoke(this, EventArgs.Empty);
		}

		public void Dispose()
		{
			Debug.Assert(!ReferenceEquals(null, timeSource));
			timeSource.Dispose();
		}

		private ITimedMedia timeSource;

		private void CallOnTimeFinished()
		{
			TimeFinished?.Invoke();
		}
	}
}
