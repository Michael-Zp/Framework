using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlClassLibrary
{
	public class TrackItem : Button, ITrackItem
	{
		public TrackItem(TrackView trackView, string label, float start, float length, int track)
		{
			Text = label;
			Start = start;
			Length = length;
			Track = track;
			this.trackView = trackView;

			BackColor = trackView.NewColor();
			Parent = trackView;
			Init(this, trackView, this);
			UpdateButton(this, this);
		}

		public string Label	{ get { return Text; } set { Text = value; } }
		public float Start { get; set; }
		public float Length { get; set; }
		public int Track { get; set; }

		private TrackView trackView;

		private int GetTrackHeight()
		{
			return trackView.Height / TrackView.trackCount;
		}


		private void Init(Control control, TrackView trackView, TrackItem item)
		{
			bool Dragging = false;
			Point DragStart = Point.Empty;
			control.MouseDown += delegate (object sender, MouseEventArgs e)
			{
				Dragging = true;
				DragStart = e.Location;
				control.Capture = true;
			};
			control.MouseUp += delegate (object sender, MouseEventArgs e)
			{
				Dragging = false;
				control.Capture = false;
			};
			control.MouseMove += delegate (object sender, MouseEventArgs e)
			{
				if (Dragging)
				{
					//min
					var left = Math.Max(0, e.X + control.Left - DragStart.X);
					//max
					left = Math.Min(left, trackView.Width - control.Width);
					control.Left = left;
					//min
					var top = Math.Max(0, e.Y + control.Top - DragStart.Y);
					//max
					top = Math.Min(top, trackView.Height - control.Height);
					//track
					item.Track = top / GetTrackHeight();
					control.Top = item.Track * GetTrackHeight();
					UpdateItem(this, this);
				}
			};
		}
		private void UpdateButton(Button btn, TrackItem item)
		{
			btn.Left = (int)Math.Round(trackView.Width * (item.Start / trackView.Length));
			btn.Width = (int)Math.Round(trackView.Width * (item.Length / trackView.Length));
			btn.Top = item.Track * GetTrackHeight();
			//btn.Text = item.Label;
		}
		private void UpdateItem(Button btn, TrackItem item)
		{
			item.Start = btn.Left * trackView.Length / trackView.Width;
			item.Length = btn.Width * trackView.Length / trackView.Width;
			//item.Label = btn.Text;
		}
	}
}
