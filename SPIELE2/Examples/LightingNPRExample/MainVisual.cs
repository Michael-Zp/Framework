using Framework;
using Geometry;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ShaderDebugging;
using System.Drawing;

namespace Example
{
	public class MainVisual
	{
		public CameraOrbit OrbitCamera { get { return camera; } }

		public MainVisual()
		{
			shaderWatcher = new ShaderFileDebugger("../../LightingNPRExample/Resources/vertex.vert"
				, "../../LightingNPRExample/Resources/fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);
			geometry = CreateMesh(shaderWatcher.Shader);

			camera.FarClip = 20;
			camera.Distance = 5;
			camera.FovY = 30;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.ClearColor(Color.White);
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				geometry = CreateMesh(shaderWatcher.Shader);
			}
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			var shader = shaderWatcher.Shader;
			shader.Begin();
			GL.Uniform4(shader.GetUniformLocation("lightColor"), new Color4(1f, 1f, 1f, 1f));
			GL.Uniform3(shader.GetUniformLocation("lightPosition"), new Vector3(1, 1, 4));
			GL.Uniform4(shader.GetUniformLocation("ambientLightColor"), new Color4(.2f, .2f, .2f, 1f));
			GL.Uniform4(shader.GetUniformLocation("materialColor"), new Color4(1f, .5f, .5f, 1f));
			Matrix4 cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), camera.CalcPosition().ToOpenTK());

			geometry.Draw();
			shader.End();
		}

		private CameraOrbit camera = new CameraOrbit();
		private ShaderFileDebugger shaderWatcher;
		private VAO geometry;

		private static VAO CreateMesh(Shader shader)
		{
			Mesh mesh = new Mesh();
			//mesh.Add(Meshes.CreateSphere(.7f, 3));
			mesh.Add(Obj2Mesh.FromObj(Resourcen.suzanne));
			var vao = new VAO();
			vao.SetAttribute(shader.GetAttributeLocation("position"), mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(shader.GetAttributeLocation("normal"), mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);
			return vao;
		}
	}
}
