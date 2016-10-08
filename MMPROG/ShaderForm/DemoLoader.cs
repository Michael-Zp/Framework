using System.IO;
using System.Linq;
using ShaderForm.DemoData2;
using Framework;
using System;

namespace ShaderForm
{
	public class ErrorEventArgs : EventArgs
	{
		public ErrorEventArgs(string message)
		{
			Message = message;
		}

		public bool Cancel { get; set; }
		public string Message { get; }
	}

	public delegate void ErrorEventHandler(object sender, ErrorEventArgs args);

	public class DemoLoader
	{
		public static void LoadFromFile(DemoModel demo, string fileName, ErrorEventHandler errorHandler = null)
		{
			try
			{
				LoadFromFile2(demo, fileName, errorHandler);
			}
			catch
			{
				//todo1: as soon as no version 1 files are needed get rid of DemoData...
				LoadFromFile1(demo, fileName, errorHandler);
			}
		}

		public static void SaveToFile(DemoModel demo, string fileName)
		{
			SaveToFile2(demo, fileName);
		}

		private static void SaveToFile2(DemoModel demo, string fileName)
		{
			var data = new DemoData2.DemoData2();
			Save(demo, data);
			data.ConvertToRelativePath(Path.GetDirectoryName(Path.GetFullPath(fileName)));
			data.ObjIntoXMLFile(fileName);
		}

		private static void LoadFromFile1(DemoModel demo, string fileName, ErrorEventHandler errorHandler)
		{
			var data = Serialize.ObjFromXMLFile(fileName, typeof(DemoData.DemoData)) as DemoData.DemoData;
			data.ConvertToAbsolutePath(Path.GetDirectoryName(Path.GetFullPath(fileName)));
			Load(data, demo, errorHandler);
		}

		private static void LoadFromFile2(DemoModel demo, string fileName, ErrorEventHandler errorHandler)
		{
			var data = Serialize.ObjFromXMLFile(fileName, typeof(DemoData2.DemoData2)) as DemoData2.DemoData2;
			data.ConvertToAbsolutePath(Path.GetDirectoryName(Path.GetFullPath(fileName)));
			Load(data, demo, errorHandler);
		}

		private static void Load(DemoData.DemoData data, DemoModel demo, ErrorEventHandler errorHandler)
		{
			demo.Clear();
			if (!LoadSound(data.SoundFileName, demo, errorHandler)) return;
			var ratios = data.ShaderRatios.Select((item) => new Tuple<float, string>(item.Ratio, item.ShaderPath));
			var keyframes = ShaderKeyframes.CalculatePosFromRatios(ratios, demo.TimeSource.Length);
			foreach (var kf in keyframes)
			{
				demo.Shaders.AddUpdateShader(kf.Item2);
				demo.ShaderKeyframes.AddUpdate(kf.Item1, kf.Item2);
			}

			foreach (var tex in data.Textures)
			{
				demo.Textures.AddUpdate(tex);
			}
			var uni = demo.Uniforms;
			foreach (var uniform in data.Uniforms)
			{
				demo.Uniforms.Add(uniform.UniformName);
				var kfs = demo.Uniforms.GetKeyFrames(uniform.UniformName);
				foreach (var kf in uniform.Keyframes)
				{
					kfs.AddUpdate(kf.Time, kf.Value);
				}
			}
		}

		private static void Load(DemoData2.DemoData2 data, DemoModel demo, ErrorEventHandler errorHandler)
		{
			demo.Clear();
			if (!LoadSound(data.SoundFileName, demo, errorHandler)) return;
			foreach (var track in data.Tracks)
			{
				//todo1: load track.Name;
				foreach (var shaderKeyframe in track.ShaderKeyframes)
				{
					demo.Shaders.AddUpdateShader(shaderKeyframe.ShaderPath);
					demo.ShaderKeyframes.AddUpdate(shaderKeyframe.Time, shaderKeyframe.ShaderPath);
				}
			}
			foreach (var tex in data.Textures)
			{
				demo.Textures.AddUpdate(tex);
			}
			var uni = demo.Uniforms;
			foreach (var uniform in data.Uniforms)
			{
				demo.Uniforms.Add(uniform.UniformName);
				var kfs = demo.Uniforms.GetKeyFrames(uniform.UniformName);
				foreach (var kf in uniform.Keyframes)
				{
					kfs.AddUpdate(kf.Time, kf.Value);
				}
			}
		}

		private static bool LoadSound(string soundFileName, DemoModel demo, ErrorEventHandler errorHandler)
		{
			if (!string.IsNullOrWhiteSpace(soundFileName))
			{
				var sound = DemoTimeSource.FromMediaFile(soundFileName);
				if (null == sound && null != errorHandler)
				{
					var args = new ErrorEventArgs("Could not load sound file '" + soundFileName + "'");
					errorHandler(demo, args);
					if (args.Cancel) return false;
				}
				demo.TimeSource.Load(sound);
			}
			return true;
		}

		private static void Save(DemoModel demo, DemoData2.DemoData2 data)
		{
			data.SoundFileName = demo.TimeSource.SoundFileName;
			data.Textures = demo.Textures.ToList();
			var track = new Track();
			track.Name = "sum";
			data.Tracks.Add(track);
			foreach (var element in demo.ShaderKeyframes.Items)
			{
				track.ShaderKeyframes.Add(new ShaderKeyframe(element.Key, element.Value));
			}
			foreach (var uniform in demo.Uniforms.Names)
			{
				var un = new Uniform(uniform, demo.Uniforms.GetKeyFrames(uniform));
				data.Uniforms.Add(un);
			}
		}
	}
}