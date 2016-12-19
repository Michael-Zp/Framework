#version 330

#include "../libs/Noise.glsl"

uniform vec3 iMouse;
uniform vec2 iResolution;
uniform float iGlobalTime;

//fractal Brownian motion
float fBm(vec2 coord) {
	int octaves = 6;
    float value = 0;
    float amplitude = 0.5;
	float lacunarity = 2;
	float gain = 0.5;
    vec2 shift = vec2(100.0);
    // Rotate to reduce axial bias
    mat2 rot = mat2(cos(0.5), sin(0.5), 
                    -sin(0.5), cos(0.5));
    for (int i = 0; i < octaves; ++i) {
        value += amplitude * noise(coord);
        coord = rot * coord * lacunarity + shift;
        amplitude *= gain;
    }
    return value;
}

void main() {
    vec2 st = gl_FragCoord.xy/iResolution;
	st *= 10;

    vec3 color = vec3(0.0);
    vec2 q = vec2(0.);
    q.x = fBm( st + 0.00*iGlobalTime);
    q.y = fBm( st + vec2(1.0));

    vec2 r = vec2(0.);
    r.x = fBm( st + 1.0*q + vec2(1.7,9.2)+ 0.15*iGlobalTime );
    r.y = fBm( st + 1.0*q + vec2(8.3,2.8)+ 0.126*iGlobalTime);

    float f = fBm(st+r);

    color = mix(vec3(0.101961,0.619608,0.666667),
                vec3(0.666667,0.666667,0.498039),
                clamp((f*f)*4.0,0.0,1.0));

    color = mix(color,
                vec3(0,0,0.164706),
                clamp(length(q),0.0,1.0));

    color = mix(color,
                vec3(0.666667,1,1),
                clamp(length(r.x),0.0,1.0));

    gl_FragColor = vec4((f*f*f+.6*f*f+.5*f)*color,1.);
}
