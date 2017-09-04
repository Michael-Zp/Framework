using Zenseless.Base;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace MvcSokoban
{
	internal class VisualLevel
	{
		public VisualLevel(IRenderContext context)
		{
			int i = 0;
			tileTypes.Add(ElementType.Floor, new Tuple<Bitmap, int>(Resourcen.GroundGravel_Grass, i++));
			tileTypes.Add(ElementType.Man, new Tuple<Bitmap, int>(Resourcen.Character4, i++));
			tileTypes.Add(ElementType.Box, new Tuple<Bitmap, int>(Resourcen.Crate_Brown, i++));
			tileTypes.Add(ElementType.Goal, new Tuple<Bitmap, int>(Resourcen.EndPoint_Red, i++));
			tileTypes.Add(ElementType.ManOnGoal, new Tuple<Bitmap, int>(Resourcen.EndPointCharacter, i++));
			tileTypes.Add(ElementType.BoxOnGoal, new Tuple<Bitmap, int>(Resourcen.EndPointCrate_Brown, i++));
			tileTypes.Add(ElementType.Wall, new Tuple<Bitmap, int>(Resourcen.Wall_Beige, i++));

			shdTexColor = context.CreateShader();
			shdTexColor.FromStrings(Tools.ToString(Resourcen.texColorVert), Tools.ToString(Resourcen.texColorFrag));

			levelGeometry = new VAO();
			var quadPos = new Vector2[4]
			{ Vector2.Zero, Vector2.UnitX, Vector2.One, Vector2.UnitY };
			var locPosition = shdTexColor.GetResourceLocation(ShaderResourceType.Attribute, "position");
			levelGeometry.SetAttribute(locPosition, quadPos, VertexAttribPointerType.Float, 2);

			locTexId = shdTexColor.GetResourceLocation(ShaderResourceType.Attribute, "texId");
			locCamera = shdTexColor.GetResourceLocation(ShaderResourceType.Uniform, "camera");
			locTint = shdTexColor.GetResourceLocation(ShaderResourceType.Uniform, "tint");

			texArray.Activate();
			GL.TexStorage3D(TextureTarget3d.Texture2DArray, 1, SizedInternalFormat.Rgba8, 64, 64, 7);
			foreach (var tile in tileTypes.Values)
			{
				LoadSubImage3D(tile.Item1, tile.Item2);
			}
			texArray.Filter = TextureFilterMode.Mipmap;
			texArray.WrapFunction = TextureWrapFunction.ClampToEdge;
			texArray.Deactivate();
		}

		internal void ResizeWindow(int width, int height)
		{
			windowAspect = width / (float)height;
		}

		internal void DrawLevelState(ILevel levelState, Color tint)
		{
			UpdateLevelGeometry(levelState);
			shdTexColor.Activate();
			texArray.Activate();
			var fitBox = Box2dExtensions.CreateContainingBox(levelState.Width, levelState.Height, windowAspect);
			var camera = Matrix4x4.CreateOrthographicOffCenter(fitBox.MinX, fitBox.MaxX, fitBox.MinY, fitBox.MaxY, 0, 1).ToOpenTK();

			GL.Uniform4(locTint, tint);
			GL.UniformMatrix4(locCamera, false, ref camera);

			levelGeometry.Activate();
			GL.DrawArraysInstanced(PrimitiveType.Quads, 0, 4, levelState.Width * levelState.Height);
			levelGeometry.Deactivate();

			texArray.Deactivate();
			shdTexColor.Deactivate();
		}

		private void LoadSubImage3D(Bitmap bitmap, int level)
		{
			var buffer = bitmap.ToBuffer();
			GL.TexSubImage3D(texArray.Target, 0, 0, 0, level, bitmap.Width, bitmap.Height, 1, PixelFormat.Bgra, PixelType.UnsignedByte, buffer);
		}

		private void UpdateLevelGeometry(ILevel levelState)
		{
			if (!ReferenceEquals(levelState, lastLevelState))
			{
				//move or different level size
				lastLevelState = levelState;
				var size = new Size(levelState.Width, levelState.Height);
				if (lastLevelSize != size)
				{
					//different level size -> create translates for tiles
					var instanceTranslate = new List<Vector2>();
					for (int x = 0; x < levelState.Width; ++x)
					{
						for (int y = 0; y < levelState.Height; ++y)
						{
							instanceTranslate.Add(new Vector2(x, y));
						}
					}
					var locInstanceTranslate = shdTexColor.GetResourceLocation(ShaderResourceType.Attribute, "instanceTranslate");
					levelGeometry.SetAttribute(locInstanceTranslate, instanceTranslate.ToArray(), VertexAttribPointerType.Float, 2, true);
					lastLevelSize = size;
				}
				//update all tile types
				var texId = new List<float>();
				for (int x = 0; x < levelState.Width; ++x)
				{
					for (int y = 0; y < levelState.Height; ++y)
					{
						var type = levelState.GetElement(x, y);
						texId.Add(tileTypes[type].Item2);
					}
				}
				levelGeometry.SetAttribute(locTexId, texId.ToArray(), VertexAttribPointerType.Float, 1, true);
			}
		}

		private ILevel lastLevelState;
		private Size lastLevelSize;
		private VAO levelGeometry;
		private IShader shdTexColor;
		private Texture texArray = new Texture(TextureTarget.Texture2DArray);
		private Dictionary<ElementType, Tuple<Bitmap, int>> tileTypes = new Dictionary<ElementType, Tuple<Bitmap, int>>();

		private int locCamera;
		private int locTint;
		private int locTexId;
		private float windowAspect;
	}
}
