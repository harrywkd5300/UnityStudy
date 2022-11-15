using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct UIResc
{
	public string		uiContents;			//!< 리소스 컨텐츠 위치.
	public string		uiName;				//!< 리소스 이름
	public UIBase		uiBase;				//!< 리소스 객체.

	public bool			isValid				{ get { return ( !string.IsNullOrEmpty( uiContents ) || !string.IsNullOrEmpty(uiName) || null != uiBase ); } }
	
	public UIResc( string contents, string name )						: this( contents, "", name, null )			{ }
	public UIResc( string contents, string path, string name, UIBase objt )
	{
		this.uiContents = contents;
		this.uiName		= name;
		this.uiBase		= objt;
	}
	
	public GameObject GetObject()
	{
		if( uiBase != null )
			return uiBase.gameObject;

		if( string.IsNullOrEmpty( uiName ) )
			return null;

		GameObject go = AssetManager.Inst.LoadResources<GameObject>( string.Format( $"{uiContents}/{uiName}" ) );
		if( go != null )
		{
			uiBase = go.GetComponent<UIBase>();

			return go;
		}
		return null;
	}
	public T GetObject<T>() where T : UnityEngine.Object
	{
		return GetObject()?.GetComponent<T>();
	}
}
