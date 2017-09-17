using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Numerics;
using Zenseless.HLGL;
using Zenseless.OpenGL;
using System;

namespace Example
{
	public class RendererPoints
	{
		public RendererPoints()
		{
			geometry = new VAO();
			GL.Enable(EnableCap.ProgramPointSize);
			GL.Enable(EnableCap.PointSprite);
			shader = ShaderLoader.FromStrings(DefaultShader.VertexShaderParticle, DefaultShader.FragmentShaderPointCircle);
		}

		public void DrawPoints(Vector3[] points, float size, Color color)
		{
			shader.Activate();
			geometry.SetAttribute(shader.GetResourceLocation(ShaderResourceType.Attribute, "position"), points, VertexAttribPointerType.Float, 3); //copy data to gpu mem
			GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "pointSize"), resolutionMin * size);
			GL.Uniform4(shader.GetResourceLocation(ShaderResourceType.Uniform, "color"), color);
			geometry.Activate();
			GL.DrawArrays(PrimitiveType.Points, 0, points.Length); //draw
			geometry.Deactivate();
			shader.Deactivate();
		}

		public void Resize(int width, int height)
		{
			resolutionMin = Math.Min(width, height);
		}

		private IShader shader;
		private VAO geometry;
		private int resolutionMin;
	}
}
