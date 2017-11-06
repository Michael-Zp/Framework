using System;
using System.Collections.Generic;

namespace Zenseless.Geometry
{
	public interface IMeshAttribute<TYPE>
	{
		string Name { get; }
		List<TYPE> List { get; }
	}
}
