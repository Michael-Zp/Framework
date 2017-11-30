﻿namespace Zenseless.HLGL
{
	/// <summary>
	/// Contains source code string constants for commonly used shaders
	/// </summary>
	public static class DefaultShader
	{
		/// <summary>
		/// Vertex shader that creates a screen filling quad if called with DrawArrays(4)
		/// </summary>
		public const string VertexShaderScreenQuad = @"
				#version 130
				uniform vec2 iResolution;
				out vec2 uv; 
				out vec2 fragCoord;
				void main() {
					const vec2 vertices[4] = vec2[4](vec2(-1.0, -1.0),
						vec2( 1.0, -1.0),
						vec2( 1.0,  1.0),
						vec2(-1.0,  1.0));
					vec2 pos = vertices[gl_VertexID];
					uv = pos * 0.5 + 0.5;
					fragCoord = uv * iResolution;
					gl_Position = vec4(pos, 0.0, 1.0);
				}";

		/// <summary>
		/// The vertex shader particle
		/// </summary>
		public const string VertexShaderParticle = @"
				#version 130 //gl_DepthRange
				uniform mat4 mvp = mat4(1.0);
				uniform float pointSize = 1.0;
				in vec4 position;
				void main() {
					vec4 pos = mvp * position;
					gl_PointSize = (1.0 - pos.z / pos.w) * pointSize;
					gl_Position = pos;
				}";

		/// <summary>
		/// The fragment shader color
		/// </summary>
		public const string FragmentShaderColor = @"
			#version 430 core
			uniform vec4 color = vec4(1);
			void main() {
				gl_FragColor = color;
			}";

		/// <summary>
		/// The fragment shader point circle
		/// </summary>
		public const string FragmentShaderPointCircle = @"
			#version 430 core
			uniform vec4 color = vec4(1);
			void main() {
				float f = distance(gl_PointCoord, vec2(0.5));
				gl_FragColor.rgb = color.rgb;
				gl_FragColor.a = 1 - smoothstep(0.4, 0.5, f);
			}";

		/// <summary>
		/// The fragment shader copy
		/// </summary>
		public const string FragmentShaderCopy = @"
			#version 430 core
			uniform sampler2D image;
			in vec2 uv;
			void main() {
				gl_FragColor = texture(image, uv);
			}";

		/// <summary>
		/// The fragment shader checker
		/// </summary>
		public const string FragmentShaderChecker = @"
			#version 430 core
			in vec2 uv;
			out vec4 color;
			void main() {
				vec2 uv10 = floor(uv * 10.0f);
				bool black = 1.0 > mod(uv10.x + uv10.y, 2.0f);
				color = black ? vec4(0, 0, 0, 1) : vec4(1, 1, 0, 1);
			}";
	}
}
