namespace DMS.HLGL
{
	public interface IState { };
	public struct BackfaceCulling : IState { };
	public struct Blend : IState { };
	public struct PointSprite : IState { };
	public struct ShaderPointSize : IState { };
	public struct ZBufferTest : IState { };
}
