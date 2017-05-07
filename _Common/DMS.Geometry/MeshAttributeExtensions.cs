namespace DMS.Geometry
{
	public static class MeshAttributeExtensions
	{
		public static MeshAttribute<TYPE> Clone<TYPE>(this MeshAttribute<TYPE> attr)
		{
			var copy = new MeshAttribute<TYPE>(attr.Name);
			copy.List.AddRange(attr.List);
			return copy;
		}
	}
}
