#version 330

uniform vec2 iMouse;
uniform float iSeedRadius = 0.025;
uniform vec2 iResolution;
uniform float iGlobalTime;
uniform sampler2D buffer;

in vec2 uv;

int countNeighbors(vec2 p, bool isLive) 
{
	vec2 uvUnit = 1.0 / iResolution.xy;
	int count = 0;
	#define KERNEL_R 1
	for (int y = -KERNEL_R; y <= KERNEL_R; ++y)
	{
		for (int x = -KERNEL_R; x <= KERNEL_R; ++x) 
		{
			vec2 delta = uvUnit * vec2(float(x), float(y));
			if (0.0 < texture2D(buffer, uv + delta).a )
				++count;
		}
	}
	if (isLive)
		--count;
	return count;
}

float gameStep() {
	bool isLive = texture2D(buffer, uv).a > 0.0;
	int neighbors = countNeighbors(uv, isLive);
	
	//apply game rules:
	//living bacteria keep on lifing if the have not less than 2 and not more than 3 neighbors
	//new bacteria start to life if they have exactly 3 neighbors
	if (isLive)
	{
		return ( (2 == neighbors) || (3 == neighbors) ) ? 1.0 : 0.0;
	}
	else 
	{
		return (3 == neighbors) ? 1.0 : 0.0;
	}
}

float seedValue()
{
	// here pixels of a circle
	float aspect = iResolution.x / iResolution.y;
	vec2 pos = uv;
	pos.x *= aspect;
	vec2 pmouse = iMouse;
	pmouse.x *= aspect;
	return distance(pmouse, pos) < iSeedRadius ? 1.0 : 0.0;
}

void main() 
{
	float live = seedValue() + gameStep();
	// draw out
	vec3 color = vec3(0.3, 0.4, 0.6) * live;
	//ghosting
	color += 0.99 * texture2D(buffer, uv).rgb;
	color -= 1.0 / 256.0; //dim over time
	gl_FragColor = vec4(color, live);
}
