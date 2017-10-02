using Zenseless.Base;
using Zenseless.HLGL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Zenseless.OpenGL
{
	using Handle = TypedHandle<IShaderProgram>;

	public class ShaderManagerGL : Disposable
	{
		public Handle CreateProgram()
		{
			var handle = new Handle(GL.CreateProgram());
			handles.Add(handle);
			return handle;
		}

		public void RemoveProgram(Handle handle)
		{
			if (handle.IsNull) throw new ArgumentNullException("Empty shader Handle");
			GL.DeleteProgram(handle.ID);
		}

		protected override void DisposeResources()
		{
			foreach(var handle in handles)
			{
				RemoveProgram(handle);
			}
		}

		private List<Handle> handles = new List<Handle>();
	}
}
