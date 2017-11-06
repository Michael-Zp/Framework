using System;
using System.Numerics;

namespace Zenseless.Geometry
{
	public static partial class Meshes
	{
		public static void SetConstantUV(this Mesh mesh, Vector2 uv)
		{
			var uvs = mesh.Uv.List;
			var pos = mesh.Position.List;
			uvs.Capacity = pos.Count;
			//overwrite existing
			for(int i = 0; i < uvs.Count; ++i)
			{
				uvs[i] = uv;
			}
			//add
			for(int i = uvs.Count; i < pos.Count; ++i)
			{
				uvs.Add(uv);
			}
		}

		public static Mesh Clone(this Mesh m)
		{
			var mesh = new Mesh();
			mesh.Position.List.AddRange(m.Position.List);
			mesh.Normal.List.AddRange(m.Normal.List);
			mesh.Uv.List.AddRange(m.Uv.List);
			mesh.IDs.AddRange(m.IDs);
			return mesh;
		}

		public static void Add(this Mesh a, Mesh b)
		{
			var aPos = a.Position.List;
			var count = (uint)aPos.Count;
			aPos.AddRange(b.Position.List);
			if(b.Normal.List.Count > 0)
			{
				if (a.Normal.List.Count != count) throw new ArgumentException("Original mesh has no normals, but added mesh has normals");
				a.Normal.List.AddRange(b.Normal.List);
			}
			if (b.Uv.List.Count > 0)
			{
				if (a.Uv.List.Count != count) throw new ArgumentException("Original mesh has no uvs, but added mesh has uvs");
				a.Uv.List.AddRange(b.Uv.List);
			}
			foreach(var id in b.IDs)
			{
				a.IDs.Add(id + count);
			}
		}

		public static Mesh Transform(this Mesh m, Matrix4x4 transform)
		{
			var mesh = new Mesh();
			mesh.Uv.List.AddRange(m.Uv.List);
			mesh.IDs.AddRange(m.IDs);
			foreach (var pos in m.Position.List)
			{
				var newPos = Vector3.Transform(pos, transform);
				mesh.Position.List.Add(newPos);
			}
			foreach (var n in m.Normal.List)
			{
				var newN = Vector3.Normalize(Vector3.TransformNormal(n, transform));
				mesh.Normal.List.Add(newN);
			}
			return mesh;
		}

		public static Mesh SwitchHandedness(this Mesh m)
		{
			var mesh = new Mesh();
			mesh.Uv.List.AddRange(m.Uv.List);
			mesh.IDs.AddRange(m.IDs);
			foreach (var pos in m.Position.List)
			{
				var newPos = pos;
				newPos.Z = -newPos.Z;
				mesh.Position.List.Add(newPos);
			}
			foreach (var n in m.Normal.List)
			{
				var newN = n;
				newN.Z = -newN.Z;
				mesh.Normal.List.Add(newN);
			}
			return mesh;
		}

		public static Mesh FlipNormals(this Mesh m)
		{
			var mesh = new Mesh();
			mesh.Position.List.AddRange(m.Position.List);
			mesh.Uv.List.AddRange(m.Uv.List);
			mesh.IDs.AddRange(m.IDs);
			foreach (var n in m.Normal.List)
			{
				var newN = -n;
				mesh.Normal.List.Add(newN);
			}
			return mesh;
		}

		public static Mesh SwitchTriangleMeshWinding(this Mesh m)
		{
			var mesh = new Mesh();
			mesh.Position.List.AddRange(m.Position.List);
			mesh.Normal.List.AddRange(m.Normal.List);
			mesh.Uv.List.AddRange(m.Uv.List);
			for (int i = 0; i < m.IDs.Count; i += 3)
			{
				mesh.IDs.Add(m.IDs[i]);
				mesh.IDs.Add(m.IDs[i + 2]);
				mesh.IDs.Add(m.IDs[i + 1]);
			}
			return mesh;
		}

