using OpenTK.Graphics.OpenGL;
using System;

namespace Framework
{
	public class QueryObject : IDisposable
	{
		public QueryObject()
		{
			GL.GenQueries(1, out id);
		}

		public void Begin(QueryTarget target)
		{
			Target = target;
			GL.BeginQuery(target, id);
		}

		public void End()
		{
			GL.EndQuery(Target);
		}
		public void Dispose()
		{
			GL.DeleteQueries(1, ref id);
		}

		public bool IsFinished
		{
			get
			{
				int isFinished;
				GL.GetQueryObject(id, GetQueryObjectParam.QueryResultAvailable, out isFinished);
				return 1 == isFinished;
			}
		}

		public int Result
		{
			get
			{
				int result;
				GL.GetQueryObject(id, GetQueryObjectParam.QueryResult, out result);
				return result;
			}
		}

		public QueryTarget Target { get; private set; }

		public bool TryGetResult(out int result)
		{
			result = -1;
			GL.GetQueryObject(id, GetQueryObjectParam.QueryResultNoWait, out result);
			return -1 != result;
		}

		private int id;
	}
}
