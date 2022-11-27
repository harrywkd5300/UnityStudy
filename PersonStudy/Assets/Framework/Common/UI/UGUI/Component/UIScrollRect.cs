using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	//[RequireComponent( typeof( Image ) )]
	//[RequireComponent( typeof( RectMask2D ) )]
	public class UIScrollRect : ScrollRect
	{
		protected override void Awake()
		{
			//Image image = GetComponent<Image>();
			//if( image != null )
			//{
			//	image.color = new Color( 1f, 1f, 1f, 0f );
			//}

			base.Awake();
		}

		[ContextMenu( "ResetPosition" )]
		public void ResetPosition()
		{
			//if( horizontal == true )
			//	horizontalNormalizedPosition = 0f;
			//if( vertical == true )
			//	verticalNormalizedPosition = 1f;
		}
	}
}