#version 430 core

uniform sampler2D image;

in vec2 uv;

void main() 
{
	vec3 image = texture(image, uv).rgb;
	image = vec3(0.2126 * image.r + 0.7152 * image.g + 0.0722 * image.b);
	gl_FragColor = vec4(image, 1.0);
}