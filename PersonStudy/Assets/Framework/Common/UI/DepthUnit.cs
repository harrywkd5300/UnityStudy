using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( RectTransform ) )]
[RequireComponent( typeof( Canvas ) )]

public class DepthUnit : MonoBehaviour {

	[SerializeField] private Canvas canvas = null;
	
	public int depth;

	public void SetDepth( int depth ) 
	{
		this.depth = depth;
		this.canvas.overrideSorting = true;
		this.canvas.sortingOrder = depth;
	}
}