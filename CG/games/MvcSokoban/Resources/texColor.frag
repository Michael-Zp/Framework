#version 430 core
uniform vec4 tint = vec4(1);
uniform sampler2D texDiffuse;

in vec2 pos;
in vec2 uvs;
out vec4 fragColor;

void main() 
{
	fragColor = tint * texture(texDiffuse, uvs);
	fragColor = tint * vec4(fract(pos), 0, 1);
}