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
				stateRect.width		= 80f;
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
				idRect.y += 20;
				idRect.width = 80f;
				idRect.xMin = 0;
			}
			EditorGUI.DropShadowLabel( idRect, "Color." );

			Rect colorRect = new Rect( idRect );
			{
				colorRect.x += 120;
				colorRect.width = 250f;
			}
			sp = property.FindPropertyRelative( "mColor" );
			if( null != sp )
			{
				sp.colorValue = EditorGUI.ColorField( colorRect, sp.colorValue );
			}

			{
				idRect.y += 20f;
			}
			EditorGUI.DropShadowLabel( idRect, "Alpha." );

			Rect alphaRect = new Rect( idRect );
			{
				alphaRect.x += 120;
				alphaRect.width = 200f;
			}
			sp = property.FindPropertyRelative( "mAlpha" );
			if( null != sp )
			{
				sp.intValue = EditorGUI.IntField( alphaRect, GUIContent.none, sp.intValue );
			}

			//idRect = new Rect( rect );
			//{
			//	idRect.x -= 65;
			//	idRect.y += 20;
			//}
			//EditorGUI.DropShadowLabel( idRect, "Alpha." );

			//Rect colorRect = new Rect( rect );
			//{
			//	//colorRect.x += 230;
			//	colorRect.y += 20;
			//	//colorRect.width = 80f;
			//}
			//Rect idRect = new Rect( rect );
			//{
			//	idRect.x += 230;
			//	idRect.y += 20;
			//	idRect.width = 50f;
			//}
			//EditorGUI.DropShadowLabel( colorRect, "Color." );
			//sp = property.FindPropertyRelative( "mColor" );
			//if( null != sp )
			//{
			//	sp.colorValue = EditorGUI.ColorField( idRect, sp.colorValue );
			//}

			//SerializedProperty sp = property.FindPropertyRelative( "mCustom" );
			//if( null != sp )
			//{
			//	sp.boolValue = EditorGUI.Toggle( customRect, sp.boolValue );

			//	if( sp.boolValue == true )
			//	{
			//		UI.State st = UI.State.Normal;

			//		sp = property.FindPropertyRelative( "mState" );
			//		st = (UI.State)sp.enumValueIndex;
			//		st = (UI.State)EditorGUI.EnumPopup( stateRect, GUIContent.none, st );
			//		sp.enumValueIndex = (int)st;
			//	}
			//	else
			//	{
			//		sp = property.FindPropertyRelative( "mCustomState" );
			//		sp.intValue = EditorGUI.IntField( stateRect, GUIContent.none, sp.intValue );
			//	}
			//}

			//EditorGUI.DropShadowLabel( titleRect, "ID." );
			//sp = property.FindPropertyRelative( "mID" );
			//if( null != sp )
			//{
			//	sp.intValue = EditorGUI.IntField( idRect, GUIContent.none, sp.intValue );
			//}

			//sp = property.FindPropertyRelative( "mSprite" );

			//if( sp.boolValue == true )
			//{
			//	titleRect = new Rect( rect );
			//	{
			//		titleRect.y -= 2f;
			//		titleRect.x += 185;
			//		titleRect.width = 20f;
			//	}
			//	EditorGUI.DropShadowLabel( titleRect, "A." );
			//	alphaRect = new Rect( rect );
			//	{
			//		alphaRect.x += 200;
			//		alphaRect.width = 30f;
			//	}
			//	sp = property.FindPropertyRelative( "mAlpha" );
			//	if( null != sp )
			//	{
			//		sp.intValue = EditorGUI.IntField( alphaRect, GUIContent.none, sp.intValue );
			//	}
			//	titleRect = new Rect( rect );
			//	{
			//		titleRect.y -= 2f;
			//		titleRect.x += 235;
			//		titleRect.width = 20f;
			//	}
			//	EditorGUI.DropShadowLabel( titleRect, "G." );
			//	Rect grayRect = new Rect( rect );
			//	{
			//		grayRect.y -= 2f;
			//		grayRect.x += 252;
			//		grayRect.width = 20f;
			//	}
			//	sp = property.FindPropertyRelative( "mGrayScale" );
			//	sp.boolValue = EditorGUI.Toggle( grayRect, sp.boolValue );

			//	Rect spriteRect = new Rect( rect );
			//	{
			//		spriteRect.y -= 2f;
			//		spriteRect.x += 275;
			//		spriteRect.width = 20f;
			//	}
			//	sp = property.FindPropertyRelative( "mUseAtlas" );
			//	bool bSpriteEnabled = sp.boolValue = EditorGUI.Toggle( spriteRect, sp.boolValue );

			//	Rect spriteNameRect = new Rect( rect );
			//	{
			//		spriteNameRect.x += 295;
			//		spriteNameRect.width -= 300f;
			//	}
			//	sp = property.FindPropertyRelative( "mSpriteName" );
			//	if( bSpriteEnabled == false )
			//		EditorGUI.TextField( spriteNameRect, sp.stringValue, GUIStyle.none );
			//	else
			//		sp.stringValue = EditorGUI.TextField( spriteNameRect, sp.stringValue );
			//}
			//else
			//{
			//	titleRect = new Rect( rect );
			//	{
			//		titleRect.y -= 2f;
			//		titleRect.x += 185;
			//		titleRect.width = 20f;
			//	}
			//	EditorGUI.DropShadowLabel( titleRect, "A." );
			//	alphaRect = new Rect( rect );
			//	{
			//		alphaRect.x += 200;
			//		alphaRect.width = 30f;
			//	}
			//	sp = property.FindPropertyRelative( "mAlpha" );
			//	if ( null != sp )
			//	{
			//		sp.intValue = EditorGUI.IntField( alphaRect, GUIContent.none, sp.intValue );
			//	}
			//	titleRect = new Rect( rect );
			//	{
			//		titleRect.y -= 2f;
			//		titleRect.x += 235;
			//		titleRect.width = 20f;
			//	}
			//	EditorGUI.DropShadowLabel( titleRect, "G." );
			//	Rect grayRect = new Rect( rect );
			//	{
			//		grayRect.y -= 2f;
			//		grayRect.x += 252;
			//		grayRect.width -= 200f;
			//	}
			//	sp = property.FindPropertyRelative( "mGrayScale" );
			//	sp.boolValue = EditorGUI.Toggle( grayRect, sp.boolValue );
			//}
		}
	}
	public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
	{
		return base.GetPropertyHeight( property, label );
	}
}