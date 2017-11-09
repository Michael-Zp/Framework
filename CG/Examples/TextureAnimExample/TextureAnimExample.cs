using Zenseless.OpenGL;
using Zenseless.Geometry;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using Zenseless.Application;
using Zenseless.HLGL;
using Zenseless.Base;

namespace Example
{
	class MyVisual
	{
		private SpriteSheetAnimation explosion;
		private SpriteSheetAnimation girlIdleRun;
		private SpriteSheetAnimation girlJumpBall;
		private SpriteSheetAnimation girlFight;
		private SpriteSheetAnimation girlDie;
		private SpriteSheetAnimation girlBack;
		private AnimationTextures alienShip;

		private MyVisual()
		{
			//animation using a single SpriteSheet
			explosion = new SpriteSheetAnimation(new SpriteSheet(TextureLoader.FromBitmap(Resourcen.explosion), 5, 5), 0, 24, 1f);

			//art from https://github.com/sparklinlabs/superpowers-asset-packs
			var spriteSheetGirl = new SpriteSheet(TextureLoader.FromBitmap(Resourcen.girl_2), 6, 7);
			girlIdleRun = new SpriteSheetAnimation(spriteSheetGirl, 0, 10, 1f);
			girlJumpBall = new SpriteSheetAnimation(spriteSheetGirl, 11, 20, 1f);
			girlFight = new SpriteSheetAnimation(spriteSheetGirl, 21, 25, 1f);
			girlDie = new SpriteSheetAnimation(spriteSheetGirl, 25, 32, 1f);
			girlBack = new SpriteSheetAnimation(spriteSheetGirl, 33, 36, 1f);

			//animation using a bitmap for each frame
			alienShip = new AnimationTextures(.5f);
			//art from http://millionthvector.blogspot.de/p/free-sprites.html
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10001));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10002));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10003));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10004));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10005));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10006));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10007));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10008));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10009));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10010));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10011));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10012));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10013));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10014));
			alienShip.AddFrame(TextureLoader.FromBitmap(Resourcen.alien10015));

			//for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D); //todo: remove if shader is used
		}

		private void Render(float absoluteTimeSeconds)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			//color is multiplied with texture color white == no change
			GL.Color3(Color.White);

			explosion.Draw(new Box2D(-.7f, .2f, .4f, .4f), absoluteTimeSeconds);
			alienShip.Draw(new Box2D(.3f, .2f, .4f, .4f), absoluteTimeSeconds);

			girlIdleRun.Draw(new Box2D(-1f, -.6f, .4f, .4f), absoluteTimeSeconds);
			girlJumpBall.Draw(new Box2D(-.6f, -.6f, .4f, .4f), absoluteTimeSeconds);
			girlFight.Draw(new Box2D( -.2f, -.6f, .4f, .4f), absoluteTimeSeconds);
			girlDie.Draw(new Box2D(.2f, -.6f, .4f, .4f), absoluteTimeSeconds);
			girlBack.Draw(new Box2D(.6f, -.6f, .4f, .4f), absoluteTimeSeconds);
		}

		[STAThread]
		private static void Main()
		{
			var window = new ExampleWindow();
			var visual = new MyVisual();
			var time = new GameTime();
			window.Render += () => visual.Render(time.Seconds);
			window.Run();
		}
	}
}