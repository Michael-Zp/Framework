using OpenTK.Graphics.OpenGL;
using System;

namespace Framework
{
	/// <summary>
	/// Gl Texture class that allows loading from a file.
	/// </summary>
	public class Texture : IDisposable
	{
		public enum FilterMode { NEAREST, BILINEAR, TRILINEAR };

		/// <summary>
		/// Initializes a new instance of the <see cref="Texture"/> class.
		/// </summary>
		public Texture()
		{
			//generate one texture and put its ID number into the "m_uTextureID" variable
			GL.GenTextures(1, out m_uTextureID);
			Width = 0;
			Height = 0;
		}

		public void WrapMode(TextureWrapMode mode)
		{
			BeginUse();
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)mode);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)mode);
			EndUse();
		}

		public void Dispose()
		{
			GL.DeleteTexture(m_uTextureID);
		}

		public void FilterBilinear()
		{
			BeginUse();
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 0);
			EndUse();
			filterMode = FilterMode.BILINEAR;
		}

		public void FilterNearest()
		{
			BeginUse();
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 0);
			EndUse();
			filterMode = FilterMode.NEAREST;
		}

		public void FilterTrilinear()
		{
			BeginUse();
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);
			EndUse();
			filterMode = FilterMode.TRILINEAR;
		}

		public void BeginUse()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, m_uTextureID);
		}

		public void EndUse()
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);
			GL.Disable(EnableCap.Texture2D);
		}

		public FilterMode Filter
		{
			get { return filterMode; }
			set
			{
				switch (value)
				{
					case FilterMode.NEAREST: FilterNearest(); break;
					case FilterMode.BILINEAR: FilterBilinear(); break;
					case FilterMode.TRILINEAR: FilterTrilinear(); break;
				}
				filterMode = value;
			}
		}
		public void LoadPixels(IntPtr pixels, int width, int height, PixelInternalFormat internalFormat, PixelFormat inputPixelFormat, PixelType type)
		{
			BeginUse();
			GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0,	inputPixelFormat, type, pixels);
			this.Width = width;
			this.Height = height;
			EndUse();
		}

		public static Texture Create(int width, int height, PixelInternalFormat internalFormat = PixelInternalFormat.Rgba8
			, PixelFormat inputPixelFormat = PixelFormat.Rgba, PixelType type = PixelType.UnsignedByte)
		{
			var texture = new Texture();
			//create empty texture of given size
			texture.LoadPixels(IntPtr.Zero, width, height, internalFormat, inputPixelFormat, type);
			//set default parameters for filtering and clamping
			texture.FilterBilinear();
			texture.WrapMode(TextureWrapMode.Repeat);
			return texture;
		}

		public int Width { get; private set; }

		public int Height { get; private set; }

		public uint ID { get { return m_uTextureID; } }
	
		private readonly uint m_uTextureID = 0;
		private FilterMode filterMode;
	}
}
