#version 330

const float PI = 3.14159265359;
const float TWOPI = 2 * PI;
const float EPSILON = 10e-4;

uniform vec2 iResolution;
uniform float iGlobalTime;
uniform vec3 iMouse;
	
float distToInt(float coord)
{
	float dist = fract(coord);
	return dist > 0.5 ? 1.0 - dist : dist;
}

float distField(const vec2 coord)
{
	//cartesian to polar coordinates
    float r = length(coord); // radius of current pixel
    float a = atan(coord.y, coord.x) + PI; //angel of current pixel [0..2*PI] 
	
	return r - 1 + 0.5 * sin(3 * a + 2 * r * r);
}

vec2 grad(const vec2 coord)
{
    vec2 h = vec2( 0.05, 0.0 );
    return vec2( distField(coord + h.xy) - distField(coord - h.xy),
                 distField(coord + h.yx) - distField(coord - h.yx) )/(2.0 * h.x);
}

void main()
{
	//create uv to be in the range [0..1]²
	vec2 uv = gl_FragCoord.xy / iResolution;
	
	float threshold = sin(iGlobalTime);

	// range [-1..1]²
    uv = vec2(1) - 2 * uv;
	//aspect correction
	uv.x *= iResolution.x / iResolution.y;
	uv *= 2;
	
	float f = distField(uv);
	float de = abs(f) / length(grad(uv));

	float subSet = f;
	subSet = smoothstep(0.0, 0.03, de);
	// subSet = step(threshold, f);

	// float blurryness = 0.012; //control sharpness
	// float thickness = 0.005;
	// subSet = smoothstep(thickness, thickness + blurryness, distToInt(subSet * 20)); // repeat step	
	
	vec3 color = vec3(subSet);	
	
	gl_FragColor = vec4(color, 1.0);
}
