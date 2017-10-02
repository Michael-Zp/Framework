using System;

namespace Zenseless.Geometry
{
	public static class Shapes
	{
		public static void CreatePlane(float startU, float sizeU, float startV, float sizeV
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
