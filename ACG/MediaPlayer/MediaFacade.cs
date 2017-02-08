using ControlClassLibrary;
using System;
using TimeTools;
using WMPLib;

namespace MediaPlayer
{
	public class MediaFacade : IDisposable, ITimeSource
	{
		public event TimeFinishedHandler TimeFinished;

		public MediaFacade(string fileName)
		{
			wmp = new WindowsMediaPlayer();
			wmp.settings.autoStart = playing;
			IsLooping = false;
			wmp.settings.setMode("autoRewind", true);
			wmp.PlayStateChange += Wmp_PlayStateChange;
			var media = wmp.newMedia(fileName);
			if (0.0 == media.duration) throw new Exception("Could not load file '" + fileName + "'");
			length = (float)media.duration;
			wmp.URL = fileName;
		}

		public void Dispose()
		{
			wmp.close();
		}

		public string FileName { get { return wmp.URL; } }

		public float Length
		{
			get { return length; }
			set { throw new ArgumentException("MediaFacade cannot change Length"); }
		}

		public bool IsLooping
		{
			get { return wmp.settings.getMode("loop"); }
			set { wmp.settings.setMode("loop", value); }
		}

		public bool IsRunning
		{
			get { return playing; }
			set { playing = value; if (playing) wmp.controls.play(); else wmp.controls.pause(); }
		}

		public float Position
		{
			get { return (float)wmp.controls.currentPosition; }
			set
			{
				if (Length < value)
				{
					TimeFinished?.Invoke();
				}
				wmp.controls.currentPosition = value;
			}
		}

		private bool playing = false;
		private float length = 10.0f;
		private WindowsMediaPlayer wmp;

		private void Wmp_PlayStateChange(int NewState)
		{
			if (8 == NewState) TimeFinished?.Invoke();
		}
	}
}
