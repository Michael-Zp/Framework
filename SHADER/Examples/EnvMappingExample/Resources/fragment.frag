#version 430 core

uniform sampler2D envMap;

uniform vec3 cameraPosition;

in vec3 pos;
in vec3 n;

out vec4 color;
const float PI = 3.14159265359;

vec2 envMapEquirect(vec3 normal) {
  float phi = acos(normal.y);
  float theta = atan(-normal.x, normal.z) + PI;
  return vec2(theta / (2*PI), phi / PI);
}

void main() 
{
	vec3 normal = normalize(n);
	vec3 v = normalize(-pos);
	//vec3 r = reflect(-v, normal);
	vec3 r = v;

	color = texture(envMap, envMapEquirect(r));
}