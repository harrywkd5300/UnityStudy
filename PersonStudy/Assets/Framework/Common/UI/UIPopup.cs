
using UnityEngine;

public class UIPopup : MonoBehaviour, IUibase
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

	public UI.Type	type			= UI.Type.None;

	public bool		isActive		{ get { return gameObject.activeSelf; } }

	public virtual void Init( UI.Type _type )	{ this.type = _type; }
	public virtual void Clear()					{ }
	public virtual bool Shuts( bool isShuts )	{ return false; }
	public virtual void Apply()					{ }
	public virtual void Refresh()				{ }

	public virtual void SetActive( bool value ) { if( isActive != value ) gameObject.SetActive( value ); }

	public override string ToString() { return name; }
}
