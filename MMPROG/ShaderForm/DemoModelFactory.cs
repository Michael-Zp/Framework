using System.ComponentModel;

namespace ShaderForm
{
	public static class DemoModelFactory
	{
		public static DemoModel Create(ISynchronizeInvoke syncObject)
		{
			var visualContext = new VisualContext();
			var shaders = new Shaders(visualContext, () => new ShaderFile(visualContext, syncObject));
			var textures = new Textures(visualContext);
			return new DemoModel(visualContext, shaders, textures, true);
		}
	}
}
