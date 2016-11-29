///idea from http://thebookofshaders.com/edit.php#09/marching_dots.frag
#version 330

uniform vec2 iResolution;
uniform float iGlobalTime;
uniform vec3 iMouse;

const float PI = 3.1415926535897932384626433832795;
const float TWOPI = 2 * PI;
const float EPSILON = 10e-4;

float quad(vec2 coord, vec2 lowerLeft, vec2 size)
{
	vec2 a = step(lowerLeft, coord);
	vec2 b = 1 - step(lowerLeft + size, coord);
	return a.x * b.x * a.y * b.y;
}

float rand(float seed)
{
	return fract(sin(seed) * 1231534.9);
}

float rand(vec2 seed) { 
    return rand(dot(seed, vec2(12.9898, 783.233)));
}

//interpolated random values at integer intervalls
float noise(float u)
{
	float intU = floor(u);
	float fractU = fract(u);
	return mix(rand(intU), rand(intU + 1), fractU); //linear interpolation
	return mix(rand(intU), rand(intU + 1), smoothstep(0, 1, fractU)); //cubic interpolation
}

//interpolated random values at integer grid
float noise(vec2 coord)
{
	vec2 i = floor(coord);
	vec2 f = fract(coord);

	float v00 = rand(i);
	float v10 = rand(i + vec2(1, 0));
	float v01 = rand(i + vec2(0, 1));
	float v11 = rand(i + vec2(1, 1));
	
	//bi-cubic interpolation
	vec2 cubic = smoothstep(0, 1, f);
	float x1 = mix(v00, v10, cubic.x);
	float x2 = mix(v01, v11, cubic.x);
	return mix(x1, x2, cubic.y);
}

void main() {
	//coordinates in range [0,1]
    vec2 coord = gl_FragCoord.xy/iResolution;
	
	float value = rand(coord.x); //cannot control frequency
	// value = noise(coord.x * 100); //can control frequency
	
	// vec2 mouse = iMouse.xy / iResolution * 100;
	// vec2 lowerLeft = vec2(0.2, 0.2) + 0.05 * vec2(noise(coord.y * mouse.y), noise(coord.x * mouse.x));
	// value = quad(coord, lowerLeft, vec2(0.5, 0.5));
	
	//2d
	// value = rand(coord);
	// value = noise(coord * 10);

	
	const vec3 white = vec3(1);
	vec3 color = value * white;
		
    gl_FragColor = vec4(color, 1.0);
}
