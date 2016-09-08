#version 430 core				

struct Particle
{
	vec2 position;
	vec2 velocity;
};

layout(std430, binding = 3) buffer layoutName
{
	Particle particle[];
};
 
out vec3 pos;

void main() 
{
	pos = vec3(particle[gl_VertexID].position, 0.5);
	particle[gl_VertexID].position += particle[gl_VertexID].velocity;
	gl_Position = vec4(pos, 1.0);
}