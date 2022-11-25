using UnityEngine;
using AssetBundle;

static public partial class MainUI
{
	static public void ActiveScene( bool isActive )
	{
		//if( canvasCamera.goRoot.activeSelf != isActive )
		//	canvasCamera.goRoot.SetActive( isActive );
	}
	static public void InitRoot()
	{
		if( null == canvasOverlay )
		{
			canvasOverlay = new CanvasDepthManager();
			GameObject obj = GameObject.Find( "Root_Overlay" );
			canvasOverlay.Initialize( obj );
		}

		if( null == canvasPopup )
		{
			canvasPopup = new CanvasDepthManager();
			GameObject obj = GameObject.Find( "Root_Popup" );
			canvasPopup.Initialize( obj );
		}
	}
	/*
	static public T ActiveUI<T>( eUIRootType rmType, UI type, UIResc resc, bool _active = false, bool _static = false ) where T : UIBase
	{
		return ActiveUI<T>( rmType, type, resc, resc.depth, _active, _static );
	}

	static public T ActiveUI<T>( eUIRootType rmType, UI type, UIResc resc, GameObject parent, bool _active = false, bool _static = false ) where T : UIBase
	{
		return ActiveUI<T>( rmType, type, resc, resc.depth, parent, _active, _static );
	}
	static public T ActiveUI<T>( eUIRootType rmType, UI type, UIResc resc, int depth, bool _active = false, bool _static = false ) where T : UIBase
	{
		if( null == UIManager.uiMainMgr )
			Initialize();

		T ui = UIManager.uiMainMgr.Active<T>( (int)type, resc, GetRootParent( rmType, depth ), false, true );
		if( ui != null )
		{
			ui.SetActive( _active );
			ui.SetStatic( _static );
			ui.transform.SetAsLastSibling();

			if( rmType != eUIRootType.Overlay )
			{
				Canvas[] arrCanvas = ui.GetComponentsInChildren<Canvas>();
				if( null != arrCanvas )
				{
					for( int i = 0; i < arrCanvas.Length; ++i )
					{
						arrCanvas[ i ].sortingOrder = depth;
					}
				}

				Camera[] arrCamera = ui.GetComponentsInChildren<Camera>();
				if( null != arrCamera )
				{
					for( int i = 0; i < arrCamera.Length; ++i )
					{
						AddCameraStack( rmType, arrCamera[ i ], i );
					}
				}
			}
		}

		return ui;
	}

	static public T ActiveUI<T>( eUIRootType rmType, UI type, UIResc resc, int depth, GameObject parent, bool _active = false, bool _static = false ) where T : UIBase
	{
		if ( null == UIManager.uiMainMgr )
			Initialize();

		T ui = UIManager.uiMainMgr.Active<T>( (int)type, resc, parent, false, true );
		if ( ui != null )
		{
			ui.SetActive( _active );
			ui.SetStatic( _static );
			ui.transform.SetAsLastSibling();

			if ( rmType != eUIRootType.Overlay )
			{
				Canvas[] arrCanvas = ui.GetComponentsInChildren<Canvas>();
				if ( null != arrCanvas )
				{
					for ( int i = 0; i < arrCanvas.Length; ++i )
					{
						arrCanvas[ i ].sortingOrder = depth;						
					}
				}

				Camera[] arrCamera = ui.GetComponentsInChildren<Camera>();
				
				if ( null != arrCamera )
				{
					for ( int i = 0; i < arrCamera.Length; ++i )
					{
						AddCameraStack( rmType, arrCamera[ i ], i );
					}
				}
			}
		}

		return ui;
	}

	static public GameObject GetRootParent( eUIRootType rmType, int depth )
	{
		switch( rmType )
		{
		case eUIRootType.Camera:
			{
				ActiveScene( true );
				return canvasCamera.LayerWithoutDepthUnit( depth );
			}
		case eUIRootType.Overlay:			return canvasOverlay.Layer( depth );
		case eUIRootType.Box:				return canvasBox.LayerWithoutDepthUnit( depth );
		case eUIRootType.Popup:				return canvasPopup.Layer( depth );
		}

		return null;
	}
	static public void AddCameraStack( eUIRootType rmType, Camera cam, int index )
	{
		if( null == cam || cam.GetComponent<UniversalAdditionalCameraData>().renderType != CameraRenderType.Overlay )
			return;

		UniversalAdditionalCameraData camData = baseCamera.GetComponent<UniversalAdditionalCameraData>();
		camData.cameraStack.Remove( cam );
		if( rmType == eUIRootType.Popup )
		{
			camData.cameraStack.Insert( camData.cameraStack.Count - 1, cam );
		}
		else if( rmType == eUIRootType.Camera || rmType == eUIRootType.Box )
		{
			if( false == cam.name.Contains( "_PP" ) )
				cam.orthographicSize = 360;
			for( int i = 0; i < camData.cameraStack.Count; ++i )
			{
				Camera camera = camData.cameraStack[ camData.cameraStack.Count - 1 - i ];
				if( null != camera && camera.name.Contains( "Overlay" ) )
				{
					if( rmType == eUIRootType.Camera )
						camData.cameraStack.Insert( camData.cameraStack.Count - 1 - i, cam );
					else
						camData.cameraStack.Insert( camData.cameraStack.Count - i + index, cam );
					break;
				}
			}
		}
	}
	*/
}

static public partial class MainUI
{
	static public CanvasDepthManager		canvasOverlay		{ private set; get; }
	static public CanvasDepthManager		canvasPopup			{ private set; get; }

	static public bool						isValid				= false;

