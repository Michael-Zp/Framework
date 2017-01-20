using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ControlClassLibrary
{
	public partial class MovableControlsLayoutPanel : Panel
	{
		public MovableControlsLayoutPanel()
		{
			ControlAdded += DraggableLayoutPanel_ControlAdded;
			AllowOverlap = false;
			RestrictToPanel = true;
		}

		public bool AllowOverlap { get; set; }
		public bool RestrictToPanel { get; set; }

		public event EventHandler<NewControlBoundsArgs> ControlBoundsChanging;

		private void DraggableLayoutPanel_ControlAdded(object sender, ControlEventArgs e)
		{
			AddDragDropHandler(e.Control);
		}

		private void AddDragDropHandler(Control control)
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
					//restrict to parent bounds
					var left = e.X + control.Left - DragStart.X;
					var top  = e.Y + control.Top - DragStart.Y;

					if (RestrictToPanel)
					{
						left = Math.Max(0, left); //left min
						left = Math.Min(left, Width - control.Width); //left max
						top = Math.Max(0, top); //top min
						top = Math.Min(top, Height - control.Height); //top max
					}

					var bounds = new Rectangle(left, top, control.Width, control.Height);
					var args = new NewControlBoundsArgs(control, bounds);

					ControlBoundsChanging?.Invoke(this, args);

					if (!AllowOverlap)
					{
						foreach (var ctrl in Controls.OfType<Control>())
						{
							if (ReferenceEquals(ctrl, control)) continue;
							if (ctrl.Bounds.IntersectsWith(args.NewBounds)) return;
						}
					}

					control.Left = args.NewBounds.Left;
					control.Top = args.NewBounds.Top;
				}
			};
		}
	}
}
