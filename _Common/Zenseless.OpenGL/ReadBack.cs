using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// Contains methods for saving (rading back from the graphcis card) the frame buffer into a Bitmap
	/// </summary>
	public static class ReadBack
	{
		/// <summary>
		/// Saves a rectangular area of the current frame buffer into a Bitmap
		/// </summary>
		/// <param name="x">start position in x-direction</param>
		/// <param name="y">start position in y-direction</param>
		/// <param name="width">size in x-direction</param>
		/// <param name="height">size in y-direction</param>
		/// <returns>Bitmap</returns>
		public static Bitmap FrameBuffer(int x, int y, int width, int height)
		{
			var format = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
			var bmp = new Bitmap(width, height);
			BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, format);
			GL.ReadPixels(x, y, width, height, TextureLoader.SelectPixelFormat(format), PixelType.UnsignedByte, data.Scan0);
			bmp.UnlockBits(data);
			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
			return bmp;
		}

		/// <summary>
		/// Saves the contents of the current frame buffer into a Bitmap
		/// </summary>
		/// <returns>Bitmap</returns>
		public static Bitmap FrameBuffer()
		{
			var viewport = new int[4];
			GL.GetInteger(GetPName.Viewport, viewport);
			return FrameBuffer(viewport[0], viewport[1], viewport[2], viewport[3]);
		}
	}
}
