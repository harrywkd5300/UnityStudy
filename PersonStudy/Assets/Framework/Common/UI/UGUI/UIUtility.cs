using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	static public class Utility
	{
		static public Image SetImage( Image image, Sprite changeImage, bool isNativeSize = false )
		{
			if( null != image && null != changeImage )
			{
				image.sprite = changeImage;

				if( isNativeSize )
					image.SetNativeSize();
			}
			return image;
		}

		static public void UpdateChildState( Transform root, UI.State updateState, bool applyChild )
		{
			foreach( Transform child in root )
			{
				IStateHandler stateHandler = child.GetComponent<IStateHandler>();
				if( stateHandler != null )
				{
					stateHandler.UpdateState( updateState, applyChild );
				}
				else
				{
					UpdateChildState( child, updateState, applyChild );
				}
			}
		}
		static public void UpdateChildState( Transform root, int updateState, bool applyChild )
		{
			foreach( Transform child in root )
			{
				IStateHandler stateHandler = child.GetComponent<IStateHandler>();
				if( stateHandler != null )
				{
					stateHandler.UpdateState( updateState, applyChild );
				}
				else
				{
					UpdateChildState( child, updateState, applyChild );
				}
			}
		}
	}
}