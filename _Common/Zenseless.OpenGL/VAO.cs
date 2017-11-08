using System;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;
using Zenseless.Base;

namespace Zenseless.OpenGL
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="System.Exception" />
	[Serializable]
	public class VAOException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VAOException" /> class.
		/// </summary>
		/// <param name="msg">The error msg.</param>
		public VAOException(string msg) : base(msg) { }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.Base.Disposable" />
	public class VAO : Disposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VAO"/> class.
		/// </summary>
		public VAO()
		{
			idVAO = GL.GenVertexArray();
		}

		/// <summary>
		/// Gets the length of the identifier.
		/// </summary>
		/// <value>
		/// The length of the identifier.
		/// </value>
		public int IDLength { get; private set; } = 0;
		/// <summary>
		/// Gets or sets the type of the primitive.
		/// </summary>
		/// <value>
		/// The type of the primitive.
		/// </value>
		public PrimitiveType PrimitiveType { get; set; } = PrimitiveType.Triangles;
		/// <summary>
		/// Gets the type of the draw elements.
		/// </summary>
		/// <value>
		/// The type of the draw elements.
		/// </value>
		public DrawElementsType DrawElementsType { get; private set; } = DrawElementsType.UnsignedShort;

		/// <summary>
		/// Sets the index.
		/// </summary>
		/// <typeparam name="IndexType">The type of the ndex type.</typeparam>
		/// <param name="data">The data.</param>
		public void SetIndex<IndexType>(IndexType[] data) where IndexType : struct
		{
			if (ReferenceEquals(null, data)) return;
			if (0 == data.Length) return;
			Activate();
			var buffer = RequestBuffer(idBufferBinding, BufferTarget.ElementArrayBuffer);
			// set buffer data
			buffer.Set(data, BufferUsageHint.StaticDraw);
			//activate for state
			buffer.Activate();
			//cleanup state
			Deactivate();
			buffer.Deactivate();
			//save data for draw call
			DrawElementsType drawElementsType = GetDrawElementsType(typeof(IndexType));
			IDLength = data.Length;
			DrawElementsType = drawElementsType;
		}

		/// <summary>
		/// Sets the attribute.
		/// </summary>
		/// <typeparam name="DataElement">The type of the ata element.</typeparam>
		/// <param name="bindingID">The binding identifier.</param>
		/// <param name="data">The data.</param>
		/// <param name="type">The type.</param>
		/// <param name="elementSize">Size of the element.</param>
		/// <param name="perInstance">if set to <c>true</c> [per instance].</param>
		public void SetAttribute<DataElement>(int bindingID, DataElement[] data, VertexAttribPointerType type, int elementSize, bool perInstance = false) where DataElement : struct
		{
			if (-1 == bindingID) return; //if attribute not used in shader or wrong name
			Activate();
			var buffer = RequestBuffer(bindingID, BufferTarget.ArrayBuffer);
			buffer.Set(data, BufferUsageHint.StaticDraw);
			//activate for state
			buffer.Activate();
			//set data format
			int elementBytes = Marshal.SizeOf(typeof(DataElement));
			GL.VertexAttribPointer(bindingID, elementSize, type, false, elementBytes, 0);
			GL.EnableVertexAttribArray(bindingID);
			if (perInstance)
			{
				GL.VertexAttribDivisor(bindingID, 1);
			}
			//cleanup state
			Deactivate();
			buffer.Deactivate();
			GL.DisableVertexAttribArray(bindingID);
		}

		/// <summary>
		/// sets or updates a vertex attribute of type Matrix4
		/// Matrix4 is stored row-major, but OpenGL expects data to be column-major, so the Matrix4 inputs become transposed in the shader
		/// </summary>
		/// <param name="bindingID">shader binding location</param>
		/// <param name="data">array of Matrix4 inputs</param>
		/// <param name="perInstance">if set to <c>true</c> [per instance].</param>
		public void SetAttribute(int bindingID, Matrix4[] data, bool perInstance = false)
		{
			if (-1 == bindingID) return; //if matrix not used in shader or wrong name
			Activate();
			var buffer = RequestBuffer(bindingID, BufferTarget.ArrayBuffer);
			// set buffer data
			buffer.Set(data, BufferUsageHint.StaticDraw);
			//activate for state
			buffer.Activate();
			//set data format
			int columnBytes = Marshal.SizeOf(typeof(Vector4));
			int elementBytes = Marshal.SizeOf(typeof(Matrix4));
			for (int i = 0; i < 4; i++)
			{
				GL.VertexAttribPointer(bindingID + i, 4, VertexAttribPointerType.Float, false, elementBytes, columnBytes * i);
				GL.EnableVertexAttribArray(bindingID + i);
				if (perInstance)
				{
					GL.VertexAttribDivisor(bindingID + i, 1);
				}
			}
			//cleanup state
			Deactivate();
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			for (int i = 0; i < 4; i++)
			{
				GL.DisableVertexAttribArray(bindingID + i);
			}
		}

		/// <summary>
		/// Activates this instance.
		/// </summary>
		public void Activate()
		{
			GL.BindVertexArray(idVAO);
		}

		/// <summary>
		/// Deactivates this instance.
		/// </summary>
		public void Deactivate()
		{
			GL.BindVertexArray(0);
		}

		/// <summary>
		/// Draws the arrays.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="count">The count.</param>
		/// <param name="start">The start.</param>
		public void DrawArrays(PrimitiveType type, int count, int start = 0)
		{
			Activate();
			GL.DrawArrays(type, start, count);
			Deactivate();
		}

		/// <summary>
		/// Draws the specified instance count.
		/// </summary>
		/// <param name="instanceCount">The instance count.</param>
		/// <exception cref="Zenseless.OpenGL.VAOException">Empty id data set! Draw yourself using active/deactivate!</exception>
		public void Draw(int instanceCount = 1)
		{
			if (0 == IDLength) throw new VAOException("Empty id data set! Draw yourself using active/deactivate!");
			Activate();
			GL.DrawElementsInstanced(PrimitiveType, IDLength, DrawElementsType, (IntPtr)0, instanceCount);
			Deactivate();
		}

		/// <summary>
		/// The identifier vao
		/// </summary>
		private int idVAO;
		/// <summary>
		/// The identifier buffer binding
		/// </summary>
		private const int idBufferBinding = int.MaxValue;
		/// <summary>
		/// The bound buffers
		/// </summary>
		private Dictionary<int, BufferObject> boundBuffers = new Dictionary<int, BufferObject>();

		/// <summary>
		/// Gets the type of the draw elements.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		/// <exception cref="Zenseless.OpenGL.VAOException">Invalid index type</exception>
		private static DrawElementsType GetDrawElementsType(Type type)
		{
			if (type == typeof(byte)) return DrawElementsType.UnsignedByte;
			if (type == typeof(ushort)) return DrawElementsType.UnsignedShort;
			if (type == typeof(uint)) return DrawElementsType.UnsignedInt;
			throw new VAOException("Invalid index type");
		}

		/// <summary>
		/// Requests the buffer.
		/// </summary>
		/// <param name="bindingID">The binding identifier.</param>
		/// <param name="bufferTarget">The buffer target.</param>
		/// <returns></returns>
		private BufferObject RequestBuffer(int bindingID, BufferTarget bufferTarget)
		{
			if (!boundBuffers.TryGetValue(bindingID, out BufferObject buffer))
			{
				buffer = new BufferObject(bufferTarget);
				boundBuffers[bindingID] = buffer;
			}
			return buffer;
		}

		/// <summary>
		/// Will be called from the default Dispose method.
		/// </summary>
		protected override void DisposeResources()
		{
			foreach (var buffer in boundBuffers.Values)
			{
				buffer.Dispose();
			}
			boundBuffers.Clear();
			GL.DeleteVertexArray(idVAO);
			idVAO = 0;
		}
	}
}
