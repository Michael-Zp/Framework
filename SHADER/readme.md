## Examples
1. [MinimalShaderExample](/SHADER/Examples/MinimalShaderExample) 
1. [ShaderDebugExample](/SHADER/Examples/ShaderDebugExample)
1. [ShaderDebugDialogExample](/SHADER/Examples/ShaderDebugDialogExample)
1. [GeometryExample](/SHADER/Examples/GeometryExample)
1. [MeshExample](/SHADER/Examples/MeshExample)
1. [InstancingExample](/SHADER/Examples/InstancingExample)
1. [TransformationExample](/SHADER/Examples/TransformationExample)
1. [CameraTransformationExample](/SHADER/Examples/CameraTransformationExample)
1. [CameraExample](/SHADER/Examples/CameraExample)
1. [PhongLightingExample](/SHADER/Examples/PhongLightingExample)
1. [EnvMappingExample](/SHADER/Examples/EnvMappingExample)
1. [RenderToTextureExample](/SHADER/Examples/RenderToTextureExample)
1. [ShadowMappingExample](/SHADER/Examples/ShadowMappingExample)
1. [PhysicsExample](/SHADER/Examples/PhysicsExample)
1. [ParticleSystemExample](/SHADER/Examples/ParticleSystemExample)
<!--1. [SSBOExample](/SHADER/Examples/SSBOExample)-->

## Best Practices
#### Automatic conversion of the data type of a uniform
**Example:**
application code: `GL.Uniform1(shader.GetUniformLocation("time"), timeSource.Elapsed.TotalSeconds);`
shader code: `uniform float time;`

**Problem:** `timeSource.Elapsed.TotalSeconds` returns a `double`. the shader expects a `float`. 
does not work on for instance Intel HW, `uniform float time` stays on `0`.

**Solution:** explicit conversion: `GL.Uniform1(shader.GetUniformLocation("time"), (float)timeSource.Elapsed.TotalSeconds);`
