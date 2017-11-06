using System;
using System.Collections.Generic;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// A Mesh is a collection of attributes, like positions, normals and texture coordinates
	/// </summary>
	public class Mesh
	{
		public static readonly string PositionName = "position";
		public static readonly string NormalName = "normal";
		public static readonly string TexCoordName = "uv";

		public IMeshAttribute<Vector3> Position { get; private set; } = new MeshAttribute<Vector3>(PositionName); //=> Get<Vector3>(PositionName);//
		public IMeshAttribute<Vector3> Normal { get; private set; } = new MeshAttribute<Vector3>(NormalName);
		public IMeshAttribute<Vector2> Uv { get; private set; } = new MeshAttribute<Vector2>(TexCoordName);
		public List<uint> IDs { get; private set; } = new List<uint>();

		public List<ELEMENT_TYPE> AddAttribute<ELEMENT_TYPE>(string name)
		{
			if (Contains(name)) throw new ArgumentException($"Attribute '{name}' already exists");
			var attribute = new List<ELEMENT_TYPE>();
			attributes.Add(name, new List<ELEMENT_TYPE>());
			return attribute;
		}
		public bool Contains(string name) => attributes.ContainsKey(name);

		public List<ELEMENT_TYPE> Get<ELEMENT_TYPE>(string name)
		{
			if (attributes.TryGetValue(name, out object data))
			{
				var typedData = data as List<ELEMENT_TYPE>;
				if(ReferenceEquals(null, typedData))
				{
					throw new InvalidCastException($"Attribute '{name}' has type {data.GetType().FullName}.");
				}
				return typedData;
			}
			throw new KeyNotFoundException($"No attribute with name '{name}' stored.");
		}

		private Dictionary<string, object> attributes = new Dictionary<string, object>();
	}
}
