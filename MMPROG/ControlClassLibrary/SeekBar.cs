﻿using System;
using System.Windows.Forms;

namespace ControlClassLibrary
{
	public partial class SeekBar : UserControl
	{
		public delegate void PositionHandler(float position);
		public delegate void PlayingStateHandler(bool playing);
		public event TimeFinishedHandler OnFinished;
		public event PlayingStateHandler OnPlayingStateChanged;
		public event PositionHandler OnPositionChanged;

		public ITimeSource TimeSource
		{
			get { return timeSource; }
			set
			{
				if (null == value) throw new Exception("Property TimeSource is forbidden to become null!");
				timeSource.OnTimeFinished -= CallOnFinished;
				if (value != defaultTimeSource)
				{
					defaultTimeSource.IsRunning = false;
				}
				timeSource = value;
				timeSource.OnTimeFinished += CallOnFinished;
				markerBarPosition.Max = timeSource.Length;
				markerBarPosition.Value = 0.0f;
				Playing = Playing;
			}
		}

		public SeekBar()
		{
			InitializeComponent();
			defaultTimeSource =	new TimeSource(100.0f);
			timeSource = defaultTimeSource;
			timeSource.IsLooping = true;
			timeSource.OnTimeFinished += CallOnFinished;
			markerBarPosition.Max = timeSource.Length;
			Playing = false;
		}

		public bool Playing
		{
			get { return playing.Checked; }
			set
			{
				bool old = playing.Checked;
				playing.Checked = value;
				if (value)
				{
					playing.Image = global::ControlClassLibrary.Properties.Resources.PauseHS;
				}
				else
				{
					playing.Image = global::ControlClassLibrary.Properties.Resources.PlayHS;
				}
				timeSource.IsRunning = value;
				timerUpdateMarkerBar.Enabled = value;
				if (value != old && null != OnPlayingStateChanged) OnPlayingStateChanged(value);
			}
		}

		public float Position
		{
			get { return markerBarPosition.Value; }
			set	{ markerBarPosition.Value = value; }
		}

		public float Length { get { return timeSource.Length; } }

		private ITimeSource timeSource;
		private bool timerChange = false;
		private readonly TimeSource defaultTimeSource;

		private void CallOnFinished()
		{
			if (null != OnFinished) OnFinished();
		}

		private void Playing_CheckedChanged(object sender, EventArgs e)
		{
			Playing = playing.Checked;
		}

		private void TimerUpdateMarkerBar_Tick(object sender, EventArgs e)
		{
			timerChange = true;
			Position = timeSource.Position;
			timerChange = false;
			//todo: cleanup handling of valuechanged (databinding?) udpate circles!
		}

		private void MarkerBarPosition_ValueChanged(object sender, EventArgs e)
		{
			if (null != OnPositionChanged) OnPositionChanged(Position);
			if (timerChange) return;
			timeSource.Position = Position;
		}
	}
}