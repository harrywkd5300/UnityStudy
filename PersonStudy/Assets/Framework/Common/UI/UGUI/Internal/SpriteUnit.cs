using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[System.Serializable]
	public class SpriteUnit
	{
		[SerializeField] [HideInInspector] State	mState	= State.Normal;
		[SerializeField] [HideInInspector] Sprite	mSprite	= null;

		public State	state	{ get { return mState; } }
		public Sprite	sprite	{ get { return mSprite;	} }

		public SpriteUnit( State _state )
		{
			this.mState		= _state;
			this.mSprite	= null;
		}
	}
}