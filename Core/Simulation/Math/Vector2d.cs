﻿using UnityEngine;
using System.Collections;
using System;


namespace Lockstep
{
	[Serializable]
	public struct Vector2d
	{
		public long x;
		public long y;


	#region Constructors
		public Vector2d (long xFixed, long yFixed)
		{
			this.x = xFixed;
			this.y = yFixed;
		}

		public Vector2d (int xInt, int yInt)
		{
			this.x = xInt << FixedMath.SHIFT_AMOUNT;
			this.y = yInt << FixedMath.SHIFT_AMOUNT;
		}

		public Vector2d (Vector2 vec2)
		{
			this.x = FixedMath.Create (vec2.x);
			this.y = FixedMath.Create (vec2.y);
		}
	#endregion


	#region Local Math
		public void Subtract (ref Vector2d other)
		{
			this.x -= other.x;
			this.y -= other.y;
		}
		public void Add (ref Vector2d other)
		{
			this.x += other.x;
			this.y += other.y;
		}
		/// <summary>
		/// This vector's square magnitude.
		/// </summary>
		/// <returns>The magnitude.</returns>
		public long SqrMagnitude ()
		{
			return (this.x * this.x + this.y * this.y) >> FixedMath.SHIFT_AMOUNT;
		}
		/// <summary>
		/// This vector's magnitude.
		/// </summary>
		public long Magnitude ()
		{
			return FixedMath.Sqrt ((this.x * this.x + this.y * this.y) >> FixedMath.SHIFT_AMOUNT);
		}
		/// <summary>
		/// Normalize this vector.
		/// </summary>
		public void Normalize ()
		{
			long mag = this.Magnitude ();
			if (mag == 0)
			{
				return;
			}
			else if (mag == FixedMath.One)
			{
				return;
			}
			this.x = (this.x << FixedMath.SHIFT_AMOUNT) / mag;
			this.y = (this.y << FixedMath.SHIFT_AMOUNT) /  mag;
		}
		/// <summary>
		/// Lerp this vector to target by amount.
		/// </summary>
		/// <param name="target">target.</param>
		/// <param name="amount">amount.</param>
		public void Lerp (long targetx, long targety, long amount)
		{
			if (amount >= FixedMath.One) {
				this.x = targetx;
				this.y = targety;
				return;
			} else if (amount <= 0) {
				return;
			}
			this.x = targetx * amount + this.x * (1 - amount);
			this.y = targety * amount + this.y * (1 - amount);
		}
		public void Rotate (long sin, long cos)
		{
			long tempX = (this.x * cos + this.y * sin) >> FixedMath.SHIFT_AMOUNT;
			this.y = (this.x * -sin + this.y * cos) >> FixedMath.SHIFT_AMOUNT;
			this.x = tempX;
		}
		public void RotateRight ()
		{
			long tempX = this.x;
			this.x = -this.y;
			this.y = tempX;
		}
		public void Reflect (long axisX, long axisY)
		{
			long projection = this.Dot (axisX, axisY);
			long projX = (axisX * projection) >> FixedMath.SHIFT_AMOUNT;
			long projY = (axisY * projection) >> FixedMath.SHIFT_AMOUNT;
			this.x = projX + projX - this.x;
			this.y = projY + projY - this.y;
		}
		public void Reflect (long axisX, long axisY, long projection)
		{
			long projX = (axisX * projection) >> FixedMath.SHIFT_AMOUNT;
			long projY = (axisY * projection) >> FixedMath.SHIFT_AMOUNT;
			this.x = projX + projX - this.x;
			this.y = projY + projY - this.y;
		}
		public long Dot (long otherX, long otherY)
		{
			return (this.x * otherX + this.y * otherY) >> FixedMath.SHIFT_AMOUNT;
		}

		static long temp1;
		static long temp2;
		public long Distance (long otherX, long otherY)
		{
			temp1 = this.x - otherX;
			temp1 *= temp1;
			temp2 = this.y - otherY;
			temp2 *= temp2;
			return (FixedMath.Sqrt ((temp1 + temp2) >> FixedMath.SHIFT_AMOUNT));
		}
	#endregion

	#region Static Math
		public static Vector2d up = new Vector2d (0,1);
		public static Vector2d right = new Vector2d (1,0);
		public static Vector2d down = new Vector2d (0,-1);
		public static Vector2d left = new Vector2d (-1,0);
		public static Vector2d one = new Vector2d (1,1);
		public static Vector2d negative = new Vector2d (-1,-1);
		public static Vector2d zero = new Vector2d (0,0);
		public static long Dot (long v1x, long v1y, long v2x, long v2y)
		{
			return (v1x * v2x + v1y * v2y) >> FixedMath.SHIFT_AMOUNT;
		}
	#endregion

	#region Convert
		public override string ToString ()
		{
			return (
				"(" +
				Math.Round (FixedMath.ToDouble (this.x), 2, MidpointRounding.AwayFromZero).ToString () +
				", " +
				Math.Round (FixedMath.ToDouble (this.y), 2, MidpointRounding.AwayFromZero) +
				")"
				);
		}
		public Vector2 ToVector2 ()
		{
			return new Vector2 (
				(float)Math.Round(FixedMath.ToDouble(this.x),2, MidpointRounding.AwayFromZero),
				(float)Math.Round(FixedMath.ToDouble(this.y),2, MidpointRounding.AwayFromZero)
				);
		}
		public Vector3 ToVector3 (float y)
		{
			return new Vector3 (
				(float)FixedMath.ToDouble (this.x),
			    y,
				(float)FixedMath.ToDouble (this.y)
				);
		}

		#endregion

		#region Operators
		public static Vector2d operator + (Vector2d v1, Vector2d v2)
		{
			return new Vector2d (v1.x + v2.x, v1.y + v2.y);
		}
		public static Vector2d operator - (Vector2d v1, Vector2d v2)
		{
			return new Vector2d (v1.x - v2.x, v1.y - v2.y);
		}
		public static Vector2d operator * (Vector2d v1, long mag)
		{
			return new Vector2d ((v1.x * mag) >> FixedMath.SHIFT_AMOUNT, (v1.y * mag) >> FixedMath.SHIFT_AMOUNT);
		}
		public static Vector2d operator * (Vector2d v1, int mag)
		{
			return new Vector2d ((v1.x * mag), (v1.y * mag));
		}
		public static Vector2d operator / (Vector2d v1, long div)
		{
			return new Vector2d (((v1.x << FixedMath.SHIFT_AMOUNT) / div), (v1.y << FixedMath.SHIFT_AMOUNT)/ div);
		}
		public static Vector2d operator / (Vector2d v1, int div)
		{
			return new Vector2d ((v1.x / div), v1.y / div);
		}
		public static Vector2d operator >> (Vector2d v1, int shift)
		{
			return new Vector2d (v1.x >> shift, v1.y >> shift);
		}

		#endregion
	}


}
