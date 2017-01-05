#version 330

#include "../libs/camera.glsl"
#include "../libs/operators.glsl"
#include "../libs/Noise3D.glsl"

uniform vec2 iResolution;
uniform float iGlobalTime;

const float epsilon = 0.0001;
const int maxSteps = 128;

//fractal Brownian motion
float fBm(vec3 coord) 
{
	int octaves = 3;
    float value = 0;
    float amplitude = 0.5;
	float lacunarity = 2;
	float gain = 0.5;
    for (int i = 0; i < octaves; ++i) {
        value += amplitude * (snoise(coord) + 0.5);
        coord = coord * lacunarity;
        amplitude *= gain;
    }
    return value;
}
			
					
float cloudDensity(vec3 point)
{
	return fBm(point * 2) * 0.3;
}

void main()
{
	vec3 camP = calcCameraPos();
	camP.y += 2;
	vec3 camDir = calcCameraRayDir(80.0, gl_FragCoord.xy, iResolution);

	vec4 colorSum = vec4(0);
	float t = 0;
	//step along the ray 
    for(int steps = 0; (steps < maxSteps); ++steps)
    {
		//break if nearly opaque
		if(colorSum.a > 0.99) break;
		//calculate new point
        vec3 point = camP + t * camDir;
        float newDensity = cloudDensity(point);
		vec4 newColor = vec4(1) * newDensity;
		
		colorSum = colorSum * colorSum.a + newColor * (1 - colorSum.a);
		t += 0.005;
    }
	gl_FragColor = vec4(colorSum);
}