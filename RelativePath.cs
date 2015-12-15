using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Framework
{
	public class RelativePath
	{
		/// <summary>
		/// returns the relative path. if no relative path is valid, the absolut path is returned.
		/// </summary>
		/// <param name="fromPath">the path the result should be relative to</param>
		/// <param name="toPath">the path to be converted into relative form</param>
		/// <returns></returns>
		public static string Get(string fromPath, string toPath)
		{
			if (string.IsNullOrEmpty(fromPath)) return toPath;
			int fromAttr = GetPathAttribute(fromPath);
			int toAttr = GetPathAttribute(toPath);

			StringBuilder path = new StringBuilder(5260); // todo: should we use MAX_PATH?
			if (0 == PathRelativePathTo(path, fromPath, fromAttr, toPath, toAttr))
			{
				return toPath;
			}
			return path.ToString();
		}

		private static int GetPathAttribute(string path)
		{
			DirectoryInfo di = new DirectoryInfo(path);
			if (di.Exists)
			{
				return FILE_ATTRIBUTE_DIRECTORY;
			}

			FileInfo fi = new FileInfo(path);
			if (fi.Exists)
			{
				return FILE_ATTRIBUTE_NORMAL;
			}

			throw new FileNotFoundException();
		}

		private const int FILE_ATTRIBUTE_DIRECTORY = 0x10;
		private const int FILE_ATTRIBUTE_NORMAL = 0x80;

		[DllImport("shlwapi.dll", SetLastError = true)]
		private static extern int PathRelativePathTo(StringBuilder pszPath,
			string pszFrom, int dwAttrFrom, string pszTo, int dwAttrTo);
	}
}
