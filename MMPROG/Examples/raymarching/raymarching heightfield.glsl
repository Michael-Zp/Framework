#version 330

#include "../libs/camera.glsl"

uniform float iGlobalTime;
uniform vec2 iResolution;
uniform sampler2D tex0;
uniform sampler2D tex1;

const float epsilon = 0.01;
const vec3 terrainCenter = vec3(0, 0, 0);
const vec3 terrainExtents = vec3(400, 10, 200);

//dot(n, O +t*d)= -k
//dot(n,O) + dot(n, t*d) = -k
//t*dot(n,d)=-k-dot(n,O)
float plane(vec3 n, float k, vec3 O, vec3 d)
{
	float denominator = dot(n, d);
	if(abs(denominator) < epsilon)
	{
		//no intersection
		return -10000.0;
	}
	return (-k-dot(n,O)) / denominator;
}	

bool hitTerrainTop(vec3 ro, vec3 rd)
{
	//only checking top plane
	float d = terrainCenter.y + 0.5 * terrainExtents.y;
	vec3 n = vec3(0, 1, 0);
	return 0 < plane(n, d, ro, rd);
}

vec2 toTextureSpace(vec2 p)
{
	vec2 minCorner = terrainCenter.xz - 0.5 * terrainExtents.xz;
	return (p - minCorner) / terrainExtents.xz;
}

float f(vec2 p)
{
	return texture(tex0, toTextureSpace(p)).x * terrainExtents.y;
}

vec3 colorF(vec2 p)
{
	return texture(tex1, toTextureSpace(p)).rgb;
}

float rayMarchingBisection(vec3 ro, vec3 rd, float minT, float maxT, int count)
{
	float t;
	for(int i = 0; i < count; ++i)
	{
		float middle = 0.5 * (minT + maxT);
		vec3 p = ro + rd * middle;
		if( p.y < f( p.xz ) )
		{
			//inside
			maxT = middle;
		}
		else
		{
			//outside
			minT = middle;
		}
		t = middle;
	}
	return t;
}

float rayMarching(vec3 ro, vec3 rd, float minT, float maxT)
{
	float delta = maxT/1000.0;
	for(float t = minT; t < maxT; t += delta)
	{
		vec3 p = ro + t * rd;
		if( p.y < f( p.xz ) )
		{
			//inside
			// return t;
			return rayMarchingBisection(ro, rd, t - delta, t, 5);
		}
		// delta += 0.0003;
	}
	return maxT;
}

vec3 rayMarchingEffort(vec3 ro, vec3 rd, float minT, float maxT)
{
	float delta = maxT/1000.0;
	for(float t = minT; t < maxT; t += delta)
	{
		vec3 p = ro + t * rd;
		if( p.y < f( p.xz ) )
		{
			//inside
			return vec3(0.0, 0.02 * t, 0.0);
		}
		delta += 0.0003;
	}
	return vec3(1.0, 0.0, 0.0);
}

vec3 getNormal(vec3 p, float delta)
{
    vec3 n;
	n.x = f(vec2(p.x-delta,p.z)) - f(vec2(p.x+delta,p.z));
	n.y = 2.0 * delta;
	n.z = f(vec2(p.x,p.z-delta)) - f(vec2(p.x,p.z+delta));
    return normalize(n);
}

vec3 getShading(vec3 p, vec3 n)
{
	vec3 color = colorF(p.xz);
	vec3 lightDir = vec3(0.0, -1.0, 1.0);
	vec3 l = normalize(-lightDir);
	return dot(l, n) * color;
}

vec3 terrainColor(vec3 ro, vec3 rd, float t)
{
    vec3 p = ro + rd * t;
    vec3 n = getNormal( p, 0.8 );
    vec3 s = getShading( p, n );
    return s;
}

void main()
{
	vec3 camP = calcCameraPos();
	camP.y += 6;
	vec3 camDir = calcCameraRayDir(80.0, gl_FragCoord.xy, iResolution);

	vec3 color = vec3(0,0,0.5);
	// if(hitTerrainTop(camP, camDir)) 
	{
		float maxT = 200.0;
		float t = rayMarching(camP, camDir, 0.1, maxT);
		// float t = rayMarchingBisection(camP, camDir, 1.0, maxT, 100);
		color = t < maxT ? terrainColor(camP, camDir, t): vec3(0.0);

		// color = rayMarchingEffort(camP, camDir, 2.0, maxT);
		//fog
		// float tmax = 60.0;
		// float factor = t/tmax;
		// color = mix(color, vec3(1.0, 0.8, 0.1), factor);
	}
	gl_FragColor = vec4(color, 1.0);
}


