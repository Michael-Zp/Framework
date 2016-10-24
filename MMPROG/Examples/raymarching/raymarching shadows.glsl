#version 330

#include "libs/camera.glsl"
#include "libs/hg_sdf.glsl"

uniform vec2 iResolution;
uniform float iGlobalTime;

const float epsilon = 0.0001;
const int maxSteps = 128;


float uBox(vec3 point, vec3 center, vec3 b )
{
  return length(max(abs(point - center) - b, vec3(0.0)));
}

float sBox(vec3 point, vec3 center, vec3 b) {
	vec3 d = abs(point - center) - b;
	return length(max(d, vec3(0))) + vmax(min(d, vec3(0)));
}

float sSphere(vec3 point, vec3 center, float radius) {
    return length(point - center) - radius;
}

float uSphere(vec3 point, vec3 center, float radius) {
    return max(0.0, sSphere(point, center, radius));
}

float opUnion(float dist1, float dist2)
{
	return min(dist1, dist2);
}

float opIntersection(float dist1, float dist2)
{
	return max(dist1, dist2);
}

float opDifference(float dist1, float dist2)
{
	return max(dist1, -dist2);
}

vec3 opRepeat(vec3 point, vec3 interval) {
	vec3 c = floor((point + interval*0.5)/interval);
	return mod(point + interval*0.5, interval) - interval*0.5;
}

float distField(vec3 point)
{
	float distPlane = fPlane(point, vec3(0, 1, 0), 1);
	float distX = sBox(opRepeat(point - vec3(0,.5,0), vec3(0, 0, 1)), vec3(0, 0, 0), vec3(10, 0.05, 0.05));
	float distZ = sBox(opRepeat(point - vec3(0,.5,0), vec3(1, 0, 0)), vec3(0, 0, 0), vec3(0.05, 0.05, 10));
	return opUnion(distPlane, opUnion(distX, distZ));
}

//by numerical gradient
vec3 getNormal(vec3 point, float delta)
{
	//get points a little bit to each side of the point
	vec3 right = point + vec3(delta, 0.0, 0.0);
	vec3 left = point + vec3(-delta, 0.0, 0.0);
	vec3 up = point + vec3(0.0, delta, 0.0);
	vec3 down = point + vec3(0.0, -delta, 0.0);
	vec3 behind = point + vec3(0.0, 0.0, delta);
	vec3 before = point + vec3(0.0, 0.0, -delta);
	//calc difference of distance function values == numerical gradient
	vec3 gradient = vec3(distField(right) - distField(left),
		distField(up) - distField(down),
		distField(behind) - distField(before));
	return normalize(gradient);
}

float softshadow( in vec3 origin, in vec3 dir, float mint, float maxt, float k )
{
    float res = 1.0;
    for( float t = mint; t < maxt; )
    {
        float h = distField(origin + dir * t);
        if( h < epsilon )
            return 0.0;
        res = min( res, k*h/t );
        t += h;
    }
    return res;
}

void main()
{
	vec3 camP = calcCameraPos();
	camP.z += -1.0;
	vec3 camDir = calcCameraRayDir(80.0, gl_FragCoord.xy, iResolution);

	//start point is the camera position
	vec3 point = camP; 	
	bool objectHit = false;
	float t = 0.0;
	//step along the ray 
    for(int steps = 0; steps < maxSteps; ++steps)
    {
		//check how far the point is from the nearest surface
        float dist = distField(point);
		//if we are very close
        if(epsilon > dist)
        {
			objectHit = true;
            break;
        }
		//not so close -> we can step at least dist without hitting anything
        t += dist;
		//calculate new point
        point = camP + t * camDir;
    }

	if(objectHit)
	{
		vec3 normal = getNormal(point, 0.01);
		vec3 lightDir = normalize(vec3(sin(iGlobalTime), -1.0, cos(iGlobalTime)));
		vec3 toLight = -lightDir;
		float diffuse = max(0, dot(toLight, normal));
		vec3 color = vec3(1); //white
		vec3 ambient = vec3(0.2);

		//shadows
		float shadow = max(0.2, softshadow(point, toLight, 0.1, 
		  10, 20.0));

		gl_FragColor = vec4(ambient + shadow * diffuse * color, 1);
	}
	else
	{
		gl_FragColor = vec4(0, 0, 1, 1);
	}
}