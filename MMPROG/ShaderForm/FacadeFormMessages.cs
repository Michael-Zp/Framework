namespace ShaderForm
{
	public class FacadeFormMessages
	{
		public void Append(string message)
		{
			formLog.Append(message);
		}

		public void Clear()
		{
			formLog.Clear();
		}

		public void SaveLayout()
		{
			formLog.SaveData();
		}

		private FormMessages formLog = new FormMessages();
	}
}
