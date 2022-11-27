using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[System.Serializable]
	public class ColorUnit
	{
		[SerializeField] [HideInInspector] State	mState	= State.Normal;
		[SerializeField] [HideInInspector] Color	mColor	= Color.white;
		[SerializeField] [HideInInspector] int		mAlpha	= 255;

		public State	state	{ get { return mState; } }
		public Color	color	{ get { return mColor; } }
		public int		alpha	{ get { return mAlpha; } }

		public ColorUnit( State _state )
		{
			this.mState = _state;
			this.mColor = Color.white;
			this.mAlpha = 255;
		}

		public void Set<T>( T target ) where T : MaskableGraphic
		{
			if( null == target )
				return;

			target.color = mColor;
		}
		public void Set( GameObject target )
		{
			if( null == target )
				return;

			Image image = target.GetComponent<Image>();
			if( null != image )
			{
				Set<Image>( image );
				return;
			}

			Text text = target.GetComponent<Text>();
			if( null != text )
			{
				Set<Text>( text );
				return;
			}

			RawImage texture = target.GetComponent<RawImage>();
			if( null != texture )
			{
				Set<RawImage>( texture );
				return;
			}
		}
	}
}