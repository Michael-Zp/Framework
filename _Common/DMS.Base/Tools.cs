using System.Text;

namespace DMS.Base
{
	public static class Tools
	{
		public static string ToString(byte[] input)
		{
			return Encoding.UTF8.GetString(input);
		}
	}
}
