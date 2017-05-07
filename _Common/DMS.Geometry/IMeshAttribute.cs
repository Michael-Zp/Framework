using System.Collections.Generic;

namespace DMS.Geometry
{
	interface IMeshAttribute<TYPE>
	{
		string Name { get; }
		List<TYPE> List { get; }
	}
}
