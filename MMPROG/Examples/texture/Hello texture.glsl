#version 330

uniform vec2 iResolution;
uniform float iGlobalTime;
uniform sampler2D tex;
		
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec2 uv = fragCoord / iResolution;
	// float Frequency = 100.0;
	// float Phase = iGlobalTime * 2.0;
	// float Amplitude = 0.01;
	// uv.y += sin(uv.x * Frequency  + 0) * Phase * 0.1;
	
	vec3 color = texture(tex, uv).rgb;
	fragColor = vec4(color, 1.0);
}

void main()
{
	mainImage(gl_FragColor, gl_FragCoord.xy);
}
