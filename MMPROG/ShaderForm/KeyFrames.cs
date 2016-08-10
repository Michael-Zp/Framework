using System.Collections.Generic;
using System;
using System.Collections;
using Framework;

namespace ShaderForm
{
	public class KeyFrames: IKeyFrames
	{
		public event EventHandler<EventArgs> OnChange;

		public void AddUpdate(float time, float value)
		{
			var clippedTime = Math.Max(0.0f, time);
			keyframes.AddUpdate(clippedTime, value);
			CallOnChange();
		}

		public void Clear()
		{
			keyframes.Clear();
			CallOnChange();
		}

		public float Interpolate(float currentTime)
		{
			if (0 == keyframes.Count) return 0.0f;
			var pair = keyframes.FindPair(currentTime);
			//linear interpolation
			float valueDelta = pair.Item2 - pair.Item1;
			return pair.Item1 + pair.Item3 * valueDelta;
		}

		public IEnumerator<KeyValuePair<float, float>> GetEnumerator()
		{
			return keyframes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return keyframes.GetEnumerator();
		}

		protected void CallOnChange()
		{
			OnChange?.Invoke(this, EventArgs.Empty);
		}

		private Framework.ControlPoints<float> keyframes = new Framework.ControlPoints<float>();
	}
}
