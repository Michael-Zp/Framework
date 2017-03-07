#version 430 core				

struct Particle
{
	vec2 position;
	vec2 velocity;
	//vec4 color; //make it vec4  not vec3 because of alignment in std430
};

layout(std430) buffer BufferParticle
{
	Particle particle[];
};
 
out vec4 colorVertex;

void process(inout Particle p)
{
	p.position += p.velocity; //update
	colorVertex = vec4(1);
	gl_Position = vec4(p.position, 0.5, 1.0);
}

void main() 
{
	process(particle[gl_VertexID]);
}