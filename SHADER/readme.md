### Setup
+ [notepad++ GLSL syntax highlighting and intelisense](https://github.com/danielscherzer/MMPROG/tree/master/notepad%2B%2B%20glsl%20extension)
+ [visual studio (incl. 2015) shader highlighting](http://www.horsedrawngames.com/shader-syntax-highlighting-in-visual-studio-2013/)


### Exercises
1. **MinimalShaderExample**: compile/run/tweak shader
1. **ShaderVBOExample**: get points to follow mouse cursor
1. **InstancingExample**: add instance attribute speed (an individual speed for each sphere) and move spheres in vertex shader
1. **MeshExample**: test [obj](http://www.scratchapixel.com/old/lessons/3d-advanced-lessons/obj-file-format/obj-file-format/) loading (only triangulated, single mesh models); load textured model; use instancing; 
1. **TransformationExample**: 
    1. get each head to rotate individually
    1. let heads orbit each other -> binary head systems
    1. solar systems of heads in a galaxy of heads; the big head-bang theory...
1. **CameraExample**: 
    1. account for the aspect ratio of your window
    1. create a camera class using LookAt or custom transformation
    1. use ortho instead of perspective
1. **PhysicsExample**:
    1. implement wind
    1. implement downward gravity (everything falls down onto a plane)
1. **LightingExample**:
    1. implement phong shading
    1. add a point light source
    1. add a spot light source
    1. animate light positions, directions, angles
    1. implement phong lighting
1. **LightingNPRExample**:
    1. implement Toon shading
    1. add cel shading
    1. implement Gooch lighting
1. **SSBOExample**:
	1. add individual particle color
	1. add reflect at window borders


### [Links and further reading](https://github.com/danielscherzer/Framework/blob/master/readme.md)


## Best Practices
#### Automatic conversion of the data type of a uniform
**Example:**
app code: `GL.Uniform1(shader.GetUniformLocation("time"), timeSource.Elapsed.TotalSeconds);`
shader code: `uniform float time;`

**Problem:** `timeSource.Elapsed.TotalSeconds` returns a `double`. the shader expects a `float`. 
does not work on for instance intel HW, `uniform float time` stays on `0`.

**Solution:** explicit conversion: `GL.Uniform1(shader.GetUniformLocation("time"), (float)timeSource.Elapsed.TotalSeconds);`
