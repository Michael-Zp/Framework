uniform sampler2D image;

in vec2 uv;

void main() {
	vec3 color = texture(image, uv).rgb;
	vec3 blurred = textureLod(image, uv, 4).rgb; //needs generate mipmap enabled to work
	color = 0.5 * color + 0.5 * blurred;
    gl_FragColor = vec4(color, 1.0);
}
