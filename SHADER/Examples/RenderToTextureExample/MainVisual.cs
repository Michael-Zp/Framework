using DMS.Geometry;
using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;

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
		}

		public void ShaderChanged(string name, Shader shader)
		{
			if(ShaderPostProcessName == name)
			{
				shaderPostProcess = shader;
			}
			else if (ShaderName == name)
			{
				this.shader = shader;
				if (ReferenceEquals(shader, null)) return;
				Mesh mesh = Meshes.CreateCornellBox();
				geometry = VAOLoader.FromMesh(mesh, shader);
			}
		}

		public CameraOrbit OrbitCamera { get { return camera; } }

		public void Draw()
		{
			if (ReferenceEquals(shader, null)) return;
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
			renderToTexture.Activate(); //start drawing into texture
			Draw();
			renderToTexture.Deactivate(); //stop drawing into texture
			renderToTexture.Texture.Activate();
			if (ReferenceEquals(shaderPostProcess, null)) return;
			shaderPostProcess.Activate();
			GL.Uniform1(shaderPostProcess.GetUniformLocation("iGlobalTime"), time);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shaderPostProcess.Deactivate();
			renderToTexture.Texture.Deactivate();
		}

		public void Resize(int width, int height)
		{
			renderToTexture = new RenderToTexture(Texture.Create(width, height));
		}

		public static readonly string ShaderPostProcessName = nameof(shaderPostProcess);
		public static readonly string ShaderName = nameof(shader);

		private CameraOrbit camera = new CameraOrbit();
		private RenderToTexture renderToTexture;
		private Shader shaderPostProcess;
		private Shader shader;
		private VAO geometry;
	}
}
