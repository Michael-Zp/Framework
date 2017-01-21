using System;
using System.Windows.Forms;

namespace ControlTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void draggableLayoutPanel1_ControlBoundsChanging(object sender, ControlClassLibrary.NewControlBoundsArgs e)
		{
			var delta = movableControlsLayoutPanel1.Height / 4.0;
			var bounds = e.NewBounds;
			bounds.Y = (int)Math.Round(Math.Round(bounds.Y / delta) * delta);
			e.NewBounds = bounds;
		}
	}
}
