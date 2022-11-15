using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBase : MonoBehaviour
{
	private RectTransform mRT;
	public RectTransform rectTransform
	{
		get 
		{
			if( mRT == null )
				mRT = gameObject.transform as RectTransform;

			return mRT;
		} 
	}
	
	public bool isActive			{ get { return gameObject.activeSelf; } }

	public virtual void Init() { }
	public virtual void Clear() { }
	public virtual bool Shuts() { return false; }
	public virtual void SetActive( bool value ) { if( isActive != value ) gameObject.SetActive( value ); }
}