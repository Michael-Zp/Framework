#version 330

uniform vec2 iResolution;
uniform float iGlobalTime;

float quad(vec2 coord, vec2 lowerLeft, vec2 size)
{
	vec2 a = step(lowerLeft, coord);
	vec2 b = 1 - step(lowerLeft + size, coord);
	return a.x * b.x * a.y * b.y;
}

float repeatTent(float x, float width)
{
	return abs(mod(x, width) - 0.5 * width);
}

float repeatStep(float x, float width)
{
	return step(0.5 * width, mod(x, width));
}

float move(float t)
{
	return repeatTent(t, 2.0) * 0.1 * repeatStep(t + 1.0, 4.0);
}

void main() {
	//coordinates in range [0,1]
    vec2 coord01 = gl_FragCoord.xy/iResolution;
	
	vec2 coordA = coord01;
	coordA.y -= move(iGlobalTime + 2);
	coordA = mod(coordA, vec2(0.2, 0.2));
	float a = quad( coordA, vec2(0.0, 0.0), vec2(0.1, 0.1));
	vec2 coordB = coord01;
	coordB.x -= move(iGlobalTime + 4);
	coordB = mod(coordB, vec2(0.2, 0.2));
	float b = quad(coordB, vec2(0.0, 0.1), vec2(0.1, 0.1));
    float result = a+b;

    vec3 color = vec3(result);
	
    gl_FragColor = vec4(color, 1.0);
}
