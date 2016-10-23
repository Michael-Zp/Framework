#version 330
#include "libs/camera.glsl"

uniform vec2 iResolution;

const float epsilon = 0.0001;
const int maxSteps = 128;

//repeat the given coordinate point every interval c[axis]
vec3 coordinateRep(vec3 point, vec3 c)
{
    return mod(point, c) - 0.5 * c;
}

//M = center of sphere
//P = some point in space
// return normal of sphere when looking from point P
vec3 sphereNormal(vec3 M, vec3 P)
{
	return normalize(P - M);
}

vec3 normalFunc(vec3 point){
	point = coordinateRep(point, vec3(3, 3, 3));
	return sphereNormal(point, vec3(0, 0, 1));
}

float dist2sphere(vec3 point, vec3 center, float radius) 
{
    return length(point - center) - radius;
}

float distFunc(vec3 point)
{
	point = coordinateRep(point, vec3(3, 3, 3));
	return dist2sphere(point, vec3(0, 0, 1), 0.3);
}

void main()
{
	vec3 camP = calcCameraPos();
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
		vec3 normal = normalFunc(point);
		vec3 lightPos = vec3(0);
		float diffuse = max(0, dot(normalize(point - lightPos), normal));
		vec3 color = vec3(1); //white
		vec3 ambient = vec3(0.1);
		gl_FragColor = vec4(ambient + diffuse * color, 1);
	}
	else
	{
		gl_FragColor = vec4(0, 0, 0, 1);
	}
}