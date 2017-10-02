﻿using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL4;

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

		public CameraOrbit OrbitCamera { get { return camera; } }
		public static readonly string ShaderPostProcessName = nameof(shaderPostProcess);
		public static readonly string ShaderName = nameof(shader);

		public void ShaderChanged(string name, IShader shader)
		{
			if(ShaderPostProcessName == name)
			{
				shaderPostProcess = shader; //todo: ati seams to do VAO vertex attribute ordering different for each shader would need to create own vao
			}
			else if (ShaderName == name)
			{
				this.shader = shader;
				if (ReferenceEquals(shader, null)) return;
				Mesh mesh = Meshes.CreateCornellBox();
				geometry = VAOLoader.FromMesh(mesh, shader);
			}
		}

		public void Draw()
		{
			if (ReferenceEquals(shader, null)) return;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			shader.Activate();
			var cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetResourceLocation(ShaderResourceType.Uniform, "camera"), true, ref cam);
			geometry.Draw();
			shader.Deactivate();
			GL.Disable(EnableCap.CullFace);
			GL.Disable(EnableCap.DepthTest);
		}

		public void DrawWithPostProcessing(float time)
		{
			renderToTexture.Activate(); //start drawing into texture
			Draw();
			renderToTexture.Deactivate(); //stop drawing into texture
			renderToTexture.Texture.Activate(); //us this new texture
			if (ReferenceEquals(shaderPostProcess, null)) return;
			shaderPostProcess.Activate(); //activate post processing shader
			GL.Uniform1(shaderPostProcess.GetResourceLocation(ShaderResourceType.Uniform, "iGlobalTime"), time);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4); //draw quad
			shaderPostProcess.Deactivate();
			renderToTexture.Texture.Deactivate();
		}

		public void Resize(int width, int height)
		{
			renderToTexture = new FBOwithDepth(Texture2dGL.Create(width, height));
		}

		private CameraOrbit camera = new CameraOrbit();
		private FBO renderToTexture;
		private IShader shaderPostProcess;
		private IShader shader;
		private VAO geometry;
	}
}
