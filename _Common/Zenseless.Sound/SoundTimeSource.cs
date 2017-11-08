﻿using Zenseless.Base;
using Zenseless.TimeTools;
using NAudio.Wave;
using System;

namespace Zenseless.Sound
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.Base.Disposable" />
	/// <seealso cref="Zenseless.TimeTools.ITimeSource" />
	public class SoundTimeSource : Disposable, ITimeSource
	{
		/// <summary>
		/// Occurs when [time finished].
		/// </summary>
		public event TimeFinishedHandler TimeFinished;

		/// <summary>
		/// Initializes a new instance of the <see cref="SoundTimeSource"/> class.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public SoundTimeSource(string fileName)
		{
			waveOutDevice = new WaveOut();
			audioFileReader = new AudioFileReader(fileName);
			loopingWaveStream = new SoundLoopStream(audioFileReader);
			loopingWaveStream.EnableLooping = false;
			waveOutDevice.Init(loopingWaveStream);
			waveOutDevice.PlaybackStopped += (s, a) => playing = false;
			length = (float)audioFileReader.TotalTime.TotalSeconds;
		}

		/// <summary>
		/// Gets or sets the length.
		/// </summary>
		/// <value>
		/// The length.
		/// </value>
		/// <exception cref="ArgumentException">NAudioFacade cannot change Length</exception>
		public float Length
		{
			get { return length; }
			set { throw new ArgumentException("NAudioFacade cannot change Length"); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is looping.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is looping; otherwise, <c>false</c>.
		/// </value>
		public bool IsLooping
		{
			get { return loopingWaveStream.EnableLooping; }
			set { loopingWaveStream.EnableLooping = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is running.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is running; otherwise, <c>false</c>.
		/// </value>
		public bool IsRunning
		{
			get { return playing; }
			set { playing = value; if (playing) waveOutDevice.Play(); else waveOutDevice.Pause(); }
		}

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		public float Position
		{
			get { return (float)audioFileReader.CurrentTime.TotalSeconds; }
			set
			{
				if (Length < value)
				{
					TimeFinished?.Invoke();
				}
				audioFileReader.CurrentTime = TimeSpan.FromSeconds(value);
			}
		}

		private IWavePlayer waveOutDevice;
		private AudioFileReader audioFileReader;
		private SoundLoopStream loopingWaveStream;

		private bool playing = false;
		private float length = 10.0f;

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			if (waveOutDevice != null)
			{
				waveOutDevice.Stop();
			}
			if (audioFileReader != null)
			{
				audioFileReader.Dispose();
				audioFileReader = null;
			}
			if (waveOutDevice != null)
			{
				waveOutDevice.Dispose();
				waveOutDevice = null;
			}
		}
	}
}
