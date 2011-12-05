using System;
using UnityEngine;

	public static class _Vector2Extensions
	{
		public static Vector2 Rotate(this Vector2 point, float degrees)
		{
			var radians = degrees.ToRadians();
			var x = (float) (Math.Cos(radians) * point.x - Math.Sin(radians) * point.y);
			var y = (float) (Math.Sin(radians) * point.x + Math.Cos(radians) * point.y);
			return new Vector2(x, y);
		}
	}


