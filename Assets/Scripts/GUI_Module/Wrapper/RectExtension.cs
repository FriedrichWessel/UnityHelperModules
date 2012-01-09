using UnityEngine;
using System.Collections;

public static class RectExtension  {
	
	public static Rect AddPosition(this Rect rect, Rect rect2){
		rect.x += rect2.x;
		rect.y += rect2.y;
		return rect;
	}
	
	public static Rect AddPosition(this Rect rect, Vector2 position){
		rect.x += position.x;
		rect.y += position.y;
		return rect;
	}
}
