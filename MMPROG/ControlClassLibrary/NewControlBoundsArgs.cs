using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlClassLibrary
{
	public class NewControlBoundsArgs : EventArgs
	{
		public NewControlBoundsArgs(Control control, Rectangle newBounds)
		{
			Control = control;
			NewBounds = newBounds;
		}

		public Rectangle NewBounds { get; set; }
		public Control Control { get; private set; }
	}
}
