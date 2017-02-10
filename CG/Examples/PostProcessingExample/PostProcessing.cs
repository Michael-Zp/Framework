using DMS.OpenGL;
using OpenTK.Graphics.OpenGL;

namespace Example
{
	public class PostProcessing
	{
		public const string ShaderCopy = @"
			#version 430 core
			uniform sampler2D image;
			in vec2 uv;
			void main() {
				vec3 image = texture(image, uv).rgb;
				gl_FragColor = vec4(image, 1.0);
			}";

		public PostProcessing(int width, int height)
		{
			fbo = new FBO();
			texImage = Texture.Create(width, height);
			SetShader(ShaderCopy);
		}

		public void Start()
		{
			fbo.BeginUse(texImage); //start drawing into texture
			GL.Viewport(0, 0, texImage.Width, texImage.Height);
		}

		public void EndAndApply(int width, int height, float time = 0.0f)
		{
			fbo.EndUse(); //stop drawing into texture
			GL.Viewport(0, 0, width, height);
			texImage.Activate();
			shader.Activate();
			GL.Uniform2(shader.GetUniformLocation("iResolution"), (float)width, (float)height);
			GL.Uniform1(shader.GetUniformLocation("iGlobalTime"), time);
			//GL.Uniform1(shader.GetUniformLocation("amplitude"), 0.01f);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shader.Deactivate();
			texImage.Deactivate();
		}

		public void SetShader(string fragmentShaderText)
		{
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
			shader = ShaderLoader.FromStrings(sVertexShader, fragmentShaderText);
		}

		private FBO fbo;
		private Texture texImage;
		private Shader shader;
	}
}
