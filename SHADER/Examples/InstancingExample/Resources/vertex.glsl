#version 430 core				

in vec3 instancePosition;

in vec3 position;
in vec3 normal;

out vec3 n;
out vec3 var_color;

void main() 
{
	n = normal;

	vec3 pos = position + instancePosition;
	var_color = position * 100;
	gl_Position = vec4(pos, 1.0);
}