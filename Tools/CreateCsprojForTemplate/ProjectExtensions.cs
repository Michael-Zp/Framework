using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using System.Linq;

namespace Tools
{
	public static class ProjectExtensions
	{
		public const string IdPackRef = "PackageReference";
		public const string IdProjRef = "ProjectReference";

		public static void RemovePackRef(this Project proj, string name)
		{
			var projRefs = from item in proj.Items
						   where IdPackRef == item.ItemType
						   where item.UnevaluatedInclude == name
						   select item;
			proj.RemoveItems(projRefs);
		}

		public static void RemoveProjRefs(this Project proj)
		{
			var projRefs = from item in proj.Items
						   where IdProjRef == item.ItemType
						   select item;
			proj.RemoveItems(projRefs);
		}

		public static void AddPackage(this Project proj, string name, string version)
		{
			var metaData = new Dictionary<string, string> { ["Version"] = version };
			proj.AddItem(IdPackRef, name, metaData);
		}
	}
}
