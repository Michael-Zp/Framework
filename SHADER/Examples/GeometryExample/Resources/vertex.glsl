#version 430 core				

uniform float time;

in vec2 in_position;
in vec2 in_velocity;

void main() 
{
	gl_PointSize = 50.0;
	gl_Position = vec4(in_position + time * in_velocity, 0.0, 1.0);
}