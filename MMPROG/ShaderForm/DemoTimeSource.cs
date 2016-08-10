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

		public bool Load(string fileName)
		{
			Debug.Assert(null != timeSource);
			try
			{
				bool isLooping = timeSource.IsLooping;
				if (File.Exists(fileName))
				{
					var absoluteFileName = Path.GetFullPath(fileName);
					var sound = new MediaFacade(absoluteFileName);
					SoundFileName = absoluteFileName;
					timeSource.Dispose();
					timeSource = sound;
				}
				else
				{
					timeSource.Dispose();
					timeSource = new TimeSource(100.0f);
					SoundFileName = string.Empty;
				}
				timeSource.IsLooping = isLooping;
				timeSource.OnTimeFinished += CallOnTimeFinished;
				if (null != OnLoaded) OnLoaded(this, EventArgs.Empty);
				return true;
			}
			catch (Exception)
			{
				if (null != OnLoaded) OnLoaded(this, EventArgs.Empty);
				return false;
			}
		}

		public void Clear()
		{
			Load(string.Empty);
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		private ITimeSource timeSource;

		private void CallOnTimeFinished()
		{
			if (null != OnTimeFinished) OnTimeFinished();
		}
	}
}
