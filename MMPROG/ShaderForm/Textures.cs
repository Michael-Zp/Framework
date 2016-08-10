using System;
using System.Collections;
using System.Collections.Generic;

namespace ShaderForm
{
	public class Textures : IEnumerable<string>, IDisposable, ITextures
	{
		public event EventHandler<EventArgs> OnChange;

		public Textures(VisualContext visual)
		{
			this.visual = visual;
		}

		public bool AddUpdate(string fileName)
		{
			if (visual.AddUpdateTexture(fileName))
			{
				textures.Add(fileName);
				CallOnChange();
				return true;
			}
			CallOnChange();
			return false;
		}

		public void Clear()
		{
			foreach (var tex in textures) visual.RemoveTexture(tex);
			textures.Clear();
			CallOnChange();
		}

		public void Dispose()
		{
			Clear();
		}

		public void Remove(string fileName)
		{
			visual.RemoveTexture(fileName);
			textures.Remove(fileName);
			CallOnChange();
		}

		public IEnumerator<string> GetEnumerator()
		{
			return textures.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return textures.GetEnumerator();
		}

		protected void CallOnChange()
		{
			if (null != OnChange) OnChange(this, EventArgs.Empty);
		}

		private List<string> textures = new List<string>();
		private VisualContext visual;
	}
}
