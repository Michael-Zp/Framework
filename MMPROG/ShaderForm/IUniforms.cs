using System;
using System.Collections.Generic;

namespace ShaderForm
{
	public interface IUniforms
	{
		IEnumerable<string> Names { get; }

		event EventHandler<string> OnAdd;
		event EventHandler<string> OnRemove;
		event EventHandler<string> OnChangeKeyframes;

		bool Add(string uniformName);
		IKeyFrames GetKeyFrames(string uniformName);
		void Remove(string uniformName);
	}
}