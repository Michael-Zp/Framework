using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			var delta = draggableLayoutPanel1.Height / 5.0;
			var bounds = e.NewBounds;
			bounds.Y = (int)Math.Round(Math.Round(bounds.Y / delta) * delta);
			e.NewBounds = bounds;
		}
	}
}
