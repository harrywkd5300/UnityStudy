using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : UIBase
{
	//!< UI Main Manager
	static public UIManager uiMainMgr	{ set; get; }

	public UIBase[]			uiBases		{ private set; get; }
	public bool				isValid		{ get { return ( null != uiBases ) ? true : false; } }

	public override void Init()
	{
	}
	public override void Clear()
	{
		if( !isValid )
			return;

		foreach( UIBase ui in uiBases )
		{
			if( ui != null )
				ui.Clear();
		}
		uiBases = null;
	}
	public override bool Shuts()
	{
		if( !isValid )
			return false;

		foreach( UIBase ui in uiBases )
		{
			if( ui != null && ui.isActive )
			{
				ui.Shuts();
			}
		}
		return true;
	}

	public UIBase this[ int index ]
	{
		get { return ( !isValid ) ? null : uiBases[ index ]; }
	}
	public UIBase Get( int index )
	{
		return ( !isValid ) ? null : uiBases[ index ];
	}
	public T Get<T>( int index ) where T : UIBase
	{
		return ( !isValid ) ? null : uiBases[ index ] as T;
	}
	public void NewBases( int maxIndex )
	{
		uiBases = new UIBase[ maxIndex ];
	}

	public void SetActive( bool value, int index )
	{
		UIBase ui = Get( index );
		if( ui != null )
			ui.SetActive( value );
	}
	public void SetActiveAll( bool value )
	{
		if( !isValid )
			return;

		foreach( UIBase ui in uiBases )
			ui.SetActive( value );
	}
	public void Remove( int index )
	{
		UIBase ui = Get( index );
		if( null == ui )
			return;

		uiBases[ index ] = null;
		ui.Clear();

		GameObject.Destroy( ui.gameObject );
	}

	public T AddByMake<T>( int index, UIResc uiResc, GameObject uiParent, bool _active = false ) where T : UIBase
	{
		if( !uiResc.isValid )
			return null;

		GameObject uiObject = AssetManager.Inst.MakeOBJ( uiResc.GetObject() );
		if( uiObject == null )
		{
			Utility.Log.Error( string.Format( $"[UI] object({uiResc.uiName}) load failed!" ) );
			return null;
		}

		T ui = Add<T>( index, uiObject, uiParent, _active );
		if( ui == null )
		{
			Debug.LogError( string.Format( "[UI] object({0}) component load failed!", uiResc.uiName ) );
			return null;
		}

		//SetCanvasCamera( ui.gameObject );

		return ui;
	}
	public T Add<T>( int index, GameObject uiObject, GameObject uiParent, bool _active = false ) where T : UIBase 
	{
		if( !isValid )
			return null;

		//!< (a).Component
		if( uiObject == null )
			return null;

		T ui = uiObject.GetComponent<T>();
		{
			if( ui == null )
				ui = uiObject.AddComponent<T>();

			if( ui == null )
				return null;
		}

		//!< (b).Attach
		if( uiParent == null )
			uiParent = gameObject;

		Utility.Object.AttachUIObject( uiParent, uiObject );

		//!< (c).Setting.
		uiBases[ index ] = ui;
		{
			ui.Init();
			ui.SetActive( _active );
		}
		return ui;
	}

	public T Active<T>( int index, UIResc uiResc, GameObject uiParent, bool isActive = true ) where T : UIBase
	{
		T ui = Get<T>( index );
		if( null != ui )
		{
			ui.SetActive( true );
			return ui;
		}

		if( null == uiResc.GetObject() )
		{
#if UNITY_EDITOR
			Utility.Log.Error( string.Format( $"[UI] object({name}) . not found resource!"));
#endif
			return null;
		}

		ui = AddByMake<T>( index, uiResc, uiParent, isActive );
		if( null != ui )
			return ui;

		Utility.Log.Error( string.Format( $"[UI] object({name}) active failed!" ) );

		return null;
	}
	public T Inactive<T>( int index ) where T : UIBase
	{
		T ui = Get<T>( index );
		if( null == ui )
			return null;

		ui.SetActive( false );
		return ui;
	}

#if _AGO
	public override void Init()
	{
		uiCamera = gameObject.GetComponentInChildren<Camera>();
		{
			if( null == uiCamera )
				uiCamera = gameObject.GetComponentInParent<Camera>();
		}

		/*
		if( uiCamera != null )
		{
			AudioListener[] listeners = GameObject.FindObjectsOfType( typeof( AudioListener ) ) as AudioListener[];
			AudioListener _listener = null;

			if( listeners != null )
			{
				for( int i = 0; i < listeners.Length; ++i )
				{
					if( listeners[ i ] && listeners[ i ].enabled && listeners[ i ].gameObject.activeInHierarchy )
					{
						_listener = listeners[ i ];
						break;
					}
				}
			}

			if( _listener == null )
			{
				uiCamera.gameObject.AddComponent<AudioListener>();
			}
		}
		*/

		if( !isValid )
		{
			Debug.LogError( string.Format( "[UI] groupUI({0}) init failed!", this.ToString() ) );
		}
	}
	public override void Clear()
	{
		if( !isValid )
			return;

		foreach( UIBase ui in uiBases )
		{
			if( ui != null )
				ui.Clear();
		}
		uiBases = null;
		uiCamera = null;
	}

	public override bool Shuts()
	{
		if( !isValid )
			return false;

		foreach( UIBase ui in uiBases )
		{
			if( ui != null && ui.isActive )
			{
				if( ui.Shuts() )
				{
					return true;
				}
			}
		}
		return false;
	}
	public void ShutsAll()
	{
		if( !isValid )
			return;

		foreach( UIBase ui in uiBases )
		{
			if( ui != null && ui.isActive )
			{
				ui.Shuts();
			}
		}
	}

	public void SetActive( bool value, int index )
	{
		UIBase ui = At( index );
		if( ui != null )
			ui.SetActive( value );
	}
	public void SetActiveAll( bool value, UIBase ignoreUI = null )
	{
		if( !isValid )
			return;

		foreach( UIBase ui in uiBases )
		{
			if( ui != null && ui != ignoreUI )
				ui.SetActive( value );
		}
	}

	public T AddByFind<T>( int index, UIResc uiResc, GameObject uiParent, bool _active = false, bool _static = false ) where T : UIBase
	{
		//GameObject uiObject = GameObjectUtil.FindChildByName( gameObject, uiResc.uiName );
		//if( uiObject == null )
		//{
		//	Debug.LogError( string.Format( "[UI] object({0}) find failed!", uiResc.uiName) );
		//	return null;
		//}

		//T ui = Add<T>( index, uiObject, uiParent, _active, _static );
		//if( ui == null )
		//{
		//	Debug.LogError( string.Format( "[UI] object({0}) component load failed!", name ));
		//	return null;
		//}

		//SetCanvasCamera( ui.gameObject );

		return null;
	}

	public T AddByMake<T>( int depth, int index, UIResc uiResc, GameObject uiParent, bool _active = false, bool _static = false ) where T : UIBase
	{
		GameObject uiObject = MakeOBJ( uiResc );
		if( uiObject == null )
		{
			Debug.LogError( string.Format( "[UI] object({0}) load failed!", uiResc.uiName ) );
			return null;
		}

		//if( uiParent == null )
		//{
		//	//uiParent = MainUI_UGUI.Layer( layer );
		//	uiParent = MainUI.Layer( depth );
		//}

		T ui = Add<T>( index, uiObject, uiParent, _active, _static );
		if( ui == null )
		{
			Debug.LogError( string.Format( "[UI] object({0}) component load failed!", uiResc.uiName ) );
			return null;
		}
		return ui;
	}
	public T AddByMake<T>( int index, UIResc uiResc, GameObject uiParent, bool _active = false, bool _static = false ) where T : UIBase
	{
		GameObject uiObject = MakeOBJ( uiResc );
		if( uiObject == null )
		{
			Debug.LogError( string.Format( "[UI] object({0}) load failed!", uiResc.uiName ) );
			return null;
		}
		
		T ui = Add<T>( index, uiObject, uiParent, _active, _static );
		if( ui == null )
		{
			Debug.LogError( string.Format( "[UI] object({0}) component load failed!", uiResc.uiName) );
			return null;
		}

		SetCanvasCamera( ui.gameObject );

		return ui;
	}
	private void SetCanvasCamera( GameObject go )
	{
		if( null != go )
		{
			Canvas[] cvs = go.GetComponentsInChildren<Canvas>();
			if( null != cvs && 0 < cvs.Length )
			{
				foreach( Canvas cv in cvs )
				{
					if( null == cv.worldCamera )
						cv.worldCamera = uiCamera;
				}
			}
		}
	}

	public T Add<T>( int index, GameObject uiObject, GameObject uiParent, bool _active = false, bool _static = false ) where T : UIBase 
	{
		if( !isValid )
			return null;

		//!< (a).Component
		if( uiObject == null )
			return null;

		T ui = uiObject.GetComponent<T>();
		{
			if( ui == null )
				ui = uiObject.AddComponent<T>();

			if( ui == null )
				return null;
		}

		//!< (b).Attach
		if( uiParent == null )
			uiParent = gameObject;

		//if( true != GameObjectUtil.IsParentObject( uiParent, uiObject ) )
		//{
		//	//if( MainUI.uiMainMgr == this )
		//	//	GameObjectUtil.AttachUIObject( uiParent, uiObject );
		//	//else
		//		GameObjectUtil.AttachObject_UGUI( uiParent, uiObject );
		//}

		//!< (c).Setting.
		uiBases[ index ] = ui;
		{
			ui.Init();
			ui.SetActive( _active );
			ui.SetStatic( _static );
		}
		return ui;
	}
	public void Remove( int index )
	{
		UIBase ui = At( index );
		if( null == ui )
			return;

		uiBases[ index ] = null;
		ui.Clear();
		GameObject.Destroy( ui.gameObject );
	}

	public T Active<T>( int index, UIResc uiResc, GameObject uiParent, bool _find = false, bool _make = false ) where T : UIBase
	{
		T ui = At<T>( index );
		if( null != ui )
		{
			ui.SetActive( true );
			return ui;
		}

		if( _find )
		{
			ui = AddByFind<T>( index, uiResc, uiParent, true, false );
			if( null != ui )
				return ui;
		}

		if( _make )
		{
			if (null == uiResc.GetObject())
			{
#if UNITY_EDITOR
				Debug.LogError(string.Format("[UI] object({0}) . not found resource!", name));
#endif
				return null;
			}

			ui = AddByMake<T>( index, uiResc, uiParent, true, false );
			if( null != ui )
				return ui;
		}

		Debug.LogError( string.Format( "[UI] object({0}) active failed!", name ) );
		return null;
	}
	public T Inactive<T>( int index ) where T : UIBase
	{
		T ui = At<T>( index );
		if( null == ui )
			return null;

		ui.SetActive( false );
		return ui;
	}

	//!< UI Main Manager
	static public UIManager uiMainMgr { set; get; }
	public T GetUI<T>( int index ) where T : UIBase
	{
		return At( index ) as T;
	}
	public T MakeUI<T>( int index, UIResc uiResc, GameObject uiParent, bool _active = false, bool _static = false ) where T : UIBase
	{
		return AddByMake<T>( index, uiResc, ( null == uiParent ) ? gameObject : uiParent, _active, _static );
	}
	public void RemoveUI( int index )
	{
		Remove( index );
	}

	static public GameObject MakeOBJ( UIResc uiResc )
	{
		GameObject go = uiResc.GetObject();
		if( null == go )
			return null;

		return Object.Instantiate( go ) as GameObject;
	}
	static public GameObject LoadOBJ( UIResc uiResc )
	{
		return uiResc.GetObject();
	}
#endif
}
