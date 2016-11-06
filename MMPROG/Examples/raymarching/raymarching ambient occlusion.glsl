#version 330

#include "libs/camera.glsl"
#include "libs/hg_sdf.glsl"

uniform vec2 iResolution;
uniform float iGlobalTime;

const float epsilon = 0.01;
const int maxSteps =128;


vec3 rotateY(vec3 point, float angle)
{
	mat3 rot = mat3(cos( angle ), 0.0, -sin( angle ),
					0.0,           1.0, 0.0,
					sin( angle ), 0.0, cos( angle ));
	return rot * point;
}

vec3 rotateZ(vec3 point, float angle)
{
	mat3 rot = mat3(cos( angle ), -sin( angle ), 0.0,
					sin( angle ),  cos( angle ), 0.0,
					0.0,           0.0, 1.0);
	return rot * point;
}

float distTentacle(vec3 point)
{
	float rr = dot(point.xy, point.xy);
	float dist = 10e7;
	for(int i = 0; i < 6; ++i)
	{
		vec3 p2 = rotateY( point, TAU * i / 6.0 + 0.04 * rr  );
		p2.y -= 3 * rr * exp2(-10.0 * rr);
		vec3 p3 = rotateZ(p2, PI / 2);
		float cylinder = fCylinder(p3, 0.1, 30.0);
		dist = min( dist, cylinder );
	}
	return dist;
}

///http://www.iquilezles.org/www/articles/smin/smin.htm
float smin( float a, float b, float k )
{
    float h = clamp( 0.5+0.5*(b-a)/k, 0.0, 1.0 );
    return mix( b, a, h ) - k*h*(1.0-h);
}

float distMonster(vec3 point)
 {
	float move = dot(point.xz, point.xz) * 0.2 * (sin(iGlobalTime));
	point. y += move;
	float tentacle = distTentacle(point);
	point.y -= 0.1;
	float sphere = fSphere(point, 0.35);
	return smin(tentacle, sphere, 0.1 );
 }

float distField(vec3 point)
{
	float plane = fPlane(point, vec3(0, 1, 0), 0.1);
	float monster = distMonster(point);
	return min(plane, monster);
}

//normal by numerical gradient
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

float ambientOcclusion(vec3 point, float delta, int samples)
{
	vec3 normal = getNormal(point, 0.0001);
	float occ = 0;
	for(int i = 1; i < samples; ++i)
	{
		occ += 1.0/i * (i * delta - distField(point + i * delta * normal));
	}
	return 1 - occ;
}

void main()
{
	vec3 camP = calcCameraPos();
	camP.z += -3.0;
	camP.y += 0.3;
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

	vec3 color = vec3(0, 0, 1);
	if(objectHit)
	{
		vec3 normal = getNormal(point, 0.01);
		vec3 lightDir = normalize(vec3(sin(iGlobalTime), -1.0, cos(iGlobalTime)));
		vec3 toLight = -lightDir;
		float diffuse = max(0, dot(toLight, normal));
		vec3 material = vec3(1); //white
		vec3 ambient = vec3(0.1);

		color = ambient + diffuse * material;
		color = ambientOcclusion(point, 0.01, 10) * material;
		// color = material;
	}
	//fog
	float tmax = 10.0;
	float factor = t/tmax;
	factor = clamp(factor, 0.0, 1.0);
	color = mix(color, vec3(1.0, 0.8, 0.1), factor);
	gl_FragColor = vec4(color, 1);
}