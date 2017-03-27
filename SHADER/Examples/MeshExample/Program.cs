using DMS.OpenGL;
using DMS.Geometry;
using OpenTK.Graphics.OpenGL;
using System;
using DMS.ShaderDebugging;
using DMS.System;
using System.IO;

namespace Example
{
	public class MyWindow : IWindow
	{
		public MyWindow()
		{
			var dir = Path.GetDirectoryName(PathTools.GetSourceFilePath()) + @"\Resources\";
			shaderWatcher = new ShaderFileDebugger(dir + "vertex.glsl", dir + "fragment.glsl"
				, Resourcen.vertex, Resourcen.fragment);
			CreateMesh(shaderWatcher.Shader);
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //draw wireframe

			//GL.Enable(EnableCap.DepthTest);
			//texDiffuse = TextureLoader.FromFile("diffuseTexture.jpg");
		}

		public void Render()
		{
			var shader = shaderWatcher.Shader;
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				CreateMesh(shader);
			}

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Activate();
			//texDiffuse.Activate();
			geometry.Draw();
			//texDiffuse.Deactivate();
			shader.Deactivate();
		}

		private void CreateMesh(Shader shader)
		{
			//load geometry
			Mesh mesh = Obj2Mesh.FromObj(Resourcen.suzanne);
			geometry.SetAttribute(shader.GetAttributeLocation("position"), mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			geometry.SetAttribute(shader.GetAttributeLocation("normal"), mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			//geometry.SetAttribute(shader.GetAttributeLocation("uv"), mesh.uvs.ToArray(), VertexAttribPointerType.Float, 2);
			geometry.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);
		}

		public void Update(float updatePeriod)
		{
		}

		private ShaderFileDebugger shaderWatcher;
		private VAO geometry = new VAO();
		//private Texture texDiffuse;

		[STAThread]
		public static void Main()
		{
			var app = new ExampleApplication();
			app.Run(new MyWindow());
		}
	}
}
