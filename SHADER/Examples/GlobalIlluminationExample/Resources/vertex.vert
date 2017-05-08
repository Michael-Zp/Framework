#version 430 core				

uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec2 uv;

out vec3 v_position;
out vec3 v_normal;
out vec3 v_color;
out float v_specularity;

void main() 
{
	v_position = position;
	v_normal = normal;
	v_color = uv.s > 0.9 ? vec3(1) : uv.s > 0.6 ? vec3(0,1,0) : vec3(1,0,0);
	v_specularity = uv.t;
	//v_color = uv.t > 0.5 ? vec3(1);

	gl_Position = camera * vec4(position, 1.0);
}