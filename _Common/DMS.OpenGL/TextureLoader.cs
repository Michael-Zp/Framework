using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DMS.OpenGL
{
	using SysDraw = System.Drawing.Imaging;

	public static class TextureLoader
	{
		public static Texture FromBitmap(Bitmap bitmap)
		{
			Texture texture = new Texture();
			texture.FilterMipmap();
			texture.Activate();
			//todo: 16bit channels
			using (Bitmap bmp = new Bitmap(bitmap))
			{
				bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
				BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
				PixelInternalFormat internalFormat = SelectInternalPixelFormat(bmp.PixelFormat);
				OpenTK.Graphics.OpenGL.PixelFormat inputPixelFormat = SelectPixelFormat(bmp.PixelFormat);
				texture.LoadPixels(bmpData.Scan0, bmpData.Width, bmpData.Height, internalFormat, inputPixelFormat, PixelType.UnsignedByte);
				bmp.UnlockBits(bmpData);
			}
			texture.Deactivate();
			return texture;
		}

		public static Texture FromFile(string fileName)
		{
			if (String.IsNullOrEmpty(fileName))
			{
				throw new ArgumentException(fileName);
			}
			if (!File.Exists(fileName))
			{
				throw new FileLoadException(fileName);
			}
			return FromBitmap(new Bitmap(fileName));
		}

		public static void SaveToFile(Texture texture, string fileName, SysDraw.PixelFormat format = SysDraw.PixelFormat.Format32bppArgb)
		{
			using (var bitmap = SaveToBitmap(texture, format))
			{
				bitmap.Save(fileName);
			}
		}

		public static Bitmap SaveToBitmap(Texture texture, SysDraw.PixelFormat format = SysDraw.PixelFormat.Format32bppArgb)
		{
			try
			{ 
				var bmp = new Bitmap(texture.Width, texture.Height);
				texture.Activate();
				BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, format);
				GL.GetTexImage(TextureTarget.Texture2D, 0, SelectPixelFormat(format), PixelType.UnsignedByte, data.Scan0);
				bmp.UnlockBits(data);
				texture.Deactivate();
				bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
				return bmp;
			}
			catch
			{
				texture.Deactivate();
				return null;
			}
		}

		public static OpenTK.Graphics.OpenGL.PixelFormat SelectPixelFormat(SysDraw.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case SysDraw.PixelFormat.Format8bppIndexed: return OpenTK.Graphics.OpenGL.PixelFormat.Red;
				case SysDraw.PixelFormat.Format24bppRgb: return OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
				case SysDraw.PixelFormat.Format32bppArgb: return OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
				default: throw new FileLoadException("Wrong pixel format " + pixelFormat.ToString());
			}
		}

		public static PixelInternalFormat SelectInternalPixelFormat(SysDraw.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case SysDraw.PixelFormat.Format8bppIndexed: return PixelInternalFormat.Luminance;
				case SysDraw.PixelFormat.Format24bppRgb: return PixelInternalFormat.Rgb;
				case SysDraw.PixelFormat.Format32bppArgb: return PixelInternalFormat.Rgba;
				default: throw new FileLoadException("Wrong pixel format " + pixelFormat.ToString());
			}
		}
	}
}
