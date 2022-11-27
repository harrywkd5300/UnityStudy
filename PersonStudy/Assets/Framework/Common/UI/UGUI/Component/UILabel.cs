using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UILabel : Text, IStateHandler
	{
		[SerializeField] [HideInInspector] List<ColorUnit> mColors = null;

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

			if( applyChild == true )
				UI.Utility.UpdateChildState( this.transform, state, applyChild );
		}
		public void UpdateState( int state, bool applyChild = false )
		{
			SetColorOption( state );

			if( applyChild == true )
				UI.Utility.UpdateChildState( this.transform, state, applyChild );
		}

		public void SetColorOption( State state )
		{
			if( null == mColors )
				return;

			foreach( ColorUnit unit in mColors )
			{
				if( state == unit.state )
				{
					unit.Set<Text>( this );
				}
			}
		}
		private void SetColorOption( int state )
		{
			if( null == mColors )
				return;

			foreach( ColorUnit unit in mColors )
			{
				if( state == (int)unit.state )
				{
					unit.Set<UILabel>( this );
				}
			}
		}

	}
}
