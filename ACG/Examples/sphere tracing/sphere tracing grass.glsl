#version 330

#include "../libs/camera.glsl"
#include "../libs/operators.glsl"
#include "../libs/Noise.glsl"
#include "../libs/Noise2D.glsl"
#include "../libs/Noise3D.glsl"
#include "../libs/hg_sdf.glsl"

uniform vec2 iResolution;
uniform float iGlobalTime;

const float epsilon = 0.001;
const int maxSteps = 256;
const float miss = -10000;

float distGrassBlade(vec3 point)
{
	float height = 1;
	float thickness = 0.05 * (1.5 - point.y);

	// float bending = gnoise(point.y + point.xz); // trunk bending
	// point.xz += bending;

	point = opRepeat(point, vec3(0.2, 0, 0.2));
	float cylinder = fCylinder(point, thickness, height);
	return cylinder;// + bark;
}

float distTerrain(vec3 point)
{
	return sPlane(point, vec3(0.0, 1.0, 0.0), 0);
}

float distField(vec3 point)
{
	float terrain = distTerrain(point);
	float grass = distGrassBlade(point);
	return min(terrain, grass);
}

vec3 colorField(vec3 point)
{
	vec3 colorGrassBlade = vec3(0.1, 0.8, 0);
	return colorGrassBlade;
}

float sphereTracing(vec3 O, vec3 dir, float maxT, int maxSteps)
{
	float t = 0.0;
	//step along the ray 
    for(int steps = 0; (steps < maxSteps) && (t < maxT); ++steps)
    {
		//calculate new point
		vec3 point = O + t * dir;
		//check how far the point is from the nearest surface
        float dist = distField(point);
		//if we are very close
        if(epsilon > dist)
        {
			return t;
            break;
        }
		//screen error decreases with distance
		// dist = max(dist, t * 0.001);
		//not so close -> we can step at least dist without hitting anything
		t += dist;
    }
	return miss;
}

void main()
{
	vec3 camP = calcCameraPos();
	camP.y += 6;
	vec3 camDir = calcCameraRayDir(80.0, gl_FragCoord.xy, iResolution);

	float maxT = 100;
	//start point is the camera position
	float t = sphereTracing(camP, camDir, maxT, maxSteps);
	
	vec3 color = vec3(0);
	if(0 < t)
	{
		vec3 point = camP + t * camDir;
		vec3 normal = getNormal(point, 0.01);
		vec3 lightDir = normalize(vec3(1, -1, 1));
		vec3 toLight = -lightDir;
		float diffuse = max(0, dot(toLight, normal));
		vec3 material = colorField(point);
		vec3 ambient = vec3(0);

		color = ambient + diffuse * material;
		// color = material;
		float weight = t / maxT;
		color = mix(color, vec3(0), pow(weight, 2)); //fog
	}
	gl_FragColor = vec4(color, 1);
}