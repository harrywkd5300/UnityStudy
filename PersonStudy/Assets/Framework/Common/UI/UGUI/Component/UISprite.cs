using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UISprite : Image, IStateHandler
	{
		[SerializeField] [HideInInspector] List<ColorUnit>	mColors = null;
		[SerializeField] [HideInInspector] List<SpriteUnit> mSprite = null;

		protected override void Awake()
		{
			base.Awake();
			if( Application.isPlaying )
			{
				UpdateState( State.Normal );
			}
		}

		public void UpdateState( State state, bool applyChild = false )
		{
			SetColorOption( state );
			SetSpriteOption( state );
		}
		public void UpdateState( int state, bool applyChild = false )
		{
			SetColorOption( state );
			SetSpriteOption( state );
		}

		public void SetColorOption( State state )
		{
			if( null == mColors )
				return;

			foreach( ColorUnit unit in mColors )
			{
				if( state == unit.state )
				{
					unit.Set<Image>( this );
				}
			}
		}
		public void SetColorOption( int state )
		{
			if( null == mColors )
				return;

			foreach( ColorUnit unit in mColors )
			{
				if( state == (int)unit.state )
				{
					unit.Set<Image>( this );
				}
			}
		}
		public void SetSpriteOption( State state )
		{
			if( null == mSprite )
				return;

			foreach( SpriteUnit unit in mSprite )
			{
				if( state == unit.state )
				{
					Utility.UGUI.SetImage( this, unit.sprite );
					break;
				}
			}
		}
		public void SetSpriteOption( int state )
		{
			if( null == mSprite )
				return;

			foreach( SpriteUnit unit in mSprite )
			{
				if( state == (int)unit.state )
				{
					Utility.UGUI.SetImage( this, unit.sprite );
					break;
				}
			}
		}
	}
}