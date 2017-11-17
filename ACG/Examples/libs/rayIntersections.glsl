struct Ray
{
	vec3 origin; // origin of ray
	vec3 dir; // direction of ray (unit length assumed)
};

//c = center of sphere
//r = radius of sphere
//return t of smaller hit point
float sphere(vec3 c, float r, Ray ray, float EPSILON)
{
	vec3 MO = ray.origin - c;
	float dotDirMO = dot(ray.dir, MO);
	float root = dotDirMO * dotDirMO - dot(ray.dir, ray.dir) * (dot(MO, MO) - r * r);
	if(root < EPSILON)
	{
		return -1.0;
	}
	float p = -dot(ray.dir, MO);
	float q = sqrt(root);
    return (p - q) > 0.0 ? p - q : p + q;
}

//center = center of sphere
//P = some point in space
// return normal of sphere in direction of P
vec3 sphereNormal(vec3 center, vec3 P)
{
	return normalize(P - center);
}

//n = normal of plane
//d = distance to origin
float plane(vec3 n, float d, Ray ray, float EPSILON)
{
	float denominator = dot(n, ray.dir);
	if(abs(denominator) < EPSILON)
	{
		//no intersection
		return -1.0;
	}
	return (-d-dot(n, ray.origin)) / denominator;
}
