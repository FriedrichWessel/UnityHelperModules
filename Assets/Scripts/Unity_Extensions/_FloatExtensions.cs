using System;

public static class _FloatExtensions
{
	public static float ToRadians(this float degrees){
		return (float) (degrees * Math.PI / 180);
	}
	
	public static float ToDegrees(this float radians){
		return (float) (radians / Math.PI * 180);
	}
}


