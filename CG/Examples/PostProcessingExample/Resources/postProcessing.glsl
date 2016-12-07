const float PI = 3.14159265359;

uniform sampler2D image;
uniform float iGlobalTime;

in vec2 uv;

//mat3 sharpen = mat3( 
//	0.0, -1.0, 0.0, 
//	-1.0, 5.0, -1.0, 
//	0.0, -1.0, 0.0 
//	);

mat3 sharpen = mat3( 
	0.0, 0.0, 0.0, 
	0.0, 1.0, 0.0, 
	0.0, 0.0, 0.0 
	);

void main()
{
	vec3 color = vec3(0);
	for (int i = 0; i < 3; ++i) 
	{
		for (int j = 0; j < 3; ++j) 
		{
			vec3 sample  = texelFetch(image, ivec2(gl_FragCoord) + ivec2(i - 1, j - 1), 0).rgb;
			color += sharpen[j, i] * sample;
		}
	}
	//color /= 9;

	gl_FragColor = vec4(color, 1.0);
}
