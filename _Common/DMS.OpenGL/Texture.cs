using DMS.Base;
using OpenTK.Graphics.OpenGL4;
using System;

namespace DMS.OpenGL
{
	public enum TextureFilterMode { Nearest, Linear, Mipmap };
	public enum TextureWrapFunction { Repeat, MirroredRepeat, ClampToEdge, ClampToBorder };

	/// <summary>
	/// Gl Texture class that allows loading from a file.
	/// </summary>
	public class Texture : Disposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Texture"/> class.
		/// </summary>
		public Texture()
		{
			//generate one texture and put its ID number into the "m_uTextureID" variable
			GL.GenTextures(1, out m_uTextureID);
			//GL.CreateTextures(target, 1, out m_uTextureID); //DSA not supported by intel
			Width = 0;
			Height = 0;
		}

		public void WrapMode(TextureWrapFunction wrapFunc)
		{
			var mode = ConvertWrapFunction(wrapFunc);
			Activate();
			GL.TexParameter(target, TextureParameterName.TextureWrapS, mode);
			GL.TexParameter(target, TextureParameterName.TextureWrapT, mode);
			Deactivate();
		}

		private int ConvertWrapFunction(TextureWrapFunction wrapFunc)
		{
			switch(wrapFunc)
			{
				case TextureWrapFunction.ClampToBorder: return (int)TextureWrapMode.ClampToBorder;
				case TextureWrapFunction.ClampToEdge: return (int)TextureWrapMode.ClampToEdge;
				case TextureWrapFunction.Repeat: return (int)TextureWrapMode.Repeat;
				case TextureWrapFunction.MirroredRepeat: return (int)TextureWrapMode.MirroredRepeat;
				default: throw new ArgumentOutOfRangeException("Unknown wrap function");
			}
		}

		public void FilterLinear()
		{
			Activate();
			GL.TexParameter(target, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
			GL.TexParameter(target, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
			GL.TexParameter(target, TextureParameterName.GenerateMipmap, 0);
			Deactivate();
			filterMode = TextureFilterMode.Linear;
		}

		public void FilterNearest()
		{
			Activate();
			GL.TexParameter(target, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);
			GL.TexParameter(target, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);
			GL.TexParameter(target, TextureParameterName.GenerateMipmap, 0);
			Deactivate();
			filterMode = TextureFilterMode.Nearest;
		}

		public void FilterMipmap()
		{
			Activate();
			GL.TexParameter(target, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
			GL.TexParameter(target, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter(target, TextureParameterName.GenerateMipmap, 1);
			Deactivate();
			filterMode = TextureFilterMode.Mipmap;
		}

		public void Activate()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(target, m_uTextureID);
		}

		public void Deactivate()
		{
			GL.BindTexture(target, 0);
			GL.Disable(EnableCap.Texture2D);
		}

		public TextureFilterMode Filter
		{
			get { return filterMode; }
			set
			{
				switch ((TextureFilterMode)(((int)value) % 3))
				{
					case TextureFilterMode.Nearest: FilterNearest(); break;
					case TextureFilterMode.Linear: FilterLinear(); break;
					case TextureFilterMode.Mipmap: FilterMipmap(); break;
				}
			}
		}
		public void LoadPixels(IntPtr pixels, int width, int height, PixelInternalFormat internalFormat, PixelFormat inputPixelFormat, PixelType type)
		{
			Activate();
			GL.TexImage2D(target, 0, internalFormat, width, height, 0, inputPixelFormat, type, pixels);
			this.Width = width;
			this.Height = height;
			Deactivate();
		}

		public static Texture Create(int width, int height, byte components = 4, bool floatingPoint = false)
		{
			if (components > 4) throw new ArgumentException("Only up to 4 components allowed");
			var internalFormat = PixelInternalFormat.Rgba8;
			var inputPixelFormat = PixelFormat.Rgba;
			var type = PixelType.UnsignedByte;
			if (floatingPoint)
			{
				type = PixelType.Float;
				switch (components)
				{
					case 1: internalFormat = PixelInternalFormat.R32f; inputPixelFormat = PixelFormat.Red; break;
					case 2: internalFormat = PixelInternalFormat.Rg32f; inputPixelFormat = PixelFormat.Rg; break;
					case 3: internalFormat = PixelInternalFormat.Rgb32f; inputPixelFormat = PixelFormat.Rgb; break;
				}
			}
			else
			{
				switch (components)
				{
					case 1: internalFormat = PixelInternalFormat.R8; inputPixelFormat = PixelFormat.Red; break;
					case 2: internalFormat = PixelInternalFormat.Rg8; inputPixelFormat = PixelFormat.Rg; break;
					case 3: internalFormat = PixelInternalFormat.Rgb8; inputPixelFormat = PixelFormat.Rgb; break;
				}
			}
			return Texture.Create(width, height, internalFormat, inputPixelFormat, type);
		}

		public static Texture Create(int width, int height, PixelInternalFormat internalFormat, PixelFormat inputPixelFormat = PixelFormat.Rgba, PixelType type = PixelType.UnsignedByte)
		{
			var texture = new Texture();
			//create empty texture of given size
			texture.LoadPixels(IntPtr.Zero, width, height, internalFormat, inputPixelFormat, type);
			//set default parameters for filtering and clamping
			texture.FilterLinear();
			texture.WrapMode(TextureWrapFunction.Repeat);
			return texture;
		}

		protected override void DisposeResources()
		{
			GL.DeleteTexture(m_uTextureID);
		}

		public int Width { get; private set; }

		public int Height { get; private set; }

		public uint ID { get { return m_uTextureID; } }
	
		private readonly uint m_uTextureID = 0;
		private TextureFilterMode filterMode;
		private readonly TextureTarget target = TextureTarget.Texture2D;
	}
}
