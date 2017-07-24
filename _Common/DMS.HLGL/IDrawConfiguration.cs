using DMS.Geometry;
using System.Numerics;

namespace DMS.HLGL
{
	public interface IDrawConfiguration
	{
		bool BackfaceCulling { get; set; }
		int InstanceCount { get; set; }
		bool ShaderPointSize { get; set; }
		bool ZBufferTest { get; set; }

		void Draw(IRenderContext context);
		void SetInputTexture(string name);
		void SetInputTexture(string name, IRenderSurface image);
		void UpdateInstanceAttribute(string name, float[] data);
		void UpdateInstanceAttribute(string name, int[] data);
		void UpdateInstanceAttribute(string name, Vector2[] data);
		void UpdateInstanceAttribute(string name, Vector3[] data);
		void UpdateInstanceAttribute(string name, Vector4[] data);
		void UpdateMeshShader(Mesh mesh, string shaderName);
		void UpdateShaderBuffer<DATA_ELEMENT_TYPE>(string name, DATA_ELEMENT_TYPE[] uniformArray) where DATA_ELEMENT_TYPE : struct;
		void UpdateUniforms<DATA>(string name, DATA uniforms) where DATA : struct;
	}
}