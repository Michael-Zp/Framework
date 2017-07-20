namespace DMS.HLGL
{
	public struct TypedHandle<TYPE>
	{
		public TypedHandle(int id)
		{
			ID = id;
		}

		public int ID { get; private set; }
	}
}
