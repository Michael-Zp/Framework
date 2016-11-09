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

float distToInt(float coord)
{
	float dist = fract(coord);
	return dist > 0.5 ? 1 - dist : dist;
}

//draw function line		
float plotFunction(float coordY, float funcResult)
{
	const float lineThickness = 0.04; 
	return smoothstep( funcResult - lineThickness, funcResult, coordY) - 
			smoothstep( funcResult, funcResult + lineThickness, coordY);
}

float grid(vec2 coord, vec2 resolution, vec2 lowerLeft, vec2 upperRight)
{
	vec2 delta = 1 * (upperRight - lowerLeft) / resolution;
	float distX = distToInt(coord.x);
	float distY = distToInt(coord.y);
	return (distX < distY) ?smoothstep(0, delta.x, distX) : smoothstep(0, delta.y, distY);
}

float onAxis(vec2 coord, vec2 resolution, vec2 lowerLeft, vec2 upperRight)
{
	vec2 absCoord = abs(coord);
	vec2 delta = (upperRight - lowerLeft) / resolution;
	vec2 distAxis = smoothstep(vec2(0), 2 * delta, absCoord);
	float onAxis = distAxis.x * distAxis.y;
	return onAxis;
}

void main() {
	//coordinates in range [0,1]
    vec2 coord01 = gl_FragCoord.xy/iResolution;
	vec2 lowerLeft = vec2(-10, -10);
	vec2 upperRight = vec2(10, 10);
	// float lineThickness = (upperRight-lowerLeft);
	vec2 coord = map(coord01, lowerLeft, upperRight);
	

	vec3 color = vec3(onAxis(coord, iResolution, lowerLeft, upperRight));
	vec3 gridColor = vec3(grid(coord, iResolution, lowerLeft, upperRight) * 0.5);
	color += gridColor;

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
	// y = ceil(sin(x)) + floor(sin(x));
	y = exp(-0.4 * abs(x)) * cos(2 * x);
	

    float graph = plotFunction(coord.y, y);
    //combine
	const vec3 green = vec3(0.0, 1.0, 0.0);
	color = (1.0 - graph) * color + graph * green;

    gl_FragColor = vec4(color, 1.0);
}
