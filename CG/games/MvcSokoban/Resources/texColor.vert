#version 430 core				
uniform mat4 camera;

in vec2 position;
in vec2 uv;

out vec2 uvs;
out vec2 pos;

void main() 
{
	uvs = uv;
	pos = position;
	gl_Position = camera * vec4(position, 0, 1);
}