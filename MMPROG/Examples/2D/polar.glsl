#version 330

const float PI = 3.14159265359;
const float EPSILON = 10e-4;

uniform vec2 iResolution;
uniform float iGlobalTime;

void main(){
    vec2 st = gl_FragCoord.xy/iResolution.xy;
    vec3 color = vec3(0.0);

    vec2 pos = vec2(1) - 2 * st;
	//aspect correction
	pos.x *= iResolution.x / iResolution.y;
	
	//cartesian to polar
    float r = length(pos);
    float a = atan(pos.y, pos.x) + PI;

    float f = cos(a * 5);
    // f = abs(cos(a * 7));
    // f = abs(cos(a*2.5))*.6+0.3;
    // f = abs(cos(a*12.)*sin(a*3.))*.8+.1;
    // f = smoothstep(-.5,1., cos(a*10.))*0.2+0.5;
    color = vec3(f);
    // color = vec3( 1. - smoothstep(f, f + 0.02, r) );

    gl_FragColor = vec4(color, 1.0);
	// a /= 2*3.1415;
    // gl_FragColor = vec4(a, a, a, 1.0);
}