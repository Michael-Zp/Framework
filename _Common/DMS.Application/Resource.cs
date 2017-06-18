using System;

namespace DMS.Application
{
	public class Resource<RESOURCE_TYPE> //TODO: IResource<RESOURCE_TYPE> where RESOURCE_TYPE : class
	{
		public Resource(Func<RESOURCE_TYPE> creator)
		{
			this.creator = creator;
		}

		public bool IsValueCreated => null == resource;
	
		public RESOURCE_TYPE Value
		{
			get
			{
				if(!IsValueCreated)
				{
					resource = creator(); 
				}
				return resource;
			}
		}

		//public event EventHandler<RESOURCE_TYPE> Change;

		private RESOURCE_TYPE resource;
		private readonly Func<RESOURCE_TYPE> creator;
	}
}
