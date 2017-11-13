#version 120
uniform vec3 iMouse;
uniform vec2 iResolution;
uniform float iGlobalTime;
varying vec2 uv;

const float bigNumber = 10000.0;
const float eps = 0.001;
vec3 toLight = normalize(vec3(sin(iGlobalTime + 2.0), 0.6, cos(iGlobalTime + 2.0)));
const vec3 ambient = vec3(0.2);

vec3 background(vec3 dir)
{
	float sun = max(0.0, dot(dir, toLight));
	float sky = max(0.0, dot(dir, vec3(0.0, 1.0, 0.0)));
	float ground = max(0.0, -dot(dir, vec3(0.0, 1.0, 0.0)));
	return 
  (pow(sun, 256.0) + 0.2 * pow(sun, 2.0)) * vec3(2.0, 1.6, 1.0) +
  pow(ground, 0.5) * vec3(0.4, 0.3, 0.2) +
  pow(sky, 1.0) * vec3(0.5, 0.6, 0.7);
}

struct Ray
{
	vec3 origin;
	vec3 dir;
};

float sphere(vec3 M, float r, struct Ray ray)
{
	vec3 MO = ray.origin - M;
	float dotDirMO = dot(ray.dir, MO);
	float root = dotDirMO * dotDirMO - dot(ray.dir, ray.dir) * (dot(MO, MO) - r * r);
	if(root < eps)
	{
		return -bigNumber;
	}
	float p = -dot(ray.dir, MO);
	float q = sqrt(root);
    return (p - q) > 0.0 ? p - q : p + q;
}

//dot(n, O +t*d)= -k
//dot(n,O) + dot(n, t*d) = -k
//t*dot(n,d)=-k-dot(n,O)
float plane(vec3 n, float k, struct Ray ray)
{
	float denominator = dot(n, ray.dir);
	if(abs(denominator) < eps)
	{
		//no intersection
		return -bigNumber;
	}
	return (-k-dot(n, ray.origin)) / denominator;
}	

vec3 sphereNormal(vec3 M, vec3 P)
{
	return normalize(P - M);
}

vec3 sphereColor(vec3 M)
{
	return abs(normalize(M - vec3(0.0, 0.0, 3.2)));
}

float lambert(vec3 n)
{
	return max(0, dot(n, toLight));
}

struct Intersection
{
	bool exists;
	vec3 n;
	vec3 color;
	vec3 point;
};

Intersection findNearestObjectHit(struct Ray ray)
{
	float t = bigNumber;
	vec3 M = vec3(-bigNumber);
	vec3 normal = vec3(0.0, 0.0, 0.0);
	for(float x = -2.0; x <= 2.0; x += 0.7)
	{
		float y = -1.0;
		for(float z = 1.0; z <= 10.0; z += 0.7)
		{	
			vec3 newM = vec3(x, y, z);
			float newT = sphere(newM, 0.1, ray);
			if (0.0 < newT && newT < t)
			{	
				t = newT;
				M = newM;
			}
		}
	}
	Intersection obj;
	obj.n = normalize(vec3(0.0, 1.0, 0.1));
	float newT = plane(obj.n, 0.9, ray);
	if (0.0 < newT && newT < t)
	{
		obj.exists = true;
		obj.color = vec3(0.5, 0.5, 0.5);
		obj.point = ray.origin + newT * ray.dir;
		obj.point += obj.n * eps; // numerical stable point
		return obj;
	}
	obj.exists = t < bigNumber;
	if(!obj.exists) return obj;
	obj.color = sphereColor(M);
	obj.point = ray.origin + t * ray.dir;
	obj.n = sphereNormal(M, obj.point);
	obj.point += obj.n * eps; // numerical stable point
	return obj;
}

vec3 directLighting(vec3 dir, Intersection inter)
{
	//shadow ray
	if(findNearestObjectHit(Ray(inter.point, toLight)).exists) return ambient;
	return ambient + inter.color * lambert(inter.n);
}

Intersection traceStep(struct Ray ray)
{
	Intersection inter = findNearestObjectHit(ray);
	if(!inter.exists) inter.color = background(ray.dir);
	inter.color = directLighting(ray.dir, inter);
	return inter;
}


void main()
{
	//camera
	float fov = 90.0;
	float tanFov = tan(fov / 2.0 * 3.14159 / 180.0) / iResolution.x;
	vec2 p = tanFov * (gl_FragCoord.xy * 2.0 - iResolution.xy);
	vec3 camP = vec3(0.0, 0.0, 0.0);
	vec3 camDir = normalize(vec3(p.x, p.y, 1.0));
	
	//primary ray
	Intersection inter = traceStep(Ray(camP, camDir));
	vec3 color = inter.color;
	if(inter.exists) 
	{
		//secondary ray - reflection
		vec3 r = reflect(camDir, inter.n);
		Intersection intRef = traceStep(Ray(inter.point, r));
		color += intRef.color * 0.8;
	}

	gl_FragColor = vec4(color, 1.0);
}


