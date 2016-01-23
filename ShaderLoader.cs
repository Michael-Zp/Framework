using System.IO;

namespace Framework
{
	public static class ShaderLoader
	{
		/// <summary>
		/// Loads vertex and fragment shaders from files.
		/// </summary>
		/// <param name="sVertexShdFile_">The s vertex SHD file_.</param>
		/// <param name="sFragmentShdFile_">The s fragment SHD file_.</param>
		/// <returns>a new instance</returns>
		public static Shader FromFiles(string sVertexShdFile_, string sFragmentShdFile_)
		{

			string sVertexShd = ShaderStringFromFileWithIncludes(sVertexShdFile_);
			string sFragmentShd = ShaderStringFromFileWithIncludes(sFragmentShdFile_);
			return Shader.LoadFromStrings(sVertexShd, sFragmentShd);
		}

		/// <summary>
		/// Reads the contents of a file into a string
		/// </summary>
		/// <param name="shaderFile">path to the shader file</param>
		/// <returns>string with contents of shaderFile</returns>
		public static string ShaderStringFromFileWithIncludes(string shaderFile)
		{
			string sShader = null;
			if (!File.Exists(shaderFile))
			{
				throw new FileNotFoundException("Could not find shader file '" + shaderFile + "'");
			}
			using (StreamReader sr = new StreamReader(shaderFile))
			{
				sShader = sr.ReadToEnd();
				//todo: handle includes
				return sShader;
			}
		}
	}
}
