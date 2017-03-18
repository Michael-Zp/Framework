## [Examples](Examples)

## Best Practices
#### Automatic conversion of the data type of a uniform
**Example:**
app code: `GL.Uniform1(shader.GetUniformLocation("time"), timeSource.Elapsed.TotalSeconds);`
shader code: `uniform float time;`

**Problem:** `timeSource.Elapsed.TotalSeconds` returns a `double`. the shader expects a `float`. 
does not work on for instance intel HW, `uniform float time` stays on `0`.

**Solution:** explicit conversion: `GL.Uniform1(shader.GetUniformLocation("time"), (float)timeSource.Elapsed.TotalSeconds);`
