using DMS.HLGL;
using System;
using System.Collections.Generic;

namespace DMS.OpenGL
{
	public class ShaderCreatorGL : ICreator<IShader>
	{
		public TypedHandle<IShader> Create()
		{
			var shader = new Shader();
			shaders.Add(shader.ProgramID, shader);
			return new TypedHandle<IShader>(shader.ProgramID);
		}

		public IShader Get(TypedHandle<IShader> handle)
		{
			if(shaders.TryGetValue(handle.ID, out IShader shader))
			{
				return shader;
			}
			throw new ArgumentException("Invalid Handle id given");
		}

		private Dictionary<int, IShader> shaders = new Dictionary<int, IShader>();
	}
}
