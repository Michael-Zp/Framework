using System.Collections.Generic;

namespace Zenseless.Geometry
{
	public class Obj2Mesh
	{
		private class VertexComparer : IEqualityComparer<ObjParser.Vertex>
		{
			public bool Equals(ObjParser.Vertex a, ObjParser.Vertex b)
			{
				return (a.idNormal == b.idNormal) && (a.idPos == b.idPos) && (a.idTexCoord == b.idTexCoord);
			}

			public int GetHashCode(ObjParser.Vertex obj)
			{
				return obj.idPos;
			}
		}

		public static DefaultMesh FromObj(byte[] objByteData)
		{
			var parser = new ObjParser(objByteData);
			var uniqueVertexIDs = new Dictionary<ObjParser.Vertex, uint>(new VertexComparer());

			var mesh = new DefaultMesh();

			foreach (var face in parser.faces)
			{
				//only accept triangles
				if (3 != face.Count) continue;
				foreach (var vertex in face)
				{
					if (uniqueVertexIDs.TryGetValue(vertex, out uint index))
					{
						mesh.IDs.Add(index);
					}
					else
					{
						uint id = (uint)mesh.Position.Count;
						//add vertex data to mesh
						mesh.Position.Add(parser.position[vertex.idPos]);
						if (-1 != vertex.idNormal) mesh.Normal.Add(parser.normals[vertex.idNormal]);
						if (-1 != vertex.idTexCoord) mesh.TexCoord.Add(parser.texCoords[vertex.idTexCoord]);
						mesh.IDs.Add(id);
						//new id
						uniqueVertexIDs[vertex] = id;
					}
				}
			}
			return mesh;
		}
	}
}