	static public bool Initialize()
	{
		GameObject goMain = AssetManager.Inst.MakeOBJ( "UI/Common/MainUI" );
		if( null == goMain )
		{
			Utility.Log.Error( "[MainUI]", "MainUI_UGUI object load failed!" );
			return false;
		}

		InitRoot();

		UIManager uiMgr = goMain.GetComponentInChildren<UIManager>();
		if( null == uiMgr )
			uiMgr = goMain.AddComponent<UIManager>();

		if( null == uiMgr )
		{
			Utility.Log.Error( "[MainUI]", "MainUI_UGUI's UIManager load failed!" );
			return false;
		}
		UIManager.uiMainMgr = uiMgr;

		DepthManager dMgr = goMain.GetComponentInChildren<DepthManager>();
		if( null == dMgr )
			dMgr = goMain.AddComponent<DepthManager>();

		if( null == dMgr )
		{
			Debug.LogError( "[UI] MainUI_UGUI's DepthManager load failed!" );
			return false;
		}

		UIManager.uiMainMgr.NewBases( (int)UI.Type.Max );
		{
			UIManager.uiMainMgr.Init();
		}

		GameObject.DontDestroyOnLoad( goMain );

		isValid = true;

		return true;
	}
	static public void Destroy()
	{
		if( UIManager.uiMainMgr == null )
			return;

		{
		}

		GameObject.Destroy( UIManager.uiMainMgr.gameObject );
		UIManager.uiMainMgr = null;

		isValid = false;
	}

	static public T ActiveUI<T>( UI.Root root, UI.Type type, UIResc resc, int depth, bool _active = false ) where T : UIBase
	{
		if( null == UIManager.uiMainMgr )
			Initialize();

		T ui = UIManager.uiMainMgr.Active<T>( (int)type, resc, GetRootParent( root, depth ), false );
		if( ui != null )
		{
			ui.SetActive( _active );
			ui.transform.SetAsLastSibling();
		}

		return ui;
	}
	static public T ActiveUI<T>( UI.Type type ) where T : UIBase
	{
		T ui = UIManager.uiMainMgr.Get<T>( (int)type );
		if( ui == null )
			return null;

		ui.SetActive( true );
		return ui;
	}
	static public T InactiveUI<T>( UI.Type type ) where T : UIBase
	{
		if( null == UIManager.uiMainMgr )
			return null;

		return UIManager.uiMainMgr.Inactive<T>( (int)type );
	}

	static public void RemoveUI( UI.Type type )
	{
		UIManager.uiMainMgr.Remove( (int)type );
	}
	static public T GetUI<T>( UI.Type type ) where T : UIBase
	{
		return UIManager.uiMainMgr.Get<T>( (int)type );
	}
	static public void CleanUIs()
	{
		if( null == UIManager.uiMainMgr )
			return;

		for( int i = 0; i < (int)UI.Type.Max; ++i )
		{
			UIBase ui = UIManager.uiMainMgr.Get( i );
			if( null != ui )
			{
				UIManager.uiMainMgr.Remove( i );
			}
		}
	}
	static public void ShutsUIs()
	{
		if( null == UIManager.uiMainMgr )
			return;

		UIManager.uiMainMgr.Shuts();
	}

	static public GameObject GetRootParent( UI.Root root, int depth )
	{
		switch( root )
		{
		case UI.Root.Scene:	return canvasOverlay.Layer( depth );
		case UI.Root.Popup:	return canvasPopup.Layer( depth );
		}
		return null;
	}
#if _AGO
	static public T ActiveUI<T>( UI.Type type ) where T : UIBase
	{
		T ui = UIManager.uiMainMgr.At<T>( (int)type );
		if( ui == null )
			return null;

		ui.SetActive( true );
		return ui;
	}
	static public T InactiveUI<T>( UI.Type type ) where T : UIBase
	{
		if( null == UIManager.uiMainMgr )
			return null;

		return null;
		//return UIManager.uiMainMgr.Inactive<T>( (int)type );
	}
	static public T GetUI<T>( UI.Type type ) where T : UIBase
	{
		return UIManager.uiMainMgr.At<T>( (int)type );
	}
	static public bool IsActiveUI( UI.Type type )
	{
		UIBase ui = UIManager.uiMainMgr.At( (int)type );
		if( null != ui &&
			true == ui.isActive )
		{
			return true;
		}
		return false;
	}
	static public void RemoveUI( UI.Type type )
	{
		UIManager.uiMainMgr.Remove( (int)type );
	}
	static public void CleanUIs()
	{
		if( null == UIManager.uiMainMgr )
			return;

		for( int i = 0; i < (int)UI.Type.Max; ++i )
		{
			UIBase ui = UIManager.uiMainMgr.At( i );
			if( null != ui )
			{
				UIManager.uiMainMgr.Remove( i );
			}
		}
	}
	static public void ShutsUIs()
	{
		if( null == UIManager.uiMainMgr )
			return;

		UIManager.uiMainMgr.ShutsAll();
	}

	static public void UpdateTouchSens()
	{
		//if( null != canvasCamera )
		//	canvasCamera.UpdateTouchSens();

		if( null != canvasOverlay )
			canvasOverlay.UpdateTouchSens();

		//if( null != canvasBox )
		//	canvasBox.UpdateTouchSens();

		if( null != canvasPopup )
			canvasPopup.UpdateTouchSens();
	}

	static public Vector3 WorldToScreenPoint( Vector3 pos )
	{
		return baseCamera.WorldToScreenPoint( pos );
	}
	static public Vector3 ScreenToWorldPoint( Vector3 pos )
	{
		return baseCamera.ScreenToWorldPoint( pos );
	}

	static public bool RectContainsScreenPoint( RectTransform rect, Vector2 screenPos )
	{
		return RectTransformUtility.RectangleContainsScreenPoint( rect, screenPos, MainUI.baseCamera );
	}
#endif
}