namespace Zenseless.HLGL
{
	public struct TypedHandle<TYPE>
	{
		public TypedHandle(int id)
		{
			ID = id;
		}

		public int ID { get; private set; }

		public bool IsNull => ID == NULL.ID;

		public static TypedHandle<TYPE> NULL;
	}
}
