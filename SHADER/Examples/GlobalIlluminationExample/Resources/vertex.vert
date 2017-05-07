#version 430 core				

uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec2 uv;

out vec3 v_position;
out vec3 v_normal;
out vec2 v_uv;

void main() 
{
	v_position = position;
	v_normal = normal;
	v_uv = uv;

	gl_Position = camera * vec4(position, 1.0);
}