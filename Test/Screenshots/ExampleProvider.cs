using Zenseless.Application;
using Zenseless.Base;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Screenshots
{
	class ExampleProvider : Disposable
	{
		[ImportMany] public IEnumerable<IExample> Examples { get; private set; } = null;

		public ExampleProvider()
		{
			//var catalog = new AssemblyCatalog(typeof(Screenshots).Assembly);
			var catalog = new DirectoryCatalog(".", "*.exe");
			_container = new CompositionContainer(catalog);
			_container.SatisfyImportsOnce(this);
		}

		private CompositionContainer _container;

		protected override void DisposeResources()
		{
			_container.Dispose();
		}
	}
}
