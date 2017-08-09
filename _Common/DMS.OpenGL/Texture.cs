using DMS.Base;
using DMS.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;

namespace DMS.OpenGL
{
	/// <summary>
	/// Gl Texture class that allows loading from a file.
	/// </summary>
	public class Texture : Disposable, ITexture
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Texture"/> class.
		/// </summary>
		public Texture(TextureTarget target = TextureTarget.Texture2D)
		{
			//generate one texture and put its ID number into the "m_uTextureID" variable
			GL.GenTextures(1, out m_uTextureID);
			//GL.CreateTextures(target, 1, out m_uTextureID); //DSA not supported by intel
			this.Target = target;
		}

		public TextureTarget Target { get; }
		public int Width { get; private set; } = 0;
		public int Height { get; private set; } = 0;
		public uint ID { get { return m_uTextureID; } }

		public TextureFilterMode Filter
		{
			get => filterMode;
			set => SetFilter(value);
		}

		public TextureWrapFunction WrapFunction
		{
			get => wrapFunction;
			set => SetWrapMode(value);
		}

		public void Activate()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(Target, m_uTextureID);
		}

		public void Deactivate()
		{
			GL.BindTexture(Target, 0);
			GL.Disable(EnableCap.Texture2D);
		}

		public void LoadPixels(IntPtr pixels, int width, int height, PixelInternalFormat internalFormat, PixelFormat inputPixelFormat, PixelType type)
		{
			Activate();
			GL.TexImage2D(Target, 0, internalFormat, width, height, 0, inputPixelFormat, type, pixels);
			this.Width = width;
			this.Height = height;
			Deactivate();
		}

		public void LoadPixels(IntPtr pixels, int width, int height, byte components = 4, bool floatingPoint = false)
		{
			var internalFormat = Convert(components, floatingPoint);
			var inputPixelFormat = Convert(components);
			var type = floatingPoint ? PixelType.UnsignedByte : PixelType.Float;
			Activate();
			GL.TexImage2D(Target, 0, internalFormat, width, height, 0, inputPixelFormat, type, pixels);
			this.Width = width;
			this.Height = height;
			Deactivate();
		}

		public static PixelInternalFormat Convert(byte components = 4, bool floatingPoint = false)
		{
			switch (components)
			{
				case 1: return floatingPoint ? PixelInternalFormat.R32f : PixelInternalFormat.R8;
				case 2: return floatingPoint ? PixelInternalFormat.Rg32f : PixelInternalFormat.Rg8;
				case 3: return floatingPoint ? PixelInternalFormat.Rgb32f : PixelInternalFormat.Rgb8;
				case 4: return floatingPoint ? PixelInternalFormat.Rgba32f : PixelInternalFormat.Rgba8;
			}
			throw new ArgumentOutOfRangeException("Invalid Format only 1-4 components allowed");
		}

		public static PixelFormat Convert(byte components = 4)
		{
			switch (components)
			{
				case 1: return PixelFormat.Red;
				case 2: return PixelFormat.Rg;
				case 3: return PixelFormat.Rgb;
				case 4: return PixelFormat.Rgba;
			}
			throw new ArgumentOutOfRangeException("Invalid Format only 1-4 components allowed");
		}

		public static Texture2D Create(int width, int height, byte components = 4, bool floatingPoint = false)
		{
			var internalFormat = Convert(components, floatingPoint);
			var inputPixelFormat = Convert(components);
			var type = floatingPoint ? PixelType.UnsignedByte : PixelType.Float;
			return Texture2D.Create(width, height, internalFormat, inputPixelFormat, type);
		}

		public static Texture2D Create(int width, int height, PixelInternalFormat internalFormat, PixelFormat inputPixelFormat = PixelFormat.Rgba, PixelType type = PixelType.UnsignedByte)
		{
			var texture = new Texture2D();
			//create empty texture of given size
			texture.LoadPixels(IntPtr.Zero, width, height, internalFormat, inputPixelFormat, type);
			//set default parameters for filtering and clamping
			texture.Filter = TextureFilterMode.Linear;
			texture.SetWrapMode(TextureWrapFunction.Repeat);
			return texture;
		}

		protected override void DisposeResources()
		{
			GL.DeleteTexture(m_uTextureID);
		}

		private readonly uint m_uTextureID = 0;
		private TextureFilterMode filterMode;
		private TextureWrapFunction wrapFunction;

		private static int ConvertWrapFunction(TextureWrapFunction wrapFunc)
		{
			switch (wrapFunc)
			{
				case TextureWrapFunction.ClampToBorder: return (int)TextureWrapMode.ClampToBorder;
				case TextureWrapFunction.ClampToEdge: return (int)TextureWrapMode.ClampToEdge;
				case TextureWrapFunction.Repeat: return (int)TextureWrapMode.Repeat;
				case TextureWrapFunction.MirroredRepeat: return (int)TextureWrapMode.MirroredRepeat;
				default: throw new ArgumentOutOfRangeException("Unknown wrap function");
			}
		}

		private void SetFilter(TextureFilterMode filter)
		{
			//case TextureFilterMode.Nearest
			var magFilter = (int)TextureMagFilter.Nearest;
			var minFilter = (int)TextureMinFilter.Nearest;
			var mipmap = 0;
			switch (filter)
			{
				case TextureFilterMode.Linear:
					magFilter = (int)TextureMagFilter.Linear;
					minFilter = (int)TextureMinFilter.Linear;
					break;
				case TextureFilterMode.Mipmap:
					magFilter = (int)TextureMagFilter.Linear;
					minFilter = (int)TextureMinFilter.LinearMipmapLinear;
					mipmap = 1;
					break;
			}
			Activate();
			GL.TexParameter(Target, TextureParameterName.TextureMagFilter, magFilter);
			GL.TexParameter(Target, TextureParameterName.TextureMinFilter, minFilter);
			GL.TexParameter(Target, TextureParameterName.GenerateMipmap, mipmap);
			Deactivate();
			filterMode = filter;
		}

		private void SetWrapMode(TextureWrapFunction wrapFunc)
		{
			var mode = ConvertWrapFunction(wrapFunc);
			Activate();
			GL.TexParameter(Target, TextureParameterName.TextureWrapS, mode);
			GL.TexParameter(Target, TextureParameterName.TextureWrapT, mode);
			GL.TexParameter(Target, TextureParameterName.TextureWrapR, mode);
			Deactivate();
			wrapFunction = wrapFunc;
		}
	}
}
