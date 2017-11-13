uniform vec2 iResolution;
uniform float iGlobalTime;

const float bigNumber = 10000.0;
const float eps = 0.001;
const float PI = 3.14159;

float quad(float a)
{
	return a * a;
}

//C = center of sphere
//r = radius of sphere
//O = origin of ray
//D = direction of ray (with unit length)
//return t of smaller hit point
float sphere(vec3 C, float r, vec3 O, vec3 D)
{
	vec3 V = O - C;
	float dotVD = dot(V, D);
	float root = quad(dotVD) - (quad(length(V)) - quad(r));
	//does ray miss the sphere?
	if(root < eps)
	{
		//return something negative
		return -bigNumber;
	}
	//ray hits the sphere -> calc t of hit point(s)
	float p = -dotVD;
	float q = sqrt(root);
    return (p - q) > 0.0 ? p - q : p + q;
}

//center = center of sphere
//P = some point in space
// return normal of sphere when looking from point P
vec3 sphereNormal(vec3 center, vec3 P)
{
	return normalize(P - center);
}

//N = normal of plane
//k = distance to origin
//O = origin of ray
//D = direction of ray
float plane(vec3 N, float k, vec3 O, vec3 D)
{
	float denominator = dot(N, D);
	if(abs(denominator) < eps)
	{
		//no intersection
		return -bigNumber;
	}
	return (-k - dot(N, O)) / denominator;
}	

void main()
{
	//camera setup
	float fov = 90.0 * (PI / 180.0);
	float fx = tan(fov / 2) / iResolution.x;
	vec2 d = fx * (gl_FragCoord.xy * 2.0 - iResolution.xy);

	vec3 camP = vec3(0.0, 0.0, 0.0);
	vec3 camDir = normalize(vec3(d.x, d.y, 1.0));
	
	//intersection
	vec3 C = vec3(0, 0, 1);
	float t = sphere(C, 0.4, camP, camDir);

	//final color
	vec3 color;
	if(t < 0)
	{
		//background
		color = vec3(0);
	}
	else
	{
		color = vec3(1);

		
		//sphere diffuse coloring
		vec3 normal = sphereNormal(C, camP + t * camDir);
		color = normal;
		// color = vec3(dot(normal,normalize(vec3(1, 1, -1))));
	}
	// gl_FragColor = vec4(camDir, 1.0);
	gl_FragColor = vec4(color, 1.0);
}


