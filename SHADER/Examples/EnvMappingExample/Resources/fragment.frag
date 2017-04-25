#version 430 core

uniform sampler2D envMap;

uniform vec3 cameraPosition;

uniform vec4 materialColor;

uniform vec4 ambientLightColor;

uniform vec3 light1Direction;
uniform vec4 light1Color;

uniform vec3 light2Position;
uniform vec4 light2Color;

uniform vec3 light3Position;
uniform vec3 light3Direction;
uniform float light3Angle;
uniform vec4 light3Color;

in vec3 pos;
in vec3 n;

out vec4 color;
const float PI = 3.14159265359;

void main() 
{
	vec3 normal = normalize(n);
	vec3 v = normalize(cameraPosition - pos);
	vec3 r = reflect(-v, n);

	float deta = acos(r.z); // [0, π]
	float phi = atan(r.y, r.x); // [−π, π]
	
	float s = deta / PI;
	float t = phi / PI * 0.5 + 0.5;

	color = texture(envMap,vec2(s, t));
	//color = vec4(s, 0, 0, 1);
	//color = vec4(0, t, 0, 1);
}