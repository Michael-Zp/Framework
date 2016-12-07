///idea from http://thebookofshaders.com/edit.php#09/marching_dots.frag
#version 330

#include "../libs/Noise.glsl"

uniform vec2 iResolution;
uniform float iGlobalTime;
uniform vec3 iMouse;

const float PI = 3.1415926535897932384626433832795;
const float TWOPI = 2 * PI;
const float EPSILON = 10e-4;

vec2 rotate2D(vec2 coord, float angle)
{
    mat2 rot =  mat2(cos(angle),-sin(angle), sin(angle),cos(angle));
    return rot * coord;
}

float lines(in vec2 pos, float b){
    float scale = 10.0;
    pos *= scale;
    return smoothstep(0.0,
                    .5+b*.5,
                    abs((sin(pos.x* PI)+b*2.0))*.5);
}

vec3 wood(vec2 coord)
{
	coord = rotate2D(coord, gnoise(coord)); // rotate the space
    float weight = lines(coord, 0.5); // draw lines
	return 	mix(vec3(0.4, 0.2, 0), vec3(0.8, 0.8, 0), weight);

}

vec3 paint(vec2 coord, float time)
{
    // Uncomment to animate
    float t = abs(1.0-sin(time*.1))*5.;
    // Comment and uncomment the following lines:
    coord += noise(coord*2.)*t; // Animate the coordinate space
    vec3 color = vec3(1.) * smoothstep(.18,.2,noise(coord)); // Big black drops
    color += smoothstep(.15,.2,noise(coord*10.)); // Black splatter
    color -= smoothstep(.35,.4,noise(coord*10.)); // Holes on splatter
	return color;
}

vec3 lavaLamp(vec2 coord, float time)
{
    // move noise lookup coordinate with time
    float DF = gnoise(coord + vec2(time * 0.1));

	
    float a = gnoise(coord * vec2(cos(time * 0.15), sin(time * 0.1)) * 0.1) * PI;
    DF += gnoise(coord + vec2(cos(a), sin(a)));

	DF = smoothstep(0.8, 1, DF);
	return mix(vec3(0, 0, 1), vec3(1, 0, 0), 1 - DF);
}

void main() {
	//coordinates in range [0,1]
    vec2 coord = gl_FragCoord.xy/iResolution;
		
	vec3 color = wood(0.5 + coord.yx * vec2(10.,5.));
	// color = paint(coord * 10, iGlobalTime * 30);
	// color = lavaLamp(coord * 5, iGlobalTime * 3);
		
    gl_FragColor = vec4(color, 1.0);
}
