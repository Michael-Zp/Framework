using System;
using System.Collections.Generic;

namespace ShaderForm
{
	class Event
	{
		public Event(string name, object sender, object arguments)
		{
			Name = name;
			Sender = sender;
			Arguments = arguments;
		}

		public string Name { get; private set; }
		public object Sender { get; private set; }
		public object Arguments { get; private set; }
	}

	public class EventQueue
	{
		public void Post(string name, object sender, object arguments)
		{
			events.Enqueue(new Event(name, sender, arguments));
		}

		public void ProcessEvents()
		{
			//create new queue now and work on old version, because Post could be called during processing
			var eventsCopy = events;
			events = new Queue<Event>();
			foreach (var aEvent in eventsCopy)
			{
				List<EventHandler<object>> listeners;
				if(eventListeners.TryGetValue(aEvent.Name, out listeners))
				{
					//listeners to aEvent exist; now call each handler
					foreach(var handler in listeners)
					{
						handler(aEvent.Sender, aEvent.Arguments);
					}
				}
			}
		}

		//public delegate void Del(object sender, object e);
		//internal void ListenTo(string eventName, Del handler)
		//{
		//	List<EventHandler<object>> listeners;
		//	if (!eventListeners.TryGetValue(eventName, out listeners))
		//	{
		//		listeners = new List<EventHandler<object>>();
		//		eventListeners.Add(eventName, listeners);
		//	}
		//	listeners.Add(handler);
		//}

		//public void ListenTo<Argument>(string eventName, EventHandler<Argument> handler)
		//{
		//	List<EventHandler<object>> listeners;
		//	if (!eventListeners.TryGetValue(eventName, out listeners))
		//	{
		//		listeners = new List<EventHandler<object>>();
		//		eventListeners.Add(eventName, listeners);
		//	}
		//	//listeners.Add(handler);
		//}

		//public void ListenTo(string eventName, Delegate handler)
		//{
		//	List<EventHandler<object>> listeners;
		//	if (!eventListeners.TryGetValue(eventName, out listeners))
		//	{
		//		listeners = new List<EventHandler<object>>();
		//		eventListeners.Add(eventName, listeners);
		//	}
		//	//listeners.Add(handler);
		//}

		//public void ListenTo(string eventName, EventHandler<object> handler)
		//{
		//	List<EventHandler<object>> listeners;
		//	if (!eventListeners.TryGetValue(eventName, out listeners))
		//	{
		//		listeners = new List<EventHandler<object>>();
		//		eventListeners.Add(eventName, listeners);
		//	}
		//	listeners.Add(handler);
		//}

		private Queue<Event> events = new Queue<Event>();
		private Dictionary<string, List<EventHandler<object>>> eventListeners = new Dictionary<string, List<EventHandler<object>>>(); 
	}
}
