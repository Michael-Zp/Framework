﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace Zenseless.Geometry
{
	/// <summary>
	/// A Mesh is a collection of attributes, like positions, normals and texture coordinates
	/// </summary>
	public class Mesh
	{
		/// <summary>
		/// Gets the i ds.
		/// </summary>
		/// <value>
		/// The i ds.
		/// </value>
		public List<uint> IDs { get; private set; } = new List<uint>();

		/// <summary>
		/// Adds the attribute.
		/// </summary>
		/// <typeparam name="ELEMENT_TYPE">The type of the element.</typeparam>
		/// <param name="name">The attribute name.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public List<ELEMENT_TYPE> AddAttribute<ELEMENT_TYPE>(string name)
		{
			if (Contains(name)) throw new ArgumentException($"Attribute '{name}' already exists");
			var attribute = new List<ELEMENT_TYPE>();
			attributes.Add(name, new List<ELEMENT_TYPE>());
			return attribute;
		}

		/// <summary>
		/// Determines whether [contains] [the specified name].
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		///   <c>true</c> if [contains] [the specified name]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(string name) => attributes.ContainsKey(name);

		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <typeparam name="ELEMENT_TYPE">The type of the lement type.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		/// <exception cref="KeyNotFoundException"></exception>
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

		/// <summary>
		/// The attributes
		/// </summary>
		private Dictionary<string, object> attributes = new Dictionary<string, object>();
	}
}
