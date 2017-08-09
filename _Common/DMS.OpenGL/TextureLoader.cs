using DMS.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace DMS.OpenGL
{
	using SysDraw = System.Drawing.Imaging;
	using SysMedia = System.Windows.Media;

	public static class TextureLoader
	{
		public static ITexture FromArray<TYPE>(TYPE[,] data, PixelInternalFormat internalFormat, PixelFormat format, PixelType type)
		{
			GCHandle pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				IntPtr pointer = pinnedArray.AddrOfPinnedObject();
				var width = data.GetLength(0);
				var height = data.GetLength(1);
				var texture = new Texture2D();
				texture.Filter = TextureFilterMode.Mipmap;
				texture.Activate();
				texture.LoadPixels(pointer, width, height, internalFormat, format, type);
				texture.Deactivate();
				return texture;
			}
			finally
			{
				pinnedArray.Free();
			}
		}

		public static ITexture FromBitmap(Bitmap bitmap)
		{
			var texture = new Texture2D();
			texture.Filter = TextureFilterMode.Mipmap;
			texture.Activate();
			//todo: 16bit channels
			using (Bitmap bmp = new Bitmap(bitmap))
			{
				bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
				var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), SysDraw.ImageLockMode.ReadOnly, bmp.PixelFormat);
				var internalFormat = SelectInternalPixelFormat(bmp.PixelFormat);
				var inputPixelFormat = SelectPixelFormat(bmp.PixelFormat);
				texture.LoadPixels(bmpData.Scan0, bmpData.Width, bmpData.Height, internalFormat, inputPixelFormat, PixelType.UnsignedByte);
				bmp.UnlockBits(bmpData);
			}
			texture.Deactivate();
			return texture;
		}

		public static ITexture FromStream(Stream stream)
		{
			var texture = new Texture2D();
			texture.Filter = TextureFilterMode.Mipmap;
			texture.Activate();
			var source = new SysMedia.Imaging.BitmapImage();
			source.BeginInit();
			source.StreamSource = stream;
			source.EndInit();
			var writable = new SysMedia.Imaging.WriteableBitmap(source);
			writable.Lock();
			var internalFormat = SelectInternalPixelFormat(source.Format);
			var inputPixelFormat = SelectPixelFormat(source.Format);
			texture.LoadPixels(writable.BackBuffer, source.PixelWidth, source.PixelHeight, internalFormat, inputPixelFormat, PixelType.UnsignedByte);
			writable.Unlock();
			texture.Deactivate();
			return texture;
		}

		public static ITexture FromFile(string fileName)
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

		public static void SaveToFile(ITexture texture, string fileName, SysDraw.PixelFormat format = SysDraw.PixelFormat.Format32bppArgb)
		{
			using (var bitmap = SaveToBitmap(texture, format))
			{
				bitmap.Save(fileName);
			}
		}

		public static Bitmap SaveToBitmap(ITexture texture, SysDraw.PixelFormat format = SysDraw.PixelFormat.Format32bppArgb)
		{
			try
			{ 
				var bmp = new Bitmap(texture.Width, texture.Height);
				texture.Activate();
				var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), SysDraw.ImageLockMode.WriteOnly, format);
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

		public static PixelFormat SelectPixelFormat(SysDraw.PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case SysDraw.PixelFormat.Format8bppIndexed: return PixelFormat.Red;
				case SysDraw.PixelFormat.Format24bppRgb: return PixelFormat.Bgr;
				case SysDraw.PixelFormat.Format32bppArgb: return PixelFormat.Bgra;
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

		private static PixelInternalFormat SelectInternalPixelFormat(SysMedia.PixelFormat pixelFormat)
		{
			if (SysMedia.PixelFormats.Bgra32 == pixelFormat)
			{
				return PixelInternalFormat.Rgba;
			}
			else if (SysMedia.PixelFormats.Rgb24 == pixelFormat)
			{
				return PixelInternalFormat.Rgb;
			}
			else if (SysMedia.PixelFormats.Gray8 == pixelFormat)
			{
				return PixelInternalFormat.Luminance;
			}
			else throw new FileLoadException("Wrong pixel format " + pixelFormat.ToString());
		}

		private static PixelFormat SelectPixelFormat(SysMedia.PixelFormat pixelFormat)
		{
			if (SysMedia.PixelFormats.Bgra32 == pixelFormat)
			{
				return PixelFormat.Bgra;
			}
			else if (SysMedia.PixelFormats.Rgb24 == pixelFormat)
			{
				return PixelFormat.Bgr;
			}
			else if (SysMedia.PixelFormats.Gray8 == pixelFormat)
			{
				return PixelFormat.Red;
			}
			else throw new FileLoadException("Wrong pixel format " + pixelFormat.ToString());
		}
	}
}
