float plane(vec3 point, vec3 normal, float d) {
    return dot(point, normal) - d;
}

float sphere(vec3 point, vec3 center, float radius) {
    return length(point - center) - radius;
}

float box(vec3 point, vec3 center, vec3 b )
{
  return length(max(abs(point - center) - b, vec3(0.0)));
}

float torus(vec3 point, vec2 t)
{
  vec2 q = vec2(length(point.xz) - t.x, point.y);
  return length(q) - t.y;
}

vec3 coordinateRep(vec3 point, vec3 c)
{
    return mod(point, c) - 0.5 * c;
}