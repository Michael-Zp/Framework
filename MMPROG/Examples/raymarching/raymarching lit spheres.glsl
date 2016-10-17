#version 330
uniform vec2 iResolution;

const float epsilon = 0.0001;
const int maxSteps = 128;

//repeat the given coordinate point every interval c[direction]
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

float sphere(vec3 point, vec3 center, float radius) 
{
    return length(point - center) - radius;
}

float distFunc(vec3 point){
	vec3 coordSphere = coordinateRep(point, vec3(10, 20, 20));
	return sphere(coordSphere, vec3(0, 0, 1), 0.3);
}

vec3 normalFunc(vec3 point){
	vec3 coordSphere = coordinateRep(point, vec3(10, 20, 20));
	return sphereNormal(coordSphere, vec3(0, 0, 1));
}

void main(){
	float fov = 80.0;
	float tanFov = tan(fov / 2.0 * 3.14159 / 180.0) / iResolution.x;
	vec2 p = tanFov * (gl_FragCoord.xy * 2.0 - iResolution.xy);

	vec3 camP = vec3(0, 0, 0);
	vec3 camDir = normalize(vec3(p.x, p.y, 1.0));
	
	vec3 point = camP; 	
	bool objectHit = false;
	float t = 0.0;
    for(int steps = 0; steps < maxSteps; ++steps)
    {
        float dist = distFunc(point);
        if(epsilon > dist)
        {
			objectHit = true;
            break;
        }
        t += dist;
        point = camP + t * camDir;
    }

	if(objectHit)
	{
		vec3 normal = normalFunc(point);
		vec3 lightPos = vec3(0);
		float diffuse = max(0, dot(normalize(point - lightPos), normal));
		gl_FragColor = vec4(diffuse * vec3(1), 1);
	}
	else
	{
		gl_FragColor = vec4(0, 0, 0, 1);
	}
}