		public static Mesh CreateCornellBox(float roomSize = 2, float sphereRadius = 0.3f, float cubeSize = 0.6f)
		{
			Mesh mesh = new Mesh();
			var plane = Meshes.CreatePlane(roomSize, roomSize, 2, 2);
			
			var xform = new Transformation();
			xform.TranslateGlobal(0, -roomSize / 2, 0);
			plane.SetConstantUV(new Vector2(3, 0));
			mesh.Add(plane.Transform(xform));
			xform.RotateZGlobal(90f);
			plane.SetConstantUV(new Vector2(1, 0));
			mesh.Add(plane.Transform(xform));
			xform.RotateZGlobal(90f);
			plane.SetConstantUV(new Vector2(0, 0));
			mesh.Add(plane.Transform(xform));
			xform.RotateZGlobal(90f);
			plane.SetConstantUV(new Vector2(2, 0));
			mesh.Add(plane.Transform(xform));
			xform.RotateYGlobal(270f);
			plane.SetConstantUV(new Vector2(0, 0));
			mesh.Add(plane.Transform(xform));

			var sphere = Meshes.CreateSphere(sphereRadius, 4);
			sphere.SetConstantUV(new Vector2(3, 0));
			xform.Reset();
			xform.TranslateGlobal(0.4f, -1 + sphereRadius, -0.2f);
			mesh.Add(sphere.Transform(xform));

			var cube = Meshes.CreateCubeWithNormals(cubeSize);
			cube.SetConstantUV(new Vector2(3, 0));
			xform.Reset();
			xform.RotateYGlobal(35f);
			xform.TranslateGlobal(-0.5f, -1 + 0.5f * cubeSize, 0.1f);
			mesh.Add(cube.Transform(xform));
			return mesh;
		}
		public struct CornellBoxMaterial //use 16 byte alignment or you have to query all variable offsets
		{
			public Vector3 color;
			public float shininess;
		};
		public static CornellBoxMaterial[] CreateCornellBoxMaterial()
		{
			var materials = new CornellBoxMaterial[4];
			materials[0].color = new Vector3(1, 1, 1);
			materials[0].shininess = 0;
			materials[1].color = new Vector3(0, 1, 0);
			materials[1].shininess = 0;
			materials[2].color = new Vector3(1, 0, 0);
			materials[2].shininess = 0;
			materials[3].color = new Vector3(1, 1, 1);
			materials[3].shininess = 256;
			return materials;
		}

		/// <summary>
		/// Creates a cube made up of pairs of triangles; stored as an indexed vertex array
		/// </summary>
		/// <param name="size">length of one side</param>
		/// <returns>Mesh with positions, ids, normals</returns>
		public static Mesh CreateCubeWithNormals(float size = 1.0f)
		{
			Mesh m = new Mesh();
			void createPosition(float x, float y, float z) => m.Position.List.Add(new Vector3(x, y, z));
			void createID(uint index) => m.IDs.Add(index);
			void createNormal(float x, float y, float z) => m.Normal.List.Add(new Vector3(x, y, z));
			ShapeBuilder.Cube(createPosition, createID, size, createNormal);
			return m;
		}

		/// <summary>
		/// creates a sphere made up of pairs of triangles; stored as an indexed vertex array
		/// </summary>
		/// <param name="radius">radius</param>
		/// <param name="subdivision">subdivision count, each subdivision creates 4 times more faces</param>
		/// <returns>Mesh with positions, ids, normals</returns>
		public static Mesh CreateSphere(float radius = 1.0f, uint subdivision = 1)
		{
			Mesh m = new Mesh();
			//var pos = m.AddAttribute<Vector3>(Mesh.PositionName);
			//var normal = m.AddAttribute<Vector3>(Mesh.NormalName);
			void createPosition(float x, float y, float z) => m.Position.List.Add(new Vector3(x, y, z));
			void createID(uint id) => m.IDs.Add(id);
			void createNormal(float x, float y, float z) => m.Normal.List.Add(new Vector3(x, y, z));
			ShapeBuilder.Sphere(createPosition, createID, radius, subdivision, createNormal);
			return m;
		}

		/// <summary>
		/// creates an icosahedron made up of pairs of triangles; stored as an indexed vertex array
		/// </summary>
		/// <param name="radius">radius</param>
		/// <returns>Mesh with positions, ids, normals</returns>
		public static Mesh CreateIcosahedron(float radius)
		{
			return CreateSphere(radius, 0);
		}

		/// <summary>
		/// Creates a plane made up of pairs of triangles; stored as an indexed vertex array.
		/// </summary>
		/// <param name="sizeX">extent of the grid in the x-coordinate axis</param>
		/// <param name="sizeZ">extent of the grid in the z-coordinate axis</param>
		/// <param name="segmentsX">number of grid segments in the x-coordinate axis</param>
		/// <param name="segmentsZ">number of grid segments in the z-coordinate axis</param>
		/// <returns>Mesh with positions, ids, normals, and uvs</returns>
		public static Mesh CreatePlane(float sizeX, float sizeZ, uint segmentsX, uint segmentsZ)
		{
			Mesh m = new Mesh();
			void CreateVertex(float x, float z) => m.Position.List.Add(new Vector3(x, 0.0f, z));
			void CreateID(uint id) => m.IDs.Add(id);
			void CreateNormal() => m.Normal.List.Add(Vector3.UnitY);
			void CreateUV(float u, float v) => m.Uv.List.Add(new Vector2(u, v));

			var startX = -sizeX / 2f;
			var startZ= -sizeZ / 2f;
			ShapeBuilder.Grid(startX, sizeX, startZ, sizeZ, segmentsX, segmentsZ, CreateVertex, CreateID
				, CreateNormal, CreateUV);
			return m;
		}
	}
}
