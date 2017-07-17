using System;

namespace DMS.HLGL
{
	public static partial class States
	{
		public interface IBackfaceCulling : IStateBool { };
		public interface IBlending : IStateBool { };
		public interface IPointSprite : IStateBool { };
		public interface IShaderPointSize : IStateBool { };
		public interface IZBufferTest : IStateBool { };
		//public interface IActiveShader : IStateHandle { };
	}
}
