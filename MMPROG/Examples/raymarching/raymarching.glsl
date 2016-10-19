#version 330
uniform vec2 iResolution;
uniform float iCamPosX;
uniform float iCamPosY;
uniform float iCamPosZ;

const float epsilon = 0.0001;
const int maxSteps = 128;

float dist2sphere(vec3 point, vec3 center, float radius) 
{
    return length(point - center) - radius;
}

float distFunc(vec3 point)
{
	return dist2sphere(point, vec3(0, 0, 1), 0.3);
}

vec3 calcCamDir(float fov) 
{
	float tanFov = tan(fov / 2.0 * 3.14159 / 180.0) / iResolution.x;
	vec2 p = tanFov * (gl_FragCoord.xy * 2.0 - iResolution.xy);
	return normalize(vec3(p.x, p.y, 1.0));
}

void main()
{
	vec3 camP = vec3(iCamPosX, iCamPosY, iCamPosZ);
	vec3 camDir = calcCamDir(80.0);
	
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
		gl_FragColor = vec4(0, 0, 1, 1);
	}
	else
	{
		gl_FragColor = vec4(0, 0, 0, 1);
	}
}