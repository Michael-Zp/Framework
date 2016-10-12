#version 330
uniform vec3 iMouse;
uniform float iGlobalTime;
uniform float cameraX;
uniform float cameraZ;
uniform vec2 iResolution;
uniform sampler2D tex0;
uniform sampler2D tex1;

const float epsilon = 0.1;

float f(float px, float py)
{
	vec2 p = vec2(px, py);
	p *= 0.03;
	p -= 0.4;
	return texture(tex0, p).x * 1.0;
}

vec3 colorF(float px, float py)
{
	vec2 p = vec2(px, py);
	p *= 0.03;
	p -= 0.4;
	return texture(tex1, p).rgb;
}

float rayMarchingBisection(vec3 ro, vec3 rd, float minT, float maxT, int count)
{
	float t;
	for(int i = 0; i < count; ++i)
	{
		float middle = 0.5 * (minT + maxT);
		vec3 p = ro + rd * middle;
		if( p.y < f( p.x, p.z ) )
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
		if( p.y < f( p.x, p.z ) )
		{
			//inside
			// return t;
			return rayMarchingBisection(ro, rd, t - delta, t, 5);
		}
		delta += 0.0003;
	}
	return maxT;
}

vec3 rayMarchingEffort(vec3 ro, vec3 rd, float minT, float maxT)
{
	float delta = maxT/1000.0;
	for(float t = minT; t < maxT; t += delta)
	{
		vec3 p = ro + t * rd;
		if( p.y < f( p.x, p.z ) )
		{
			//inside
			return vec3(0.0, 0.02 * t, 0.0);
		}
		delta += 0.0003;
	}
	return vec3(1.0, 0.0, 0.0);
}

vec3 getNormal(vec3 p)
{
    vec3 n;
	n.x = f(p.x-epsilon,p.z) - f(p.x+epsilon,p.z);
	n.y = 2.0 * epsilon;
	n.z = f(p.x,p.z-epsilon) - f(p.x,p.z+epsilon);
    return normalize(n);
}

vec3 getShading(vec3 p, vec3 n)
{
	vec3 color = colorF(p.x, p.z);
	vec3 lightPosition = vec3(0.0, 15.0, 0.0);
	vec3 l = normalize(lightPosition - p);
	return dot(l, n) * color;
}

vec3 terrainColor(vec3 ro, vec3 rd, float t)
{
    vec3 p = ro + rd * t;
    vec3 n = getNormal( p );
    vec3 s = getShading( p, n );
    return s;
}

void main()
{
	float fov = 80.0;
	float tanFov = tan(fov / 2.0 * 3.14159 / 180.0) / iResolution.x;
	vec2 p = tanFov * (gl_FragCoord.xy * 2.0 - iResolution.xy);

	vec2 pos = iMouse.xy * 0.025 + vec2( 0.8, 10.);
	vec3 camP = vec3(cameraX, 3.0, cameraZ);
	vec3 camDir = normalize(vec3(p.x, p.y, 1.0));

	float maxT = 60.0;
	float t = rayMarching(camP, camDir, 1.0, maxT);
	// float t = rayMarchingBisection(camP, camDir, 1.0, maxT, 100);
	vec3 color = t < maxT ? terrainColor(camP, camDir, t): vec3(0.0);

	// vec3 color = rayMarchingEffort(camP, camDir, 2.0, maxT);
	//fog
	// float tmax = 60.0;
	// float factor = t/tmax;
	// color = mix(color, vec3(1.0, 0.8, 0.1), factor);
	
	gl_FragColor = vec4(color, 1.0);
}


