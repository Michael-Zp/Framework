using System.Collections.Generic;
using System.Numerics;

namespace Zenseless.Geometry
{
	public class DefaultMesh : Mesh
	{
		public List<Vector3> Position => position;
		public List<Vector3> Normal => normal;
		public List<Vector2> TexCoord => texCoord;

		public DefaultMesh()
		{
			position = AddAttribute<Vector3>(PositionName);
			normal = AddAttribute<Vector3>(NormalName);
			texCoord = AddAttribute<Vector2>(TexCoordName);
		}

		private List<Vector3> position;
		private List<Vector3> normal;
		private List<Vector2> texCoord;
	}
}
