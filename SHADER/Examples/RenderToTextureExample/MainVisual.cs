using DMS.Geometry;
using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;
using System.Text;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			camera.FarClip = 50;
			camera.Distance = 1.8f;
			camera.TargetY = -0.3f;
			camera.FovY = 70;
			shaderPostProcess = ShaderLoader.FromStrings(TextureToFrameBuffer.VertexShaderScreenQuad, Encoding.UTF8.GetString(Resources.Swirl));
			shader = ShaderLoader.FromStrings(Encoding.UTF8.GetString(Resources.vertex), Encoding.UTF8.GetString(Resources.fragment));
			Mesh mesh = Meshes.CreateCornellBox();
			geometry = VAOLoader.FromMesh(mesh, shader);
		}

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void Draw()
		{
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			geometry.Draw();
			shader.Deactivate();
			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.CullFace);
		}

		public void DrawWithPostProcessing(float time)
		{
			rtt.Activate(); //start drawing into texture
			Draw();
			rtt.Deactivate(); //stop drawing into texture
			rtt.Texture.Activate();
			shaderPostProcess.Activate();
			GL.Uniform1(shaderPostProcess.GetUniformLocation("iGlobalTime"), time);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderPostProcess.Deactivate();
			rtt.Texture.Deactivate();
		}

		public void Resize(int width, int height)
		{
			rtt = new RenderToTexture(Texture.Create(width, height));
			camera.Aspect = (float)width / height;
		}

		private CameraOrbit camera = new CameraOrbit();
		private RenderToTexture rtt;
		private Shader shaderPostProcess;
		private Shader shader;
		private VAO geometry;
	}
}
