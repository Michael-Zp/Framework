using System;
using OpenTK.Graphics.OpenGL;

namespace DMS.HLGL
{
	public class StateCommandGL<TYPE> : IStateCommand<TYPE> where TYPE : IEquatable<TYPE>
	{
		public StateCommandGL(Action<TYPE> glCommand, TYPE defaultValue)
		{
			if (ReferenceEquals(null, glCommand)) throw new ArgumentNullException();
			this.glCommand = glCommand;
			value = defaultValue;
			UpdateGL();
		}

		public TYPE Value
		{
			get => value;
			set
			{
				if (value.Equals(this.value)) return;
				this.value = value;
				UpdateGL();
			}
		}

		private TYPE value;
		private Action<TYPE> glCommand;

		private void UpdateGL()
		{
			glCommand.Invoke(value);
		}
	}
}
