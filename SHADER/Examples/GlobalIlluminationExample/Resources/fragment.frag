#version 430 core

in vec3 v_position;
in vec3 v_normal;
in vec2 v_uv;

out vec4 color;

void main() 
{
	vec3 normal = normalize(v_normal);

	color =  vec4(abs(normal), 1);
}