using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer( typeof( UI.ColorUnit ) )]
public class ColorUnitEditor : PropertyDrawer
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
				idRect.x += 180f;
				idRect.width = 40f;
				idRect.xMin = 0f;
			}
			EditorGUI.DropShadowLabel( idRect, "Color." );

			Rect colorRect = new Rect( stateRect );
			{
				colorRect.x += 120;
				colorRect.width = 150f;
			}
			sp = property.FindPropertyRelative( "mColor" );
			if( null != sp )
			{
				sp.colorValue = EditorGUI.ColorField( colorRect, sp.colorValue );
			}

			Rect idAlphaRect = new Rect( stateRect );
			{
				idAlphaRect.x += 280;
				idAlphaRect.width = 40f;
			}
			EditorGUI.DropShadowLabel( idAlphaRect, "Alpha." );

			Rect alphaRect = new Rect( idAlphaRect );
			{
				alphaRect.x += 40;
				alphaRect.width = 80f;
			}
			sp = property.FindPropertyRelative( "mAlpha" );
			if( null != sp )
			{
				sp.intValue = EditorGUI.IntField( alphaRect, GUIContent.none, sp.intValue );
			}
		}
	}
	public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
	{
		return base.GetPropertyHeight( property, label );
	}
}