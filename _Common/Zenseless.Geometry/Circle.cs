﻿using System;

namespace Zenseless.Geometry
{
	/// <summary>
	/// Represents a circle
	/// </summary>
	/// <seealso cref="System.IEquatable{Circle}" />
	[Serializable]
	public class Circle : IEquatable<Circle>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Circle"/> class.
		/// </summary>
		/// <param name="centerX">The center x.</param>
		/// <param name="centerY">The center y.</param>
		/// <param name="radius">The radius.</param>
		public Circle(float centerX, float centerY, float radius)
		{
			CenterX = centerX;
			CenterY = centerY;
			Radius = radius;
		}

		/// <summary>
		/// Gets or sets the center x.
		/// </summary>
		/// <value>
		/// The center x.
		/// </value>
		public float CenterX { get; set; }
		/// <summary>
		/// Gets or sets the center y.
		/// </summary>
		/// <value>
		/// The center y.
		/// </value>
		public float CenterY { get; set; }
		/// <summary>
		/// Gets or sets the radius.
		/// </summary>
		/// <value>
		/// The radius.
		/// </value>
		public float Radius { get; set; }

		/// <summary>
		/// Intersectses the specified circle.
		/// </summary>
		/// <param name="circle">The circle.</param>
		/// <returns></returns>
		public bool Intersects(Circle circle)
		{
			var rr = circle.Radius + Radius;
			rr *= rr;
			var xx = circle.CenterX - CenterX;
			xx *= xx;
			var yy = circle.CenterY - CenterY;
			yy *= yy;
			return rr > xx + yy;  
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator ==(Circle a, Circle b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator !=(Circle a, Circle b)
		{
			return !a.Equals(b);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
		/// </returns>
		public bool Equals(Circle other)
		{
			if (ReferenceEquals(null, other)) return false;
			return CenterX == other.CenterX && CenterY == other.CenterY && Radius == other.Radius;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object other)
		{
			return Equals(other as Circle);
		}

		/// <summary>
		/// A hash code produced out of hash codes of Radius and center.
		/// </summary>
		/// <returns>
		/// A hash code produced out of hash codes of Radius and center.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Radius.GetHashCode();
				hashCode = (hashCode * 397) ^ CenterX.GetHashCode();
				hashCode = (hashCode * 397) ^ CenterY.GetHashCode();
				return hashCode;
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return '(' + CenterX.ToString() + ',' + CenterY.ToString() + ';' + Radius.ToString() + ')';
		}
	}
}
