using DMS.Geometry;
using DMS.OpenGL;
using DMS.ShaderDebugging;
using DMS.Base;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;

namespace Example
{
	public class MainVisual : IWindow
	{
		public CameraOrbit OrbitCamera { get { return camera; } }

		public MainVisual()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/Resources/";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.vert", dir + "fragment.frag"
				, Resourcen.vertex, Resourcen.fragment);

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
				UpdateGeometry(shaderWatcher.Shader);
			}
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			var shader = shaderWatcher.Shader;
			shader.Activate();
			GL.Uniform4(shader.GetUniformLocation("lightColor"), new Color4(1f, 1f, 1f, 1f));
			GL.Uniform3(shader.GetUniformLocation("lightPosition"), new Vector3(1, 1, 4));
			GL.Uniform4(shader.GetUniformLocation("ambientLightColor"), new Color4(.2f, .2f, .2f, 1f));
			GL.Uniform4(shader.GetUniformLocation("materialColor"), new Color4(1f, .5f, .5f, 1f));
			Matrix4 cam = camera.CalcMatrix().ToOpenTK();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), camera.CalcPosition().ToOpenTK());

			geometry.Draw();
			shader.Deactivate();
		}

		public void Update(float updatePeriod)
		{
		}

		private CameraOrbit camera = new CameraOrbit();
		private ShaderFileDebugger shaderWatcher;
		private VAO geometry;

		private void UpdateGeometry(Shader shader)
		{
			Mesh mesh = new Mesh();
			//mesh.Add(Meshes.CreateSphere(.7f, 3));
			mesh.Add(Obj2Mesh.FromObj(Resourcen.suzanne));
			geometry = VAOLoader.FromMesh(mesh, shader);
		}
	}
}
