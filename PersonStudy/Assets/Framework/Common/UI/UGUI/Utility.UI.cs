using System;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
	static public class UGUI
	{
		//!< Common
		static public void SetActive( GameObject go, bool value )
		{
			go?.SetActive( value );
		}

		//!< Text
		static public void SetText( Text text, int value )
		{
			if( null == text )
				return;

			SetText( text, Common.GetString( value ) );
		}
		static public void SetText( Text text, int value, params object[] args )
		{
			if( null == text )
				return;

			SetText( text, Common.GetString( value, args ) );
		}
		static public void SetText( Text text, string value )
		{
			if( null == text )
				return;

			text.text = value;
		}
		static public void SetText( Text text, string value, params object[] args )
		{
			if( null == text )
				return;

			text.text = value;
		}

		//!< Utility
		static public void UpdateChildState( Transform root, UI.State updateState, bool applyChild )
		{
			foreach( Transform child in root )
			{
				IStateHandler stateHandler = child.GetComponent<IStateHandler>();
				if( stateHandler != null )
				{
					stateHandler.UpdateState( updateState, applyChild );
				}
				else
				{
					UpdateChildState( child, updateState, applyChild );
				}
			}
		}
		static public void UpdateChildState( Transform root, int updateState, bool applyChild )
		{
			foreach( Transform child in root )
			{
				IStateHandler stateHandler = child.GetComponent<IStateHandler>();
				if( stateHandler != null )
				{
					stateHandler.UpdateState( updateState, applyChild );
				}
				else
				{
					UpdateChildState( child, updateState, applyChild );
				}
			}
		}
	}
}