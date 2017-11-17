#version 420

#include "../libs/camera.glsl"
#include "../libs/rayIntersections.glsl"

uniform vec3 iMouse;
uniform vec2 iResolution;
uniform float iGlobalTime;
in vec2 uv;

const float bigNumber = 10000.0;
const float eps = 1e-5;
vec3 toLight = normalize(vec3(sin(iGlobalTime + 2.0), 0.6, cos(iGlobalTime + 2.0)));
const vec3 ambient = vec3(0);

struct Object
{
	vec4 data;
	int materialType; //-1 == background, 0 == plane, 1 == sphere
};

const int OBJECT_COUNT = 9;
struct Scene
{
	Object objects[OBJECT_COUNT];
};

Scene buildScene()
{
	const vec3 delta = vec3(-0.5, -.2, 0);
	Scene scene;
	int i = 0;
	for(float x = delta.x; x <= delta.x + 1.0; x += 1.0)
	{
		float y = delta.y;
		for(float z = delta.z + 1.0; z <= delta.z + 4.0; z += 1.0)
		{	
			vec3 newM = vec3(x, y, z);
			scene.objects[i] = Object(vec4(newM, 0.3), 1);
			++i;
		}
	}
	scene.objects[i] = Object(vec4(vec3(0.0, 1.0, 0.0), .5), 0);
	return scene;
};

Scene scene = buildScene();

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

vec3 sphereColor(vec3 M)
{
	return abs(normalize(M - vec3(1.0, 0.0, 2.0)));
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
	vec3 dirIn;
};

float intersect(Object obj, Ray ray)
{
	switch(obj.materialType)
	{
		case 0: //plane
			return plane(obj.data.xyz, obj.data.w, ray, eps);
		case 1: //sphere
			return sphere(obj.data.xyz, obj.data.w, ray, eps);
		default:
			return -bigNumber;
	}
}

vec3 objectColor(int id)
{
	Object obj = scene.objects[id];
	switch(obj.materialType)
	{
		case 0: //plane
			return vec3(.5);
		case 1: //sphere
			return sphereColor(obj.data.xyz);
		default:
			return vec3(0);
	}
}

vec3 normal(int id, vec3 point)
{
	Object obj = scene.objects[id];
	switch(obj.materialType)
	{
		case 0: //plane
			return obj.data.xyz;
		case 1: //sphere
			return sphereNormal(obj.data.xyz, point);
		default:
			return vec3(0);;
	}
}

Intersection findNearestObjectHit(Ray ray)
{
	float tMin = bigNumber;
	int idMin = -1;
	for(int id = 0; id < OBJECT_COUNT; ++id)
	{
		Object obj = scene.objects[id];
		float t = intersect(obj, ray);
		if(0 < t && t < tMin)
		{
			tMin = t;
			idMin = id;
		}
	}
	Intersection inter;
	inter.exists = tMin < bigNumber;
	if(inter.exists)
	{
		inter.point = ray.origin + tMin * ray.dir;
		inter.n = normal(idMin, inter.point);
		inter.color = objectColor(idMin);
		inter.dirIn = ray.dir;
	}
	return inter;
}

vec3 directLighting(Intersection inter)
{
	float inside = 0 < dot(inter.dirIn, inter.n) ? -1.0 : 1.0;
	vec3 stableRayO = inter.point + inside * inter.n * eps;
	//shadow ray
	if(findNearestObjectHit(Ray(stableRayO, toLight)).exists) return ambient;
	return ambient + inter.color * lambert(inter.n);
}

Intersection traceStep(Ray ray)
{
	Intersection inter = findNearestObjectHit(ray);
	if(!inter.exists) 
	{
		inter.color = background(ray.dir);
	}
	else 
	{
		inter.color = directLighting(inter);
	}
	return inter;
}

Intersection reflection(in Intersection inter)
{
	Intersection r = traceStep(Ray(inter.point + inter.n * eps, reflect(inter.dirIn, inter.n)));
	return r;
}

void main()
{
	vec3 camP = calcCameraPos();
	vec3 camDir = calcCameraRayDir(70.0, gl_FragCoord.xy, iResolution);
	
	//primary ray
	Intersection inter = traceStep(Ray(camP, camDir));
	vec3 color = inter.color;
	for(int i = 1; i <= 4; ++i)
	{
		if(inter.exists) 
		{
			//reflection
			inter = reflection(inter);
			color += inter.color * pow(.8, i);
		}	
	}

	gl_FragColor = vec4(color, 1.0);
}


