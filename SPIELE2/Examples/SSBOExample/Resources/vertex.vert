#version 430 core				

layout(std430, binding = 3) buffer layoutName
{
	vec2 positions[];
};
 
out vec3 pos;

void main() 
{
	pos = vec3(positions[gl_VertexID], 0.5);
	gl_Position = vec4(pos, 1.0);
}