using DMS.Geometry;
using DMS.OpenGL;

namespace DMS.HLGL
{
	public class Geometry
	{
		public Geometry(VAO vao) //todo: give mesh
		{
			Vao = vao;
		}
		public VAO Vao { get; private set; }
		//todo parameters: primitive type, instanceCount
	}
}