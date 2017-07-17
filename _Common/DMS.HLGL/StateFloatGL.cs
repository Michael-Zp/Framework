namespace DMS.HLGL
{
	public class StateFloatGL : IStateFloat
	{
		private float value = 0f;

		public float Value { get => value; set => this.value = value; }
	}
}
