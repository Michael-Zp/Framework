#version 430 core				
struct Material
{
	vec3 color;
	float reflectivity;
	//vec4 color; //make it vec4  not vec3 because of alignment
};

uniform bufferMaterials
{
	Material material[4];
};

uniform mat4 camera;

in vec3 position;
in vec3 normal;
in vec2 uv;

out blockData
{
	vec3 position;
	vec3 normal;
	vec3 color;
	float reflectivity;
} o;

void set(Material mat)
{
	o.color = mat.color;
	o.reflectivity = mat.reflectivity;
}

void main() 
{
	o.position = position;
	o.normal = normal;
	set(material[int(uv.s)]);
	gl_Position = camera * vec4(position, 1.0);
}