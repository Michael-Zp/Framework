using Framework;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System;
using OpenTK;
using OpenTK.Input;

namespace Example
{
	public class Rasterizer
	{
		public delegate void DrawHandler();

		public Rasterizer(int resolutionX, int resolutionY, DrawHandler drawHandler)
		{
			if (ReferenceEquals(null, drawHandler)) throw new ArgumentException("Draw handler must not equal null!");
			LoadCopyShader();
			this.drawHandler = drawHandler;
			gameWindow.KeyDown += GameWindow_KeyDown;
			//registers a callback for drawing a frame
			gameWindow.RenderFrame += GameWindow_RenderFrame;
			gameWindow.RenderFrame += (sender, e) => gameWindow.SwapBuffers();
			texRenderSurface = Texture.Create(resolutionX, resolutionY);
			texRenderSurface.FilterNearest();
			renderToTexture = new FBO();
			GL.ClearColor(Color.White);
		}

		public void Run()
		{
			gameWindow.Run();
		}

		private GameWindow gameWindow = new GameWindow(1024, 1024);
		private Texture texRenderSurface;
		private FBO renderToTexture;
		private Shader shader;
		private DrawHandler drawHandler;

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (Key.Escape == e.Key)
			{
				gameWindow.Exit();
			}
		}
		private void GameWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			renderToTexture.BeginUse(texRenderSurface);
			GL.Viewport(0, 0, texRenderSurface.Width, texRenderSurface.Height);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			drawHandler();
			renderToTexture.EndUse();
			GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			texRenderSurface.BeginUse();
			shader.BeginUse();
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shader.EndUse();
			texRenderSurface.EndUse();
		}

		private void LoadCopyShader()
		{
			string shaderCopy = @"
				#version 430 core
				uniform sampler2D image;
				in vec2 uv;
				void main() {
					vec3 color = texture(image, uv).rgb;
					gl_FragColor = vec4(color, 1.0);
				}";
			string sVertexShader = @"
				#version 130				
				out vec2 uv; 
				void main() {
					const vec2 vertices[4] = vec2[4](vec2(-1.0, -1.0),
                                    vec2( 1.0, -1.0),
                                    vec2( 1.0,  1.0),
                                    vec2(-1.0,  1.0));
					vec2 pos = vertices[gl_VertexID];
					uv = pos * 0.5 + 0.5;
					gl_Position = vec4(pos, 1.0, 1.0);
				}";
			shader = ShaderLoader.FromStrings(sVertexShader, shaderCopy);
		}
	}
}
