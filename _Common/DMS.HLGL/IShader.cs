namespace DMS.HLGL
{
	public enum ShaderType
	{
		FragmentShader, VertexShader, GeometryShader,
		TessEvaluationShader, TessControlShader, ComputeShader
	}

	public interface IShader
	{
		bool IsLinked { get; }
		string LastLog { get; }
		int ProgramID { get; }

		void Activate();
		void Compile(string sShader, ShaderType type);
		void Deactivate();
		int GetAttributeLocation(string name);
		int GetShaderStorageBufferBindingIndex(string name);
		int GetUniformBufferBindingIndex(string name);
		int GetUniformLocation(string name);
		void Link();
	}
}