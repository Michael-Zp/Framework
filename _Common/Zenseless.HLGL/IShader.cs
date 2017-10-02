using System;

namespace Zenseless.HLGL
{
	public enum ShaderType
	{
		FragmentShader, VertexShader, GeometryShader,
		TessEvaluationShader, TessControlShader, ComputeShader
	}

	public enum ShaderResourceType
	{
		Uniform, Attribute, UniformBuffer, RWBuffer
	}

	public interface IShader : IDisposable
	{
		bool IsLinked { get; }
		string LastLog { get; }
		int ProgramID { get; }

		void Activate();
		void Compile(string sShader, ShaderType type);
		void Deactivate();
		int GetResourceLocation(ShaderResourceType resourceType, string name);
		void Link();
	}
}