using System;
using OpenTK.Graphics.OpenGL;

namespace Framework
{
	public class ShaderException : Exception
	{
		public string Type { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderException"/> class.
		/// </summary>
		/// <param name="msg">The error msg.</param>
		public ShaderException(string type, string msg) : base(msg)
		{
			Type = type;
		}
	}

	/// <summary>
	/// Shader class
	/// </summary>
	public class Shader : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Shader"/> class.
		/// </summary>
		public Shader()
		{
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (IsLoaded())
			{
				GL.DeleteProgram(m_ProgramID);
			}
		}

		/// <summary>
		/// Loads vertex and fragment shaders from strings.
		/// </summary>
		/// <param name="sVertexShd_">The s vertex SHD_.</param>
		/// <param name="sFragmentShd_">The s fragment SHD_.</param>
		/// <returns>a new instance</returns>
		public static Shader LoadFromStrings(string sVertexShd_, string sFragmentShd_)
		{
			Shader shd = new Shader();
			shd.m_ProgramID = CompileLink(sVertexShd_, sFragmentShd_);
			if (!shd.IsLoaded())
			{
				shd = null;
			}
			return shd;
		}

		/// <summary>
		/// Begins this shader use.
		/// </summary>
		public void Begin()
		{
			if(IsLoaded()) GL.UseProgram(m_ProgramID);
		}

		/// <summary>
		/// Ends this shader use.
		/// </summary>
		public void End()
		{
			if (IsLoaded()) GL.UseProgram(0);
		}

		/// <summary>
		/// Determines whether this shader is loaded.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this shader is loaded; otherwise, <c>false</c>.
		/// </returns>
		public bool IsLoaded() { return 0 != m_ProgramID; }

		public int GetUniformLocation(string name)
		{
			return GL.GetUniformLocation(m_ProgramID, name);
		}

		private int m_ProgramID = 0;

		private static string CorrectLineEndings(string input)
		{
			return input.Replace("\n", Environment.NewLine);
		}


		private static int Compile(string sShader, ShaderType type)
		{
			int shaderObject = 0;
			int status_code;
			if (!string.IsNullOrEmpty(sShader))
			{
				shaderObject = GL.CreateShader(type);
				// Compile vertex shader
				GL.ShaderSource(shaderObject, sShader);
				GL.CompileShader(shaderObject);
				GL.GetShader(shaderObject, ShaderParameter.CompileStatus, out status_code);
				if (1 != status_code)
				{
					string log = CorrectLineEndings(GL.GetShaderInfoLog(shaderObject));
					throw new ShaderException(type.ToString(), log);
				}
			}
			return shaderObject;
		}

		private static int CompileLink(string sVertexShd_, string sFragmentShd_)
		{
			int program = 0;
			int vertexObject = Compile(sVertexShd_, ShaderType.VertexShader);
			int fragmentObject = Compile(sFragmentShd_, ShaderType.FragmentShader); ;
			int status_code;

			program = GL.CreateProgram();
			if (0 != vertexObject)
			{
				GL.AttachShader(program, vertexObject);
			}
			if (0 != fragmentObject)
			{
				GL.AttachShader(program, fragmentObject);
			}
			try
			{
				GL.LinkProgram(program);
			}
			catch (Exception)
			{
				throw new ShaderException("Link", "Unknown error!");
			}
			GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status_code);
			if (1 != status_code)
			{
				string log = CorrectLineEndings(GL.GetProgramInfoLog(program));
				GL.DeleteProgram(program);
				throw new ShaderException("Link", log);
			}
			GL.UseProgram(0);
			return program;
		}
	}
}
