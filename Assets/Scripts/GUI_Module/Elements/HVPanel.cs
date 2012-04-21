using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HVPanel : Panel {

	public Frame.ElementOrientation Orientation;
	
	public Vector2 ElementOffset;
	public float ElementSpacing;
	
	protected override void AwakeOverride (){
		base.AwakeOverride();
	}
	
	
	public override void UpdateElement(bool updateChildren = true){
		initDirectChildren();
		Vector2 position = ElementOffset;
		foreach (var child in DirectChildren){
			var childRegion = child.VirtualRegionOnScreen;
			child.Position = position;
			if(Orientation == ElementOrientation.horizontal){
				position.x += childRegion.width + ElementSpacing;	
			} else {
				position.y += childRegion.height + ElementSpacing;
			}
		}
		base.UpdateElement(updateChildren);
		
	}
	
}
