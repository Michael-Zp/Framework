///idea from http://thebookofshaders.com/edit.php#09/marching_dots.frag
#version 330

uniform vec2 iResolution;
uniform float iGlobalTime;

const float PI = 3.1415926535897932384626433832795;
const float TWOPI = 2 * PI;
const float EPSILON = 10e-4;

float random(float u)
{
	return fract(sin(u) * 1231534.9);
}

float random(vec2 coord) { 
    return random(dot(coord, vec2(21.97898, 7809.33123)));
}

void main() {
	//coordinates in range [0,1]
    vec2 coord = gl_FragCoord.xy/iResolution;
	
	float value = random(coord.x);
	// value = random(coord);

	const vec3 white = vec3(1);
	vec3 color = (1 - value) * white;
		
    gl_FragColor = vec4(color, 1.0);
}
