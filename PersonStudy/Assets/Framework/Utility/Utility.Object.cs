namespace Utility
{
	using UnityEngine;
	using System.Collections.Generic;

	static public class Object
	{
		static public void SetActive( GameObject go, bool active )
		{
			if( go != null )
				go.SetActive( active );
		}

		static public bool AttachObject( GameObject goParent, GameObject goChild )
		{
			if( null == goParent || null == goChild )
				return false;

			goChild.transform.SetParent( goParent.transform );
			goChild.transform.localPosition	= Vector3.zero;
			goChild.transform.localScale	= Vector3.one;
			return true;
		}
		static public bool AttachUIObject( GameObject goParent, GameObject goChild, bool childLayer = true )
		{
			if( null == goParent || null == goChild )
				return false;

			goChild.transform.SetParent( goParent.transform );
			goChild.transform.localPosition = Vector2.zero;
			goChild.transform.localScale = Vector2.one;

			if( goParent.layer != goChild.layer )
			{
				SetLayer( goChild, goParent.layer, childLayer );
			}
			return true;
		}

		static public void SetLayer( GameObject go, int layer, bool child )
		{
			if( go == null )
				return;

			go.layer = layer;

			if( child == true )
			{
				foreach( Transform gt in go.transform )
				{
					SetLayer( gt.gameObject, layer, child );
				}
			}
		}
	}
}