namespace ShaderForm
{
	public class FacadeFormMessages
	{
		public void Append(string message)
		{
			//todo: different kind of log messages with different color (Richtextbox)
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
