﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// static class that provides geometric shape builder methods
	/// </summary>
	public static class Shapes
	{
		/// <summary>
		/// creates a grid shape made up of pairs of triangles; stored as an indexed vertex array.
		/// </summary>
		/// <param name="startU">start coordinate of the grid in the first coordinate axis</param>
		/// <param name="sizeU">extent of the grid in the first coordinate axis</param>
		/// <param name="startV">start coordinate of the grid in the second coordinate axis</param>
		/// <param name="sizeV">extent of the grid in the second coordinate axis</param>
		/// <param name="segmentsU">number of grid segments in the first coordinate axis</param>
		/// <param name="segmentsV">number of grid segments in the second coordinate axis</param>
		/// <param name="createPosition">callback for each position creation</param>
		/// <param name="createID">callback for each index creation</param>
		/// <param name="createNormal">callback for each vertex normal creation</param>
		/// <param name="createUV">callback for each vertex texture coordinate creation</param>
		public static void CreateGrid(float startU, float sizeU, float startV, float sizeV
			, uint segmentsU, uint segmentsV
			, Action<float, float> createPosition, Action<uint> createID
			, Action createNormal = null, Action<float, float> createUV = null)
		{
			if (ReferenceEquals(null, createPosition)) throw new ArgumentNullException(nameof(createPosition) + " must not be null");
			if (ReferenceEquals(null, createID)) throw new ArgumentNullException(nameof(createID) + " must not be null");
			float deltaU = (1.0f / segmentsU) * sizeU;
			float deltaV = (1.0f / segmentsV) * sizeV;
			//create vertex data
			for (uint u = 0; u < segmentsU + 1; ++u)
			{
				for (uint v = 0; v < segmentsV + 1; ++v)
				{
					float pU = startU + u * deltaU;
					float pV = startV + v * deltaV;
					createPosition(pU, pV);
					createNormal?.Invoke();
					createUV?.Invoke(u, v);
				}
			}
			uint verticesZ = segmentsV + 1;
			//create ids
			for (uint u = 0; u < segmentsU; ++u)
			{
				for (uint v = 0; v < segmentsV; ++v)
				{
					createID(u * verticesZ + v);
					createID(u * verticesZ + v + 1);
					createID((u + 1) * verticesZ + v);

					createID((u + 1) * verticesZ + v);
					createID(u * verticesZ + v + 1);
					createID((u + 1) * verticesZ + v + 1);
				}
			}
		}

		/// <summary>
		/// Creates a sphere made up of triangles
		/// </summary>
		/// <param name="createPosition">callback for each position creation</param>
		/// <param name="createID">callback for each index creation</param>
		/// <param name="radius_">radius of the sphere</param>
		/// <param name="subdivision">subdivision count, each subdivision creates 4 times more faces</param>
		/// <param name="createNormal">callback for each vertex normal creation</param>
		public static void CreateSphere(Action<float, float, float> createPosition, Action<uint> createID, float radius_ = 1.0f, uint subdivision = 1
			, Action<float, float, float> createNormal = null)
		{
			if (ReferenceEquals(null, createPosition)) throw new ArgumentNullException(nameof(createPosition) + " must not be null");
			if (ReferenceEquals(null, createID)) throw new ArgumentNullException(nameof(createID) + " must not be null");
			//idea: subdivide icosahedron
			const float X = 0.525731112119133606f;
			const float Z = 0.850650808352039932f;

			var vdata = new float[12, 3] {
				{ -X, 0.0f, Z}, { X, 0.0f, Z}, { -X, 0.0f, -Z }, { X, 0.0f, -Z },
				{ 0.0f, Z, X }, { 0.0f, Z, -X }, { 0.0f, -Z, X }, { 0.0f, -Z, -X },
				{ Z, X, 0.0f }, { -Z, X, 0.0f }, { Z, -X, 0.0f }, { -Z, -X, 0.0f }
			};
			var tindices = new uint[20, 3] {
				{ 0, 1, 4 }, { 0, 4, 9 }, { 9, 4, 5 }, { 4, 8, 5 }, { 4, 1, 8 },
				{ 8, 1, 10 }, { 8, 10, 3 }, { 5, 8, 3 }, { 5, 3, 2 }, { 2, 3, 7 },
				{ 7, 3, 10 }, { 7, 10, 6 }, { 7, 6, 11 }, { 11, 6, 0 }, { 0, 6, 1 },
				{ 6, 10, 1 }, { 9, 11, 0 }, { 9, 2, 11 }, { 9, 5, 2 }, { 7, 11, 2 } };

			List<Vector3> uniformPositions = new List<Vector3>();
			for (int i = 0; i < 12; ++i)
			{
				uniformPositions.Add(new Vector3(vdata[i, 0], vdata[i, 1], vdata[i, 2]));
				createNormal?.Invoke(vdata[i, 0], vdata[i, 1], vdata[i, 2]);
			}
			for (int i = 0; i < 20; ++i)
			{
				Subdivide(uniformPositions, createID, tindices[i, 0], tindices[i, 1], tindices[i, 2], subdivision, createNormal);
			}

			//scale
			foreach(var pos in uniformPositions)
			{
				var p = pos * radius_;
				createPosition(p.X, p.Y, p.Z);
			}
		}

		private static uint CreateID(List<Vector3> positions, Action<uint> createID, uint id1, uint id2
			, Action<float, float, float> createNormal = null)
		{
			//todo: could detect if edge already calculated and reuse....
			uint i12 = (uint)positions.Count;
			Vector3 v12 = positions[(int)id1] + positions[(int)id2];
			v12 /= v12.Length();
			createNormal?.Invoke(v12.X, v12.Y, v12.Z);
			positions.Add(v12);
			return i12;
		}

		private static void Subdivide(List<Vector3> positions, Action<uint> createID, uint id1, uint id2, uint id3, uint depth
			, Action<float, float, float> createNormal = null)
		{
			if (0 == depth)
			{
				createID(id1);
				createID(id2);
				createID(id3);
				return;
			}

			uint i12 = CreateID(positions, createID, id1, id2, createNormal);
			uint i23 = CreateID(positions, createID, id2, id3, createNormal);
			uint i31 = CreateID(positions, createID, id3, id1, createNormal);

			Subdivide(positions, createID, id1, i12, i31, depth - 1, createNormal);
			Subdivide(positions, createID, id2, i23, i12, depth - 1, createNormal);
			Subdivide(positions, createID, id3, i31, i23, depth - 1, createNormal);
			Subdivide(positions, createID, i12, i23, i31, depth - 1, createNormal);
		}
	}
}
