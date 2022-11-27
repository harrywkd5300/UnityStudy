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

		static public bool IsParentObject( GameObject goParent, GameObject goChild )
		{
			Transform trans = goChild.transform;
			Transform parent = goParent.transform;

			while( trans != null )
			{
				if( parent == trans )
					return true;

				trans = trans.parent;
			}
			return false;
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

			RectTransform rt = goChild.GetComponent<RectTransform>();
			if( rt == null )
				return false;

			rt.SetParent( goParent.transform, false );
			rt.localPosition = Vector3.zero;
			rt.localScale = Vector3.one;

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