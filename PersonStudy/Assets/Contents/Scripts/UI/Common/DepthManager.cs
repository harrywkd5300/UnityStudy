using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contents Depth			-> 0 ~ 899
/// Main Bottom & Main Top	-> 900
/// Contents Popup Depth	-> 901 ~ 999
/// System Popup Depth		-> 1000
/// </summary>
/// 
public class DepthManager : MonoBehaviour
{
	private GameObjectPool		goPool;
	private GameObject			goRoot;
	private List<DepthUnit>		goList;

	public void Initialize( GameObject goRoot )
	{
		if( goRoot == null || goPool != null )
			return;

		this.goRoot = goRoot;
		this.goList = new List<DepthUnit>();

		UIResc resc = new UIResc( "UI/Common", "DepthUnit" );
		goPool = GameObjectPool.MakeComponent( goRoot, resc.GetObject(), 4, 4 );
	}

	public GameObject GetDepthObject( int depth )
	{
		GameObject res = null;
		foreach( DepthUnit du in goList )
		{
			if( du.depth == depth )
			{
				res = du.gameObject;
				break;
			}
		}

		if( res == null )
		{
			res = AddDepthObject( depth );
		}
		return res;
	}
	private GameObject AddDepthObject( int depth )
	{
		GameObject child = goPool.AddChild( goRoot );
		if( child != null )
		{
			RectTransform rt = child.GetComponent<RectTransform>();
			if( rt != null )
			{
				rt.offsetMin = Vector2.zero;
				rt.offsetMax = Vector2.zero;
				rt.anchorMin = Vector2.zero;
				rt.anchorMax = Vector2.one;
			}

			DepthUnit du = child.GetComponent<DepthUnit>();
			if( du != null )
			{
				du.SetDepth( depth );
				goList.Add( du );
			}

			child.transform.localPosition	= Vector3.zero;
			child.transform.localScale		= Vector3.one;
			child.transform.name			= string.Format( "Depth{0:D4}", depth );

			//Optimize();
		}
		return child;
	}
	private void Optimize()
	{
		goList.Sort( Sorting );

		int idx = 0;
		foreach( DepthUnit du in goList )
		{
			du.transform.SetSiblingIndex( idx++ );
		}

	}
	private int Sorting( DepthUnit lhs, DepthUnit rhs )
	{
		if( lhs.depth > rhs.depth ) return 1;
		else if( lhs.depth < rhs.depth ) return -1;

		return 0;
	}
}