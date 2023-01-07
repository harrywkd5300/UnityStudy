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

			Rect stateRect		= new Rect( position );
			{
				stateRect.width = 80f;
			}

			SerializedProperty sp = property.FindPropertyRelative( "mState" );
			if( null != sp )
			{
				UI.State st = UI.State.Normal;

				st = (UI.State)sp.enumValueIndex;
				st = (UI.State)EditorGUI.EnumPopup( stateRect, GUIContent.none, st );
				sp.enumValueIndex = (int)st;
			}

#if _AGO_
			Rect useAtlasRect	= new Rect( stateRect );
			{
				useAtlasRect.x		+= 100f;
				useAtlasRect.width	= 100f;
			}
			Rect spriteRect		= new Rect( stateRect ); 
			{
				spriteRect.y		+= 25f;
				spriteRect.width	= 400f;
			}
			Rect atlasTypeRect	= new Rect( stateRect );
			{
				atlasTypeRect.y += 25f;
				atlasTypeRect.width = 80f;
			}
			Rect atlasNameRect = new Rect( atlasTypeRect );
			{
				atlasNameRect.x += 100f;
				//atlasNameRect.y += 25f;
				atlasNameRect.width = 200f;
			}

			bool bSpriteEnabled = false;

			sp = property.FindPropertyRelative( "mUseAtlas" );
			if( null != sp )
			{
				sp.boolValue = EditorGUI.ToggleLeft( useAtlasRect, "mUseAtlas", sp.boolValue );
				bSpriteEnabled = sp.boolValue;
			}

			if( bSpriteEnabled )
			{
				SpriteAtlasType st = SpriteAtlasType.None;

				sp = property.FindPropertyRelative( "mSpriteAltasType" );
				if( null != sp )
				{
					st = (SpriteAtlasType)sp.enumValueIndex;
					st = (SpriteAtlasType)EditorGUI.EnumPopup( atlasTypeRect, GUIContent.none, st );
					sp.enumValueIndex = (int)st;
				}

				sp = property.FindPropertyRelative( "mSpriteName" );
				if( null != sp )
				{
					sp.stringValue = EditorGUI.TextField( atlasNameRect, "AtlasName", sp.stringValue );
				}

			}
			else
			{
				sp = property.FindPropertyRelative( "mSprite" );
				if( null != sp )
				{
					EditorGUI.ObjectField( spriteRect, sp, new GUIContent( "Sprite." ) );
				}
			}
#else
			Rect spriteRect = new Rect( stateRect );
			{
				spriteRect.x += 100f;
				spriteRect.width = 300f;
			}

			sp = property.FindPropertyRelative( "mSprite" );
			if( null != sp )
			{
				EditorGUI.ObjectField( spriteRect, sp, new GUIContent( "Sprite." ) );
			}
#endif


			//Rect spriteRect = new Rect( rect );
			//{
			//	spriteRect.y		+= 25f;

			//}
			//sp = property.FindPropertyRelative( "mUseAtlas" );
			//bool bSpriteEnabled = sp.boolValue = EditorGUI.Toggle( spriteRect, sp.boolValue );

			//Rect spriteNameRect = new Rect( spriteRect );
			//{
			//	spriteNameRect.x += 25;
			//}
			//sp = property.FindPropertyRelative( "mSpriteName" );
			//if( bSpriteEnabled == false )
			//	EditorGUI.TextField( spriteNameRect, sp.stringValue, GUIStyle.none );
			//else
			//	sp.stringValue = EditorGUI.TextField( spriteNameRect, sp.stringValue );
		}
	}
	public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
	{
		return base.GetPropertyHeight( property, label );
	}
}