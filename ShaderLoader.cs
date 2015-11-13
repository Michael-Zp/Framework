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
			string sVertexShd = null;
			if (!File.Exists(sVertexShdFile_))
			{
				throw new FileNotFoundException("Could not find " + sVertexShdFile_);
			}
			using (StreamReader sr = new StreamReader(sVertexShdFile_))
			{
				sVertexShd = sr.ReadToEnd();
			}
			string sFragmentShd = null;
			if (!File.Exists(sFragmentShdFile_))
			{
				throw new FileNotFoundException("Could not find " + sFragmentShdFile_);
			}
			using (StreamReader sr = new StreamReader(sFragmentShdFile_))
			{
				sFragmentShd = sr.ReadToEnd();
			}
			return Shader.LoadFromStrings(sVertexShd, sFragmentShd);
		}
	}
}
