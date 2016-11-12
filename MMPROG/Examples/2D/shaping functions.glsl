#version 330
/// motivation : https://www.shadertoy.com/view/XsXXDn
/// idea from https://thebookofshaders.com/05/ nice explanation + links to function tools
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

//calculate the smallest just visible width/height for objects
vec2 screenDelta(vec2 resolution, vec2 lowerLeft, vec2 upperRight)
{
	return (upperRight - lowerLeft) / resolution;
}

//distance to nearest integer
float distToInt(float coord)
{
	float dist = fract(coord);
	return dist > 0.5 ? 1 - dist : dist;
}

vec2 distToInt(vec2 coord)
{
	vec2 dist = fract(coord);
	dist.x = dist.x > 0.5 ? 1 - dist.x : dist.x;
	dist.y = dist.y > 0.5 ? 1 - dist.y : dist.y;
	return dist;
}

float grid(vec2 coord, vec2 screenDelta)
{
	vec2 dist = vec2(distToInt(coord));
	vec2 smoothGrid = smoothstep(vec2(0), screenDelta, dist);
	return smoothGrid.x * smoothGrid.y;
}

float onAxis(vec2 coord, vec2 screenDelta)
{
	vec2 absCoord = abs(coord);
	vec2 distAxis = smoothstep(vec2(0), 2 * screenDelta, absCoord);
	float onAxis = distAxis.x * distAxis.y;
	return onAxis;
}

//draw function line		
float plotFunction(float coordY, float funcResult, float thickness)
{
	return smoothstep( funcResult - thickness, funcResult, coordY) - 
			smoothstep( funcResult, funcResult + thickness, coordY);
}

void main() {
	//map coordinates in range [0,1]
    vec2 coord01 = gl_FragCoord.xy/iResolution;
	//screen aspect
	float aspect = iResolution.x / iResolution.y;
	//coordinate system corners
	vec2 lowerLeft = vec2(-10 * aspect, -10);
	vec2 upperRight = vec2(10 * aspect, 10);
	//setup coordinate system
	vec2 coord = map(coord01, lowerLeft, upperRight);
	//calculate just visible screen deltas
	vec2 screenDelta = screenDelta(iResolution, lowerLeft, upperRight);

	//axis
	vec3 color = vec3(onAxis(coord, screenDelta));
	//grid
	vec3 gridColor = vec3(1 - (1 - grid(coord, screenDelta)) * 0.1);
	//combine
	color *= gridColor;
	
	
	//function
	float x = coord.x;
    float y = x;
	// y = sin(x);
    // Step will return 0.0 unless the value is over 0.5,
    // in that case it will return 1.0
	// y = step(0.5, x);
	// y = mod(x, 1.0); // return x modulo of 0.5
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
	// y = exp(-0.4 * abs(x)) * 20 * cos(2 * x);
	// y = abs(mod(x, 2.0) - 1); // repeated tent
	y = abs(mod(x, 2.0) - 1) * step(2, mod(x + 1.0, 4.0)); 

    float graph = plotFunction(coord.y, y, 4 * max(screenDelta.x, screenDelta.y));
    // combine
	const vec3 green = vec3(0.0, 1.0, 0.0);
	color = (1.0 - graph) * color + graph * green;

    gl_FragColor = vec4(color, 1.0);
}
