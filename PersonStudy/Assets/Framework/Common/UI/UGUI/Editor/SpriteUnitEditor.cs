using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer( typeof( UI.SpriteUnit ) )]
public class SpriteUnitEditor : PropertyDrawer
{
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
	{
		GUIContent gc = new GUIContent();

		using( new EditorGUI.PropertyScope( position, label, property ) )
		{
			EditorGUIUtility.labelWidth		= 80;
			position.height					= EditorGUIUtility.singleLineHeight;
			float height					= EditorGUIUtility.singleLineHeight + 4;
			float viewWidth					= EditorGUIUtility.currentViewWidth;

			Rect rect = new Rect( position );

			Rect stateRect = new Rect( rect );
			{
				stateRect.width		= 60f;
			}

			SerializedProperty sp = property.FindPropertyRelative( "mState" );
			if( null != sp )
			{
				UI.State st = UI.State.Normal;

				st = (UI.State)sp.enumValueIndex;
				st = (UI.State)EditorGUI.EnumPopup( stateRect, GUIContent.none, st );
				sp.enumValueIndex = (int)st;
			}

			Rect idRect = new Rect( stateRect );
			{
				idRect.xMin = 0f;
				idRect.x += 110f;
				idRect.width = 350f;
			}

			sp = property.FindPropertyRelative( "mSprite" );
			if( null != sp )
			{
				EditorGUI.ObjectField( idRect, sp, new GUIContent( "Sprite." ) );
			}
		}
	}
	public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
	{
		return base.GetPropertyHeight( property, label );
	}
}