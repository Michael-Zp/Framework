﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Zenseless.Base
{
	/// <summary>
	/// Contains helper functions for file paths
	/// </summary>
	public static class PathTools
	{
		/// <summary>
		/// Returns the full path of the main module of the current process.
		/// </summary>
		/// <returns>Full path of the main module of the current process.</returns>
		public static string GetCurrentProcessPath()
		{
			return Process.GetCurrentProcess().MainModule.FileName;
		}

		/// <summary>
		/// Returns the directory of the main module of the current process.
		/// </summary>
		/// <returns>Directory of the main module of the current process.</returns>
		public static string GetCurrentProcessDir()
		{
			return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
		}

		/// <summary>
		/// Returns the absolute path for the specified path string by using Path.GetFullPath. If an exception is thrown the input parameter is returned.
		/// </summary>
		/// <param name="fileName">The file or directory for which to obtain absolute path information.</param>
		/// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
		public static string GetFullPath(string fileName)
		{
			try
			{
				return Path.GetFullPath(fileName);
			}
			catch
			{
				return fileName;
			}
		}

		/// <summary>
		/// Returns the relative path. if no relative path is valid, the absolut path is returned.
		/// </summary>
		/// <param name="fromPath">the path the result should be relative to</param>
		/// <param name="toPath">the path to be converted into relative form</param>
		/// <returns></returns>
		public static string GetRelativePath(string fromPath, string toPath)
		{
			if (string.IsNullOrEmpty(fromPath) || string.IsNullOrEmpty(toPath)) return toPath;
			try
			{
				int fromAttr = GetPathAttribute(fromPath);
				int toAttr = GetPathAttribute(toPath);

				StringBuilder path = new StringBuilder(5260); // MAX_PATH could be enough
				if (0 == SafeNativeMethods.PathRelativePathTo(path, fromPath, fromAttr, toPath, toAttr))
				{
					return toPath;
				}
				return path.ToString();
			}
			catch
			{
				return toPath;
			}
		}

		/// <summary>
		/// Returns the full path of the source file that contains the caller. This is the file path at the time of compile.
		/// </summary>
		/// <param name="doNotAssignCallerFilePath">Dummy default parameter. Needed for internal attribute evaluation. Do not assign.</param>
		/// <returns></returns>
		public static string GetSourceFilePath([CallerFilePath] string doNotAssignCallerFilePath = "")
		{
			return doNotAssignCallerFilePath;
		}

		/// <summary>
		/// IncludeTrailingPathDelimiter ensures that a path name ends with a trailing path delimiter ('\" on Windows, '/' on Linux). 
		/// If S already ends with a trailing delimiter character, it is returned unchanged; otherwise path with appended delimiter character is returned. 
		/// </summary>
		/// <param name="path">Input path</param>
		/// <returns>Input path with trailing path delimiter</returns>
		public static string IncludeTrailingPathDelimiter(string path)
		{
			var d = Path.DirectorySeparatorChar;
			return (d != path.Last()) ? path + d : path;
		}

		private const int FILE_ATTRIBUTE_DIRECTORY = 0x10;
		private const int FILE_ATTRIBUTE_NORMAL = 0x80;

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

		internal static class SafeNativeMethods
		{
			[DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
			internal static extern int PathRelativePathTo(StringBuilder pszPath,
				string pszFrom, int dwAttrFrom, string pszTo, int dwAttrTo);
		}
	}
}