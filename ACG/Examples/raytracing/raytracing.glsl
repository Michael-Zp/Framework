#version 420

#include "../libs/camera.glsl"
#include "../libs/rayIntersections.glsl"

uniform vec2 iResolution;
uniform float iGlobalTime;
in vec2 uv;

const float bigNumber = 10000.0;
const float eps = 1e-5;
vec3 toLight = normalize(vec3(sin(iGlobalTime + 2.0), 0.6, cos(iGlobalTime + 2.0)));
const vec3 lightColor = vec3(1, 1, 0.9);
const vec3 lightColorAmbient = vec3(0.15, 0.15, 0);

struct Object
{
	vec4 data;
	int type; //0 == plane, 1 == sphere
	int material; // 0 == diffuse, 1 = reflective
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
			scene.objects[i] = Object(vec4(newM, 0.3), 1, 1);
			++i;
		}
	}
	scene.objects[i] = Object(vec4(vec3(0.0, 1.0, 0.0), .5), 0, 0);
	return scene;
};

Scene scene = buildScene();

vec3 backgroundColor(vec3 dir)
{
	float sun = max(0.0, dot(dir, toLight));
	float sky = max(0.0, dot(dir, vec3(0.0, 1.0, 0.0)));
	float ground = max(0.0, -dot(dir, vec3(0.0, 1.0, 0.0)));
	return 
  (pow(sun, 256.0) + 0.2 * pow(sun, 2.0)) * vec3(2.0, 1.6, 1.0) +
  pow(ground, 0.5) * vec3(0.4, 0.3, 0.2) +
  pow(sky, 1.0) * vec3(0.5, 0.6, 0.7);
}

struct TraceState
{
	bool hitObject;
	vec3 n;
	vec3 color;
	vec3 point;
	vec3 dirIn;
};

float statesect(Object obj, Ray ray)
{
	switch(obj.type)
	{
		case 0: //plane
			return plane(obj.data.xyz, obj.data.w, ray, eps);
		case 1: //sphere
			return sphere(obj.data.xyz, obj.data.w, ray, eps);
		default:
			return -bigNumber;
	}
}

vec3 objectColor(TraceState state, int id)
{
	Object obj = scene.objects[id];
	switch(obj.type)
	{
		case 0: //plane
			vec2 p = floor(state.point.xz * 8.0);
			return mix(vec3(0.5), vec3(1), mod(p.x + p.y, 2.0));
		case 1: //sphere
			return abs(normalize(obj.data.xyz - vec3(1.0, 0.0, 2.0)));
		default:
			return vec3(0);
	}
}

vec3 normal(int id, vec3 point)
{
	Object obj = scene.objects[id];
	switch(obj.type)
	{
		case 0: //plane
			return obj.data.xyz;
		case 1: //sphere
			return sphereNormal(obj.data.xyz, point);
		default:
			return vec3(0);;
	}
}

struct Hit
{
	int objectId;
	float tMin;
};

Hit findNearestObjectHit(Ray ray)
{
	Hit hit = Hit(-1, bigNumber);
	for(int id = 0; id < OBJECT_COUNT; ++id)
	{
		Object obj = scene.objects[id];
		float t = statesect(obj, ray);
		if(0 < t && t < hit.tMin)
		{
			hit.tMin = t;
			hit.objectId = id;
		}
	}
	return hit;
}

vec3 directLighting(TraceState state)
{
	float inside = 0 < dot(state.dirIn, state.n) ? -1.0 : 1.0;
	vec3 stableRayO = state.point + inside * state.n * eps;
	//shadow ray
	vec3 ambient = lightColorAmbient * state.color;
	Hit hit = findNearestObjectHit(Ray(stableRayO, toLight));
	if(hit.tMin < bigNumber) return ambient;
	return ambient + lightColor * state.color * max(0, dot(state.n, toLight));
}

TraceState traceStep(Ray ray)
{
	Hit hit = findNearestObjectHit(ray);
	TraceState state;
	state.hitObject = hit.tMin < bigNumber;
	vec3 bckgroundColor = backgroundColor(ray.dir);
	if(state.hitObject) 
	{
		state.point = ray.origin + hit.tMin * ray.dir;
		state.n = normal(hit.objectId, state.point);
		state.dirIn = ray.dir;
		state.color = objectColor(state, hit.objectId);
		state.color = directLighting(state);
		// state.color = mix(directLighting(state), bckgroundColor, hit.tMin * .004);
	}
	else 
	{
		state.color = bckgroundColor;
	}
	return state;
}

TraceState reflection(in TraceState state)
{
	vec3 r = reflect(state.dirIn, state.n);
	vec3 stableRayO = state.point + state.n * eps;
	TraceState stateOut = traceStep(Ray(stableRayO, r));
	return stateOut;
}

// TraceState refraction(in TraceState state)
// {
	// vec3 t = refract(state.dirIn, state.n, 1.3);
	// vec3 stableRayO = state.point + state.n * eps;
	// TraceState stateOut = traceStep(Ray(stableRayO, t));
	// return stateOut;
// }

void main()
{
	vec3 camP = calcCameraPos();
	vec3 camDir = calcCameraRayDir(70.0, gl_FragCoord.xy, iResolution);
	
	//primary ray
	TraceState state = traceStep(Ray(camP, camDir));
	vec3 color = state.color;
	for(int i = 1; i <= 3; ++i)
	{
		if(state.hitObject) 
		{
			//reflection
			state = reflection(state);
			color += state.color * pow(.5, i);
		}	
	}

	gl_FragColor = vec4(color, 1.0);
}


