using DMS.OpenGL;

namespace Example
{
	public class VisualBackground
	{
		public VisualBackground(Texture envMap)
		{
			this.envMap = envMap;
		}

		//public void ShaderChanged(string name, Shader shader)
		//{
		//	if (ShaderName != name) return;
		//	this.shader = shader;
		//	if (ReferenceEquals(shader, null)) return;
		//	var sphere = Meshes.CreateSphere(1, 4);
		//	var envSphere = sphere.SwitchTriangleMeshWinding();
		//	envSphere.Add(sphere);
		//	geometry = VAOLoader.FromMesh(envSphere, shader);
		//}

		private Texture envMap;
		//private VAO geometry;
	}
}
