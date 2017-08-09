using DMS.Base;
using DMS.Geometry;
using DMS.HLGL;
using DMS.OpenGL;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace MvcSokoban
{
	internal class RendererGL4 : IRenderer
	{
		public RendererGL4(IRenderContext context)
		{
			levelGeometry = new VAO();
			shdTexColor = context.CreateShader();
			shdTexColor.FromStrings(Tools.ToString(Resourcen.texColorVert), Tools.ToString(Resourcen.texColorFrag));
			locPosition = shdTexColor.GetResourceLocation(ShaderResourceType.Attribute, "position");
			locCamera = shdTexColor.GetResourceLocation(ShaderResourceType.Uniform, "camera");
			locTint = shdTexColor.GetResourceLocation(ShaderResourceType.Uniform, "tint");

			//tileSet.Add(ElementType.Floor, TextureLoader.FromBitmap(Resourcen.GroundGravel_Grass));
			//tileSet.Add(ElementType.Man, TextureLoader.FromBitmap(Resourcen.Character4));
			//tileSet.Add(ElementType.Box, TextureLoader.FromBitmap(Resourcen.Crate_Brown));
			//tileSet.Add(ElementType.Goal, TextureLoader.FromBitmap(Resourcen.EndPoint_Red));
			//tileSet.Add(ElementType.ManOnGoal, TextureLoader.FromBitmap(Resourcen.EndPointCharacter));
			//tileSet.Add(ElementType.BoxOnGoal, TextureLoader.FromBitmap(Resourcen.EndPointCrate_Brown));
			//tileSet.Add(ElementType.Wall, TextureLoader.FromBitmap(Resourcen.Wall_Beige));
		}

		public void Clear()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		public void DrawLevel(ILevel level, Color tint)
		{
			if(level != lastLevel)
			{
				lastLevel = level;
				UpdateLevelGeometry(level);
			}
			shdTexColor.Activate();
			var fitBox = Box2dExtensions.CreateContainingBox(level.Width, level.Height, windowAspect);
			var camera = Matrix4x4.CreateOrthographicOffCenter(fitBox.MinX, fitBox.MaxX, fitBox.MinY, fitBox.MaxY, 0, 1).ToOpenTK();

			GL.Uniform4(locTint, tint);
			GL.UniformMatrix4(locCamera, false, ref camera);
			levelGeometry.Draw();
			shdTexColor.Deactivate();
		}

		private void UpdateLevelGeometry(ILevel level)
		{
			var pos = new List<Vector2>();
			var ids = new List<uint>();
			void CreatePos(float x, float y) => pos.Add(new Vector2(x, y));
			void CreateID(uint id) => ids.Add(id);
			Shapes.CreatePlane(0, level.Width, 0, level.Height, (uint)level.Width, (uint)level.Height
				, CreatePos, CreateID);
			levelGeometry.SetAttribute(locPosition, pos.ToArray(), VertexAttribPointerType.Float, 2);
			levelGeometry.SetID(ids.ToArray());
		}

		public void Print(string message, float size, TextAlignment alignment = TextAlignment.Left)
		{
		}

		public void ResizeWindow(int width, int height)
		{
			GL.Viewport(0, 0, width, height);
			windowAspect = width / (float)height;
		}

		private Dictionary<ElementType, ITexture> tileSet = new Dictionary<ElementType, ITexture>();
		private float windowAspect;
		private ILevel lastLevel;
		private VAO levelGeometry;
		private IShader shdTexColor;
		private int locPosition;
		private int locCamera;
		private int locTint;
	}
}