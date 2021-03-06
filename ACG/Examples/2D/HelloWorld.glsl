#version 330

uniform vec2 iResolution;
	
void main()
{
	//create uv to be in the range [0..1]x[0..1]
	vec2 uv = gl_FragCoord.xy / iResolution;
	//4 component color red, green, blue, alpha
	vec4 color =  vec4(0.7, 0.5, 0.3, 1); //line i
	// color.rgb = vec3(step(0.5, uv.x)); //line ii
	// color.rgb = vec3(smoothstep(0.35, 0.65, uv.x)); //line iii
	// color.rgb = vec3(step(0.5, uv.x) * step(0.5, uv.y)); //line iv
	// vec2 corner = step(vec2(0.5), uv);
	// color.rgb = vec3(corner.x * corner.y); //line v
	
	gl_FragColor = color;
}
