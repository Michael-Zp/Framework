using System.Collections.Generic;
using System.Numerics;

namespace Zenseless.Geometry
{
	public class Mesh
	{
		public IMeshAttribute<Vector3> Position { get; private set; } = new MeshAttribute<Vector3>(nameof(Position).ToLowerInvariant());
		public IMeshAttribute<Vector3> Normal { get; private set; } = new MeshAttribute<Vector3>(nameof(Normal).ToLowerInvariant());
		public IMeshAttribute<Vector2> Uv { get; private set; } = new MeshAttribute<Vector2>(nameof(Uv).ToLowerInvariant());
		public List<uint> IDs { get; private set; } = new List<uint>();

		//private Dictionary<string, List<object>> attributes = new Dictionary<string, List<object>>();
	}
}
