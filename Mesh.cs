using OpenTK;
using System.Collections.Generic;

namespace Framework
{
	public class Mesh
	{
		public List<Vector3> positions = new List<Vector3>();
		public List<Vector3> normals = new List<Vector3>();
		public List<Vector2> uvs = new List<Vector2>();
		public List<uint> ids = new List<uint>();

		public Mesh SwitchTriangleMeshHandedness()
		{
			var mesh = new Mesh();
			foreach(var pos in positions)
			{
				var newPos = pos;
				newPos.Z = -newPos.Z;
				mesh.positions.Add(newPos);
			}
			foreach (var n in normals)
			{
				var newN = n;
				newN.Z = -newN.Z;
				mesh.normals.Add(newN);
			}
			foreach (var uv in uvs)
			{
				mesh.uvs.Add(uv);
			}
			for(int i = 0; i < ids.Count; i += 3)
			{
				mesh.ids.Add(ids[i]);
				mesh.ids.Add(ids[i + 2]);
				mesh.ids.Add(ids[i + 1]);
			}
			return mesh;
		}
	};
}
