using UnityEngine;
using System.Collections;

public class BoxTransformationMap {

	public Box Box{
		get;set;
	}
	public Rect Transformation{
		get;set;
	}
	
	public BoxTransformationMap(Box box, Rect transformation){
		this.Box = box;
		this.Transformation = transformation;
	}
}
