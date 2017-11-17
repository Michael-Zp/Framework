using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Zenseless.Base
{
	/// <summary>
	/// Contains tools for saving image lists. 
	/// Intended to be used for recording of videos. Is used in ExampleWindow.
	/// </summary>
	public static class ImageListTools
	{
		/// <summary>
		/// Save a list of images to the directory given by PathTools.GetCurrentProcessOutputDir(true) 
		/// </summary>
		/// <param name="images">Images to save</param>
		public static void SaveToDefaultDir(this IEnumerable<Bitmap> images)
		{
			images.Save(PathTools.GetCurrentProcessOutputDir());
		}

		/// <summary>
		/// Save a list of images to a given directory.
		/// </summary>
		/// <param name="images">Images to save</param>
		/// <param name="directory">Directory to save to</param>
		public static void Save(this IEnumerable<Bitmap> images, string directory)
		{
			if (ReferenceEquals(null, images)) return;
			Directory.CreateDirectory(directory);
			var d = PathTools.IncludeTrailingPathDelimiter(directory);
			var time = DateTime.Now.ToString();
			int i = 0;
			foreach (var image in images)
			{
				image.Save($"{d}pic{i.ToString("0000")}.png");
				++i;
			}
		}
	}
}
