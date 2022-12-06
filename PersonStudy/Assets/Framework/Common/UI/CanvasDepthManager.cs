using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class CanvasDepthManager
{
	public GameObject			goRoot			{ private set; get; }
	public DepthManager			depthMgr		{ private set; get; }
	public EventSystem			eventSystem		{ private set; get; }

	public bool Initialize( GameObject root )
	{
		if( null == root )
			return false;

		this.goRoot = root;

		if( null == depthMgr )
		{
			DepthManager dMgr = goRoot.GetComponentInChildren<DepthManager>();
			if( null == dMgr )
			{
				dMgr = goRoot.AddComponent<DepthManager>();
			}

			if( null == dMgr )
			{
				Utility.Log.Error( "[DepthManager]", "MainUI_UGUI's DepthManager load failed!" );
				return false;
			}

			depthMgr = dMgr;
			{
				depthMgr.Initialize( goRoot );
			}
		}

		Camera camera = goRoot.GetComponentInChildren<Camera>();
		if( null != camera )
			CameraManager.Inst.AddCameraStack( camera );

		return true;
	}

	public Canvas GetCanvas()
	{
		if( null != goRoot )
			goRoot.GetComponent<Canvas>();

		return null;
	}
	public GameObject Layer( int depth )
	{
		return depthMgr.GetDepthObject( depth );
	}
	public void UpdateTouchSens()
	{
		if( null != eventSystem )
		{
			eventSystem.pixelDragThreshold = 30;
		}
	}
	public GameObject LayerWithoutDepthUnit( int depth )
	{
		string name = string.Format( "Depth{0:D4}", depth );
		Transform child = goRoot.transform.Find( name );
		if( null == child )
		{
			GameObject go = new GameObject( name );
			RectTransform rt = go.AddComponent<RectTransform>();
			if( rt != null )
			{
				rt.offsetMin = Vector2.zero;
				rt.offsetMax = Vector2.zero;
				rt.anchorMin = Vector2.zero;
				rt.anchorMax = Vector2.one;
			}
			go.transform.SetParent( goRoot.transform );
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			child = go.transform;
		}
		return child.gameObject;
	}
}
