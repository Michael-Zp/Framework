using System.Collections.Generic;
using System.Numerics;

namespace DMS.Geometry
{
	public class Mesh
	{
		public MeshAttribute<Vector3> position = new MeshAttribute<Vector3>(nameof(position));
		public MeshAttribute<Vector3> normal = new MeshAttribute<Vector3>(nameof(normal));
		public MeshAttribute<Vector2> uv = new MeshAttribute<Vector2>(nameof(uv));
		public List<uint> IDs = new List<uint>();
	}
}
