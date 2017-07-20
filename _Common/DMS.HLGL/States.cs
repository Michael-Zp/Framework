using System;
using System.Numerics;

namespace DMS.HLGL
{
	public static partial class States
	{
		public interface IBackfaceCulling : IStateBool { };
		public interface IBlending : IStateBool { };
		public interface IClearColor : IStateTyped<Vector4> { };
		public interface ILineWidth : IStateTyped<float> { };
		public interface IPointSprite : IStateBool { };
		public interface IShaderPointSize : IStateBool { };
		public interface IZBufferTest : IStateBool { };
		//public interface IActiveShader : IStateHandle { };
	}
}
