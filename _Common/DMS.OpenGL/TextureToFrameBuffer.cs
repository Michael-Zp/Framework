﻿using OpenTK.Graphics.OpenGL;

namespace DMS.OpenGL
{
	public class TextureToFrameBuffer
	{
		public delegate void SetUniforms(Shader currentShader);

		public TextureToFrameBuffer(string fragmentShader = FragmentShaderCopy, string vertexShader = VertexShaderScreenQuad)
		{
			shader = ShaderLoader.FromStrings(vertexShader, fragmentShader);
		}

		public void Draw(Texture texture, SetUniforms setUniformsHandler = null)
		{
			shader.Activate();
			texture.Activate();
			setUniformsHandler?.Invoke(shader);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			texture.Deactivate();
			shader.Deactivate();
		}

		public const string VertexShaderScreenQuad = @"
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

		public const string FragmentShaderCopy = @"
			#version 430 core
			uniform sampler2D image;
			in vec2 uv;
			void main() {
				vec3 image = texture(image, uv).rgb;
				gl_FragColor = vec4(image, 1.0);
			}";

		private Shader shader;
	}
}