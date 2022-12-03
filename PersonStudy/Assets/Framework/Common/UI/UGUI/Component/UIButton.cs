using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace UI
{
	public class UIButton : Button, IStateHandler
	{
		[System.Serializable]
		public class ColorOptions
		{
			[SerializeField][HideInInspector] GameObject		mTarget			= null;

			public void UpdateState( State state )
			{
				if( mTarget != null )
				{
					IStateHandler handler = mTarget.GetComponent<IStateHandler>();

					if( handler != null )
					{
						handler.UpdateState( state );
					}
				}
			}
			public void UpdateState( int state )
			{
				if( mTarget != null )
				{
					IStateHandler handler = mTarget.GetComponent<IStateHandler>();

					if( handler != null )
					{
						handler.UpdateState( state );
					}
				}
			}
		}

		[SerializeField] [HideInInspector] List<ColorOptions> mColorOptions = null;
		public bool mUseTween = true;

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void DoStateTransition( SelectionState state, bool instant )
		{
			base.DoStateTransition( state, instant );

			if( Application.isPlaying && null != mColorOptions )
			{
				foreach( ColorOptions op in mColorOptions )
				{
					switch( state )
					{
					case SelectionState.Normal:			op.UpdateState( State.Normal );		break;
					case SelectionState.Disabled:		op.UpdateState( State.Disabled );	break;
					}
				}
			}
		}

		private void OnEvent( SelectionState state )
		{
			if( interactable == false )
				return;

			if( Application.isPlaying && null != mColorOptions )
			{
				foreach( ColorOptions op in mColorOptions )
				{
					switch( state )
					{
					case SelectionState.Normal:		op.UpdateState( State.Normal ); break;
					case SelectionState.Pressed:	op.UpdateState( State.Pressed ); break;
					}
				}

				switch( state )
				{
				case SelectionState.Pressed: DoScale( 0.95f ); break;
				default: DoScale( 1f ); break;
				}
			}
		}
		public override void OnPointerDown( PointerEventData eventData )
		{
			base.OnPointerDown( eventData );
			OnEvent( SelectionState.Pressed );
		}
		public override void OnPointerUp( PointerEventData eventData )
		{
			base.OnPointerUp( eventData );
			OnEvent( SelectionState.Normal );
		}
		private void DoScale( float scale )
		{
			if( mUseTween == true )
				transform.DOScale( new Vector3( scale, scale, 1f ), 0.1f ).SetEase( Ease.OutBack );
		}

		public void UpdateState( State state, bool applyChild = false )
		{
			foreach( ColorOptions op in mColorOptions )
				op.UpdateState( state );

			if( applyChild == true )
				Utility.UGUI.UpdateChildState( this.transform, state, applyChild );
		}
		public void UpdateState( int state, bool applyChild = false )
		{
			foreach( ColorOptions op in mColorOptions )
				op.UpdateState( state );

			if( applyChild == true )
				Utility.UGUI.UpdateChildState( this.transform, state, applyChild );
		}
	}
}