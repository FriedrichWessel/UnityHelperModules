using UnityEngine;
using System.Collections;

public static class _RectExtension  {
	
	public static Rect AddPosition(this Rect rect, Rect rect2){
		rect.x += rect2.x;
		rect.y += rect2.y;
		return rect;
	}
	
}
