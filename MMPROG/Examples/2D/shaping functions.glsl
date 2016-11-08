#version 330
/// motivation : https://www.shadertoy.com/view/XsXXDn
/// this code is modified from https://thebookofshaders.com/05/ nice explanation + links to function tools
/// look at http://www.cdglabs.org/Shadershop/ for visual function composing

uniform vec3 iMouse;
uniform vec2 iResolution;
uniform float iGlobalTime;
varying vec2 uv;

const float PI = 3.14159265359;

/// maps normalized [0..1] coordinates 
/// into range [lowerLeft, upperRight]
vec2 map(vec2 coord01, vec2 lowerLeft, vec2 upperRight)
{
	vec2 extents = upperRight - lowerLeft;
	return lowerLeft + coord01 * extents;
}

//draw function line		
float plotFunction(float coordY, float funcResult)
{
	const float lineTickness = 0.04; 
	return smoothstep( funcResult - lineTickness, funcResult, coordY) - 
			smoothstep( funcResult, funcResult + lineTickness, coordY);
}

//draw graph and color representation of values
//positive function results are red, negative are blue
vec3 colorPlot(float coordY, float funcResult)
{
	const vec3 green = vec3(0.0, 1.0, 0.0);
    vec3 color = vec3(funcResult, 0.0, -funcResult);
    float graph = plotFunction(coordY, funcResult);
    return (1.0 - graph)*color + graph * green;
}

void main() {
	//coordinates in range [0,1]
    vec2 coord01 = gl_FragCoord.xy/iResolution;
	vec2 coord = map(coord01, vec2(-10, -1), vec2(10, 1));
	
	float x = coord.x;
    float y = x;
	// y = sin(x);
    // Step will return 0.0 unless the value is over 0.5,
    // in that case it will return 1.0
	// y = step(0.5, x);
	// y = mod(x, 0.5); // return x modulo of 0.5
	// y = fract(x); // return only the fraction part of a number
	// y = ceil(x);  // nearest integer that is greater than or equal to x
	// y = floor(x); // nearest integer less than or equal to x
	// y = sign(x);  // extract the sign of x
	// y = abs(x);   // return the absolute value of x
	// y = clamp(x,0.0,1.0); // constrain x to lie between 0.0 and 1.0
	// y = min(0.0,x);   // return the lesser of x and 0.0
	// y = max(0.0,x);   // return the greater of x and 0.0 
	// y = abs(sin(x));
	// y = fract(sin(x));
	y = ceil(sin(x)) + floor(sin(x));
	// y = exp(-0.4 * abs(x)) * cos(2 * x);
	
    gl_FragColor = vec4(colorPlot(coord.y, y), 1.0);
}
