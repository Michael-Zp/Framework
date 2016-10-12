uniform vec2 iResolution;
uniform float iGlobalTime;

float quad(float a)
{
	return a * a;
}

float sphere(vec3 M, vec3 O, vec3 d)
{
	float r = 0.1;
	vec3 MO = O - M;
	float root = quad(dot(d, MO))- quad(length(d)) * (quad(length(MO)) - quad(r));
	if(root < 0.001)
	{
		return -1000.0;
	}
	float p = -dot(d, MO);
	float q = sqrt(root);
    return (p - q) > 0.0 ? p - q : p + q;
}

void main()
{
	float fov = 90.0;
	float tanFov = tan(fov / 2.0 * 3.14159 / 180.0) / iResolution.x;
	vec2 p = tanFov * (gl_FragCoord.xy * 2.0 - iResolution.xy);

	vec3 camP = vec3(0.0, 0.0, 0.0);
	vec3 camDir = normalize(vec3(p.x, p.y, 1.0));

	float t = sphere(vec3(0, 0, 1), camP, camDir);

	vec3 color;
	if(t < 0)
	{
		color = vec3(0);
	}
	else
	{
		color = vec3(1);
	}
	gl_FragColor = vec4(color, 1.0);
}


