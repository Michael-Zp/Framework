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

		public Mesh SwitchHandedness()
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
			mesh.uvs.AddRange(uvs);
			mesh.ids.AddRange(ids);
			return mesh;
		}
		public Mesh SwitchTriangleMeshWinding()
		{
			var mesh = new Mesh();
			mesh.positions.AddRange(positions);
			mesh.normals.AddRange(normals);
			mesh.uvs.AddRange(uvs);
			for (int i = 0; i < ids.Count; i += 3)
			{
				mesh.ids.Add(ids[i]);
				mesh.ids.Add(ids[i + 2]);
				mesh.ids.Add(ids[i + 1]);
			}
			return mesh;
		}
	};
}
