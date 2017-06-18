using System;

namespace DMS.Application
{
	public interface IResource<TYPE> where TYPE : class
	{
		bool IsValueCreated { get; }
		TYPE Value { get; }

		event EventHandler<TYPE> Change;
	}
}
