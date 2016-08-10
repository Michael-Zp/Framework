//uniform vec3 iMouse;
uniform vec2 iResolution;
uniform float iGlobalTime;
uniform sampler2D tex;
in vec2 uv;
		
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec2 xy = uv;
	float Frequency = 100.0;
	float Phase = iGlobalTime * 2.0;
	float Amplitude = 0.01;
	xy.y += sin(xy.x * Frequency + Phase) * Amplitude;
	
	vec3 color = texture(tex, xy ).rgb;
	color = color;
	fragColor = vec4(color, 1.0);
}

void main()
{
	mainImage(gl_FragColor, gl_FragCoord.xy);
}
