using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssetBundle;

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
			Utility.Log.Error( "[UI]", string.Format( $"object({uiResc.uiName}) load failed!" ) );
			return null;
		}

		T ui = Add<T>( index, uiObject, uiParent, _active );
		if( ui == null )
		{
			Utility.Log.Error( "[UI]", string.Format( "object({0}) component load failed!", uiResc.uiName ) );
			return null;
		}

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

		if( true != Utility.Object.IsParentObject( uiParent, uiObject ) )
		{
			Utility.Object.AttachUIObject( uiParent, uiObject );
		}

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
			Utility.Log.Error( "[UI]", string.Format( $"object({name}) . not found resource!"));
#endif
			return null;
		}

		ui = AddByMake<T>( index, uiResc, uiParent, isActive );
		if( null != ui )
			return ui;

		Utility.Log.Error( "[UI]", string.Format( $"object({name}) active failed!" ) );

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
}
