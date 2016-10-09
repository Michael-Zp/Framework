using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
				CallOnChange();
				return true;
			}
			CallOnChange();
			return false;
		}

		public void Clear()
		{
			foreach (var tex in visual.GetTextureNames().ToList()) visual.RemoveTexture(tex);
			CallOnChange();
		}

		public void Dispose()
		{
			Clear();
		}

		public void Remove(string fileName)
		{
			visual.RemoveTexture(fileName);
			CallOnChange();
		}

		public IEnumerator<string> GetEnumerator()
		{
			return visual.GetTextureNames().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return visual.GetTextureNames().GetEnumerator();
		}

		protected void CallOnChange()
		{
			OnChange?.Invoke(this, EventArgs.Empty);
		}

		private VisualContext visual;
	}
}
