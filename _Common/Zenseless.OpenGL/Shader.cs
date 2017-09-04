using System;
using OpenTK.Graphics.OpenGL4;
using Zenseless.Base;
using System.Collections.Generic;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	using TKShaderType = OpenTK.Graphics.OpenGL4.ShaderType;
	using ShaderType = HLGL.ShaderType;

	/// <summary>
	/// Shader class
	/// </summary>
	/// todo: rename to ShaderProgram and create Shader classes to compile individual (fragment, vertex, ...) shaders
	public class Shader : Disposable, IShader
	{
		public bool IsLinked { get; private set; } = false;

		public string LastLog { get; private set; }

		public int ProgramID { get; private set; } = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="Shader"/> class.
		/// </summary>
		public Shader()
		{
			ProgramID = GL.CreateProgram();
		}

		public void Compile(string sShader, ShaderType type)
		{
			IsLinked = false;
			int shaderObject = GL.CreateShader(ConvertType(type));
			if (0 == shaderObject) throw new ShaderCompileException(type, "Could not create " + type.ToString() + " object", string.Empty, sShader);
			// Compile vertex shader
			GL.ShaderSource(shaderObject, sShader);
			GL.CompileShader(shaderObject);
			GL.GetShader(shaderObject, ShaderParameter.CompileStatus, out int status_code);
			LastLog = GL.GetShaderInfoLog(shaderObject);
			if (1 != status_code)
			{
				GL.DeleteShader(shaderObject);
				throw new ShaderCompileException(type, "Error compiling  " + type.ToString(), LastLog, sShader);
			}
			GL.AttachShader(ProgramID, shaderObject);
			shaderIDs.Add(shaderObject);
		}

		/// <summary>
		/// Begins this shader use.
		/// </summary>
		public void Activate()
		{
			GL.UseProgram(ProgramID);
		}

		/// <summary>
		/// Ends this shader use.
		/// </summary>
		public void Deactivate()
		{
			GL.UseProgram(0);
		}

		public int GetResourceLocation(ShaderResourceType resourceType, string name)
		{
			switch(resourceType)
			{
				case ShaderResourceType.Attribute: return GL.GetAttribLocation(ProgramID, name);
				case ShaderResourceType.UniformBuffer: return GetResourceIndex(name, ProgramInterface.UniformBlock);
				case ShaderResourceType.RWBuffer: return GetResourceIndex(name, ProgramInterface.ShaderStorageBlock);
				case ShaderResourceType.Uniform: return GetResourceIndex(name, ProgramInterface.Uniform);
				default: throw new ArgumentOutOfRangeException("Unknown ShaderResourceType");
			}
		}

		public void Link()
		{
			try
			{
				GL.LinkProgram(ProgramID);
			}
			catch (Exception)
			{
				throw new ShaderException("Unknown Link error!", string.Empty);
			}
			GL.GetProgram(ProgramID, GetProgramParameterName.LinkStatus, out int status_code);
			if (1 != status_code)
			{
				throw new ShaderException("Error linking shader", GL.GetProgramInfoLog(ProgramID));
			}
			IsLinked = true;
			RemoveShaders();
		}

		protected override void DisposeResources()
		{
			if (0 != ProgramID)
			{
				GL.DeleteProgram(ProgramID);
			}
		}

		private List<int> shaderIDs = new List<int>();

		private TKShaderType ConvertType(ShaderType type)
		{
			switch(type)
			{
				case ShaderType.ComputeShader: return TKShaderType.ComputeShader;
				case ShaderType.FragmentShader: return TKShaderType.FragmentShader;
				case ShaderType.GeometryShader: return TKShaderType.GeometryShader;
				case ShaderType.TessControlShader: return TKShaderType.TessControlShader;
				case ShaderType.TessEvaluationShader: return TKShaderType.TessEvaluationShader;
				case ShaderType.VertexShader: return TKShaderType.VertexShader;
				default: throw new ArgumentOutOfRangeException("Unknown Shader type");
			}
		}

		private int GetResourceIndex(string name, ProgramInterface type)
		{
			return GL.GetProgramResourceIndex(ProgramID, type, name);
		}

		private void RemoveShaders()
		{
			foreach (int id in shaderIDs)
			{
				GL.DetachShader(ProgramID, id);
				GL.DeleteShader(id);
			}
			shaderIDs.Clear();
		}
	}
}
