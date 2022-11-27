using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UISprite : Image, IStateHandler
	{
		[SerializeField] [HideInInspector] List<ColorUnit> mColors = null;

		protected override void Awake()
		{
			base.Awake();
			if( Application.isPlaying )
			{
				SetColorOption( State.Normal );
			}
		}

		public void UpdateState( State state, bool applyChild = false )
		{
			SetColorOption( state );
		}
		public void UpdateState( int state, bool applyChild = false )
		{
			SetColorOption( state );
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
	}
}