using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

namespace UI
{
	[System.Serializable]
	public class SpriteUnit
	{
		[SerializeField] [HideInInspector] State			mState				= State.Normal;
		[SerializeField] [HideInInspector] Sprite			mSprite				= null;
		[SerializeField] [HideInInspector] bool				mUseAtlas			= false;
		[SerializeField] [HideInInspector] SpriteAtlasType	mSpriteAltasType	= SpriteAtlasType.None;
		[SerializeField] [HideInInspector] string			mSpriteName			= string.Empty;

		public State			state			{ get { return mState; } }
		public Sprite			sprite			{ get { return mSprite;	} }
		public bool				useAtlas		{ get { return mUseAtlas; } }
		public SpriteAtlasType	spriteAtlas		{ get { return mSpriteAltasType; } }
		public string			spriteName		{ get { return mSpriteName; } }

		public SpriteUnit( State _state )
		{
			this.mState				= _state;
			this.mSprite			= null;
			this.mUseAtlas			= false;
			this.mSpriteAltasType	= SpriteAtlasType.None;
			this.mSpriteName		= string.Empty;
		}
	}
}