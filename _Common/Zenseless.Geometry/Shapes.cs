using System;

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
		/// <param name="createVertex">callback for each vertex creation</param>
		/// <param name="createID">callback for each index creation</param>
		/// <param name="createNormal">callback for each vertex normal creation</param>
		/// <param name="createUV">callback for each vertex texture coordinate creation</param>
		public static void CreateGrid(float startU, float sizeU, float startV, float sizeV
			, uint segmentsU, uint segmentsV
			, Action<float, float> createVertex, Action<uint> createID
			, Action createNormal = null, Action<float, float> createUV = null)
		{
			if (ReferenceEquals(null, createVertex)) throw new ArgumentNullException(nameof(createVertex) + " must not be null");
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
					createVertex(pU, pV);
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
	}
}
