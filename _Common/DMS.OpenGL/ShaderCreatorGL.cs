using DMS.HLGL;
using System;
using System.Collections.Generic;

namespace DMS.OpenGL
{
	public class ShaderCreatorGL : ICreator<IShader>, IShader
	{
		public TypedHandle<IShader> Create()
		{
			var shader = new Shader();
			shaders.Add(shader.ProgramID, shader);
			return new TypedHandle<IShader>(shader.ProgramID);
		}

		public Shader Get(TypedHandle<IShader> handle)
		{
			if(shaders.TryGetValue(handle.ID, out Shader shader))
			{
				return shader;
			}
			throw new ArgumentException("Invalid Handle id given");
		}

		private Dictionary<int, Shader> shaders = new Dictionary<int, Shader>();
	}
}
