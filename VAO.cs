using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Framework
{
	public class VAO : IDisposable
	{
		public VAO()
		{
			idVAO = GL.GenVertexArray();
		}

		public void Dispose()
		{
			foreach (uint id in bufferIDs)
			{
				GL.DeleteBuffer(id);
			}
			GL.DeleteVertexArray(idVAO);
			idVAO = 0;
		}

		public void SetID<Index>(Index[] data, PrimitiveType primitiveType) where Index : struct
		{
			Activate();
			uint indexBufferID;
			GL.GenBuffers(1, out indexBufferID);
			bufferIDs.Push(indexBufferID);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferID);
			var type = typeof(Index);
			int elementBytes = Marshal.SizeOf(type);
			int bufferByteSize = data.Length * elementBytes;
			// set buffer data
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)bufferByteSize, data, BufferUsageHint.StaticDraw);
			Deactive();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			DrawElementsType drawElementsType = GetDrawElementsType(type);
			idData = new IDData(primitiveType, data.Length, drawElementsType);
		}

		public void SetAttribute<DataElement>(int bindingID, DataElement[] data, VertexAttribPointerType type, int elementSize, bool perInstance = false) where DataElement : struct
		{
			Activate();
			uint bufferID;
			GL.GenBuffers(1, out bufferID);
			bufferIDs.Push(bufferID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
			int elementBytes = Marshal.SizeOf(typeof(DataElement));
			int bufferByteSize = data.Length * elementBytes;
			// set buffer data
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)bufferByteSize, data, BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(bindingID, elementSize, type, false, elementBytes, 0);
			GL.EnableVertexAttribArray(bindingID);

			if (perInstance)
			{
				GL.VertexAttribDivisor(bindingID, 1);
			}

			Deactive();
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DisableVertexAttribArray(bindingID);
		}

		public void Activate()
		{
			GL.BindVertexArray(idVAO);
		}

		public void Deactive()
		{
			GL.BindVertexArray(0);
		}

		public void Draw(int instanceCount = 1)
		{
			Activate();
			GL.DrawElementsInstanced(idData.primitiveType, idData.length, idData.drawElementsType, (IntPtr)0, instanceCount);
			Deactive();
		}

		private struct IDData
		{
			public DrawElementsType drawElementsType;
			public int length;
			public PrimitiveType primitiveType;

			public IDData(PrimitiveType primitiveType, int length, DrawElementsType drawElementsType)
			{
				this.primitiveType = primitiveType;
				this.length = length;
				this.drawElementsType = drawElementsType;
			}
		}

		private IDData idData;
		private int idVAO;
		private Stack<uint> bufferIDs = new Stack<uint>();

		private static DrawElementsType GetDrawElementsType(Type type)
		{
			if (type == typeof(ushort)) return DrawElementsType.UnsignedShort;
			if (type == typeof(uint)) return DrawElementsType.UnsignedInt;
			throw new Exception("Invalid index type");
		}
	}
}
