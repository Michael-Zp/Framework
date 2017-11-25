#version 330

#include "../libs/camera.glsl" 
// #include "../libs/noise3D.glsl" //uncomment for simplex noise: slower but more "fractal"

uniform float iGlobalTime;
uniform vec2 iResolution;

float time = iGlobalTime + 0.8;

// adapted from https://www.shadertoy.com/view/4sfGzS 

vec3 sundir = normalize( vec3(sin(time), 0.0, cos(time)) );

float hash(vec3 p)
{
    p  = fract( p*0.3183099 + .1 );
	p *= 17.0;
    return fract( p.x*p.y*p.z*(p.x+p.y+p.z) );
}

float noise( in vec3 x )
{
	x *= 2;
#ifdef noise3D_glsl
	return snoise(x * 0.25); //enable: slower but more "fractal"
#endif
    vec3 p = floor(x);
    vec3 f = fract(x);
    f = f*f*(3.0-2.0*f);
	
    return mix(mix(mix( hash(p+vec3(0,0,0)), 
                        hash(p+vec3(1,0,0)),f.x),
                   mix( hash(p+vec3(0,1,0)), 
                        hash(p+vec3(1,1,0)),f.x),f.y),
               mix(mix( hash(p+vec3(0,0,1)), 
                        hash(p+vec3(1,0,1)),f.x),
                   mix( hash(p+vec3(0,1,1)), 
                        hash(p+vec3(1,1,1)),f.x),f.y),f.z);
}

float fbm(vec3 p, const int octaves )
{
	float f = 0.0;
	float weight = 0.5;
	for(int i = 0; i < octaves; ++i)
	{
		f += weight * noise( p );
		weight *= 0.5;
		p *= 2.0;
	}
	return f;
}

float densityLayer(const vec3 p, const int octaves)
{
	vec3 q = p ; //+ vec3(0.0, 0.10, 1.0) * time; //cloud movement
	float f = fbm(q, octaves);
	return clamp( 1.5 - p.y - 2.0 + 1.75 * f, 0.0, 1.0 );
}

vec4 integrate( const vec4 sum, const float dif, const float den, const vec3 bgcol, const float t )
{
    // lighting
    vec3 lin = vec3(0.65, 0.7, 0.75) * 1.4 + vec3(1.0, 0.6, 0.3) * dif;        
    vec4 col = vec4( mix( vec3(1.0, 0.95, 0.8), vec3(0.25, 0.3, 0.35), den ), den );
    col.rgb *= lin;
	const float density = 0.003;
    col.rgb = mix( col.rgb, bgcol, 1.0 - exp( -density * t * t ) );
    // front to back blending    
    col.a *= 0.4;
    col.rgb *= col.a;
    return sum + col * ( 1.0 - sum.a );
}

#define MARCH(STEPS,MAPLOD) for(int i=0; i<STEPS; i++) { vec3  pos = ro + t*rd; if( pos.y<-3.0 || pos.y>2.0 || sum.a > 0.99 ) break; float den = MAPLOD( pos ); if( den>0.01 ) { float dif =  clamp((den - MAPLOD(pos+0.3*sundir))/0.6, 0.0, 1.0 ); sum = integrate( sum, dif, den, bgcol, t ); } t += max(0.05,0.02*t); }

float map5( const vec3 p ) { return densityLayer(p, 5); }
float map4( const vec3 p ) { return densityLayer(p, 4); }
float map3( const vec3 p ) { return densityLayer(p, 3); }
float map2( const vec3 p ) { return densityLayer(p, 2); }

vec4 raymarch( const vec3 ro, const vec3 rd, const vec3 bgcol )
{
	vec4 sum = vec4(0.0);

	float t = 0.0;

	int steps = 30;
    MARCH(steps, map5);
    MARCH(steps, map4);
    MARCH(steps, map3);
    MARCH(steps, map2);

    return clamp( sum, 0.0, 1.0 );
}

vec3 render(const vec3 ro, const vec3 rd )
{
    // background sky     
	float sun = clamp( dot( sundir, rd ), 0.0, 1.0 );
	vec3 backgroundSky = vec3( 0.7, 0.79, 0.83 )
		- rd.y * 0.2 * vec3( 1.0, 0.5, 1.0 )
		+ 0.2 * vec3( 1.0, .6, 0.1 ) * pow( sun, 8.0 );

    // clouds    
    vec4 res = raymarch( ro, rd, backgroundSky );
    vec3 col = backgroundSky * ( 1.0 - res.a ) + res.rgb; // blend clouds with sky
    
    // add sun glare    
	col += 0.2 * vec3( 1.0, 0.4, 0.2 ) * pow( sun, 3.0 );

    return col;
}

void main()
{
	vec3 camP = calcCameraPos();
	camP += vec3(7.4, 0.6, -4.8);
	vec3 camDir = calcCameraRayDir(80.0, gl_FragCoord.xy, iResolution);

    vec3 color = render( camP, camDir);
    gl_FragColor = vec4(color, 1.0 );
}



