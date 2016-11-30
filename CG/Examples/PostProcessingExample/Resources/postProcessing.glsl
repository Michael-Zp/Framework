const float PI = 3.14159265359;

uniform sampler2D image;
uniform float iGlobalTime;

in vec2 uv;

void main () {
	// range [-1..1]²
    vec2 range11 = 2 * uv - 1;

	//cartesian to polar coordinates
    float radius = length(range11); // radius of current pixel
    float angle = atan(range11.y, range11.x); //angel of current pixel [-PI..PI] 

	float newAngle = angle + 0.3 * radius * sin(radius * 3 + iGlobalTime);

	float x = radius * cos(newAngle);
	float y = radius * sin(newAngle);

	vec2 newUv = (vec2(x, y) + 1) * 0.5;
    vec3 color = texture(image, newUv).rgb;  
    // Combine the offset colors.
    gl_FragColor = vec4(color, 1.0);
}


//void main() {
//	vec3 colorCenter = texture(image, uv).rgb;

////	if(brightness > 0.9)
//	//{
//		int kernelSize = 9;
//		int lodLevel = 0;
//		int kernelSize2 = (kernelSize - 1) / 2;
//		vec2 size = textureSize(image, lodLevel);
//		vec2 delta = uv / size;

//		vec3 colorSum = vec3(0);
//		int count = 0;
//		for(int x = -kernelSize2; x <= kernelSize2; ++x)
//		{
//			for(int y = -kernelSize2; y <= kernelSize2; ++y)
//			{
//				vec3 color = textureLod(image, uv + vec2(x, y) * delta, lodLevel).rgb;
//				float brightness = (color.r + color.g + color.b) / 3;
//				if(brightness > 0.9)
//				{
//					colorSum += color;
//					++count;
//				}
//			}
//		}
//		colorSum /= count;
//		//colorSum = mix(colorCenter, colorSum, brightness);
//		//color = blurColor;
//	//}
//    gl_FragColor = vec4(colorSum, 1.0);
//}
