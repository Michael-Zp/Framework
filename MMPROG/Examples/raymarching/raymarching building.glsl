#version 330

#include "libs/camera.glsl"

uniform vec2 iResolution;

const float epsilon = 0.0001;
const int maxSteps = 128;

float vmax(vec3 v) {
	return max(max(v.x, v.y), v.z);
}

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

vec3 opRepeat(vec3 point, vec3 inteval) {
	vec3 c = floor((point + inteval*0.5)/inteval);
	return mod(point + inteval*0.5, inteval) - inteval*0.5;
}

float distFunc(vec3 point)
{
	float dist1 = sBox(point, vec3(0, 0, 0), vec3(1, 1, 1));
	vec3 repXY = opRepeat(point, vec3(.05, .05, 1));
	float dist2 = sBox(repXY, vec3(0, 0, 0), vec3(0.01, 0.01, 0.4));
	vec3 repYZ = opRepeat(point, vec3(1, .05, .05));
	float dist3 = sBox(repYZ, vec3(0, 0, 0), vec3(0.4, 0.01, 0.01));
	return opDifference(dist1, opUnion(dist2, dist3));
}

//by numerical gradient
vec3 getNormal(vec3 point)
{
	float d = epsilon;
	//get points a little bit to each side of the point
	vec3 right = point + vec3(d, 0.0, 0.0);
	vec3 left = point + vec3(-d, 0.0, 0.0);
	vec3 up = point + vec3(0.0, d, 0.0);
	vec3 down = point + vec3(0.0, -d, 0.0);
	vec3 behind = point + vec3(0.0, 0.0, d);
	vec3 before = point + vec3(0.0, 0.0, -d);
	//calc difference of distance function values == numerical gradient
	vec3 gradient = vec3(distFunc(right) - distFunc(left),
		distFunc(up) - distFunc(down),
		distFunc(behind) - distFunc(before));
	return normalize(gradient);
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
        float dist = distFunc(point);
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
		vec3 normal = getNormal(point);
		vec3 lightDir = normalize(vec3(.5,-1,.5));
		float diffuse = max(0, dot(-lightDir, normal));
		vec3 color = vec3(1); //white
		vec3 ambient = vec3(0.2);

		gl_FragColor = vec4(ambient + diffuse * color, 1);
	}
	else
	{
		gl_FragColor = vec4(0, 0, 1, 1);
	}
}