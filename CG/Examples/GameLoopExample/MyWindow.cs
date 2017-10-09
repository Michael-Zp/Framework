﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Example
{
	public class MyWindow : GameWindow
	{
		public MyWindow(int width = 512, int height = 512): base(width, height)
		{
			KeyDown += MyWindow_KeyDown;
			Visible = true;
		}

		private void MyWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (Key.Escape == e.Key) close = true; //if Escape is pressed end program
		}

		public bool WaitForNextFrame()
		{
			if (close) return false;
			SwapBuffers(); //double buffering
			ProcessEvents(); //handle all events that are sent to the window (user inputs, operating system stuff); this call could destroy window, so check immediatily after this call if window still exists, otherwise gl calls will fail.
			return Exists;
		}

		private bool close = false;
	}
}
