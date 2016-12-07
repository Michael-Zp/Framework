const float PI = 3.14159265359;

uniform sampler2D image;
uniform float iGlobalTime;

in vec2 uv;

mat3 sx = mat3( 
	1.0, 2.0, 1.0, 
	0.0, 0.0, 0.0, 
	-1.0, -2.0, -1.0 
	);

mat3 sy = mat3( 
	1.0, 0.0, -1.0, 
	2.0, 0.0, -2.0, 
	1.0, 0.0, -1.0 
	);

float grayScale(vec3 color)
{
	return 0.2126 * color.r + 0.7152 * color.g + 0.0722 * color.b;
}

void main()
{
	mat3 I;
	for (int i = 0; i < 3; ++i) 
	{
		for (int j=0; j<3; ++j) 
		{
			vec3 sample  = texelFetch(image, ivec2(gl_FragCoord) + ivec2(i-1,j-1), 0 ).rgb;
			I[i][j] = grayScale(sample);
		}
	}

	float gx = dot(sx[0], I[0]) + dot(sx[1], I[1]) + dot(sx[2], I[2]);
	float gy = dot(sy[0], I[0]) + dot(sy[1], I[1]) + dot(sy[2], I[2]);
	float g = sqrt(pow(gx, 2.0)+pow(gy, 2.0));
	gl_FragColor = vec4(vec3(g), 1.0);
}
