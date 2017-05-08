#version 430 core
uniform vec3 ambient;
uniform vec3 lightPosition;
uniform vec3 lightColor;
uniform vec3 cameraPosition;

in vec3 v_position;
in vec3 v_normal;
in vec3 v_color;
in float v_specularity;

float lambert(vec3 n, vec3 l)
{
	return max(0, dot(n, l));
}

float specular(vec3 n, vec3 l, vec3 v, float shininess)
{
	//if(0 > dot(n, l)) return 0;
	vec3 r = reflect(-l, n);
	return pow(max(0, dot(r, v)), shininess);
}

out vec4 color;

void main() 
{
	vec3 normal = normalize(v_normal);
	vec3 v = normalize(cameraPosition - v_position);

	//point light
	vec3 l = normalize(lightPosition - v_position);
	vec3 light = v_color * ambient + v_color * lightColor * lambert(normal, l)
				+ v_specularity * lightColor * specular(normal, l, v, 100);


	color =  vec4(light, 1);
}