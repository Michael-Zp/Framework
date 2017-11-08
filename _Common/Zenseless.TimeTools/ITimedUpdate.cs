namespace Zenseless.TimeTools
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITimedUpdate
	{
		/// <summary>
		/// Updates the specified absolute time.
		/// </summary>
		/// <param name="absoluteTime">The absolute time.</param>
		void Update(float absoluteTime);
	}
}
