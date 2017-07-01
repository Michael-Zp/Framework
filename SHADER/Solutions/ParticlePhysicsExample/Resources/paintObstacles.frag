#version 430 core

uniform MouseState
{
	vec2 position;
};

in vec2 uv;
out vec4 color;

void main() 
{
	float circle = distance(uv, position) < 0.05 ? 1 : 0;
	color = vec4(vec3(circle), 1.0);
}