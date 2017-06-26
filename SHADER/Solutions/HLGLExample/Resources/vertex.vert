#version 430 core				

uniform ShaderParameter  //use 16 byte alignment or you have to query all variable offsets
{
	mat4 camera;
	vec3 translate;
};

in vec3 position;
in vec3 normal;
in vec2 uv;

out vec3 n;
out vec2 uvs;

void main() 
{
	n = normal;
	uvs = uv;

	gl_Position = camera * vec4(translate + position, 1.0);
}