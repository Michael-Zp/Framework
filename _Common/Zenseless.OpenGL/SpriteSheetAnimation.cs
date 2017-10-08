using Zenseless.Geometry;
using System;
using Zenseless.HLGL;

namespace Zenseless.OpenGL
{
	public class SpriteSheetAnimation : IAnimation
	{
		public SpriteSheetAnimation(SpriteSheet spriteSheet, uint fromID, uint toID, float animationLength)
		{
			this.spriteSheet = spriteSheet;
			FromID = fromID;
			ToID = toID;
			AnimationLength = animationLength;
		}

		public float AnimationLength { get; set; }
		public uint FromID { get; set; }
		public SpriteSheet spriteSheet { get; private set; }
		public uint ToID { get; set; }

		/// <summary>
		/// Calculates the sprite id (the current frame of the animation) out of the given time
		/// </summary>
		/// <param name="fromID">sprite id for first animation frame</param>
		/// <param name="toID">sprite id for last animation frame</param>
		/// <param name="animationLength">total animation time in seconds</param>
		/// <param name="time">current time</param>
		/// <returns>sprite id of the current frame of the animation</returns>
		public static uint CalcAnimationSpriteID(uint fromID, uint toID, float animationLength, float time)
		{
			float normalizedDeltaTime = (time % animationLength) / animationLength;
			float id = fromID + normalizedDeltaTime * (toID - fromID);
			return (uint)Math.Round(id);
		}

		/// <summary>
		/// draws a GL quad, textured with an animation.
		/// </summary>
		/// <param name="rectangle">coordinates ofthe GL quad</param>
		/// <param name="totalSeconds">animation position in seconds</param>
		public void Draw(Box2D rectangle, float totalSeconds)
		{
			var id = CalcAnimationSpriteID(FromID, ToID, AnimationLength, totalSeconds);
			var texCoords = spriteSheet.CalcSpriteTexCoords(id);
			spriteSheet.Activate();
			rectangle.DrawTexturedRect(texCoords);
			spriteSheet.Deactivate();
		}
	}
}
