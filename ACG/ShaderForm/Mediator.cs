using System;

namespace ShaderForm
{
	public class Mediator
	{
		public Mediator()
		{
			//eventQueue.ListenTo(nameof(UniformAdded), Mediator_UniformAdded);
			//eventQueue.ListenTo(nameof(UniformAdded), (sender, name) => UniformAdded?.Invoke(sender, name));
		}

		private void Mediator_UniformAdded(object sender, object e)
		{
			throw new NotImplementedException();
		}

		//public event EventHandler<string> UniformAdded;
		//public event EventHandler<string> UniformRemoved;
		//public event EventHandler<string> ChangedKeyframes;

		private EventQueue eventQueue = new EventQueue();
		private FacadeFormMessages log = new FacadeFormMessages();

		internal void ShowLog()
		{
			log.Show();
		}

		internal void Log(string message)
		{
			log.Append(message);
		}

		public void SaveLayout()
		{
			log.SaveLayout();
		}
	}
}
