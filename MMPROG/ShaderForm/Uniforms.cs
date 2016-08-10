using System;
using System.Collections.Generic;

namespace ShaderForm
{
	public class Uniforms : IUniforms
	{
		public event EventHandler<string> OnChangeKeyframes;
		public event EventHandler<string> OnAdd;
		public event EventHandler<string> OnRemove;

		public bool Add(string uniformName)
		{
			if(!UniformHelper.IsNameValid(uniformName)) return false;
			if (uniforms.ContainsKey(uniformName)) return true;
			try
			{
				var kf = new KeyFrames();
				kf.OnChange += (sender, arg) => { if (null != OnChangeKeyframes) OnChangeKeyframes(sender, uniformName); };
				uniforms.Add(uniformName, kf);
				if (null != OnAdd) OnAdd(this, uniformName);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void Clear()
		{
			if (null != OnRemove)
			{
				foreach (var uniformName in uniforms.Keys)
				{
					OnRemove(this, uniformName);
				}
			}
			uniforms.Clear();
		}

		public delegate void UniformCommand(string name, float value);
		public void Interpolate(float currentTime, UniformCommand command)
		{
			if (null == command) return;
			foreach (KeyValuePair<string, KeyFrames> item in uniforms)
			{
				var value = item.Value.Interpolate(currentTime);
				command(item.Key, value);
			}
		}

		public IEnumerable<string> Names { get { return uniforms.Keys; } }

		public IKeyFrames GetKeyFrames(string uniformName)
		{
			KeyFrames kfs;
			if (uniforms.TryGetValue(uniformName, out kfs)) return kfs;
			return null;
		}

		public void Remove(string uniformName)
		{
			uniforms.Remove(uniformName);
			if (null != OnRemove) OnRemove(this, uniformName);
		}

		private Dictionary<string, KeyFrames> uniforms = new Dictionary<string, KeyFrames>();
	}
}
