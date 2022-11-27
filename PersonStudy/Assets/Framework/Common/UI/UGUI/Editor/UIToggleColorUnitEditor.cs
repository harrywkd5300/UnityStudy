//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomPropertyDrawer( typeof( UI.UIToggle ) )]
//public class ToggleGxColorUnitEditor : PropertyDrawer
//{
//	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
//	{
//		GUIContent gc = new GUIContent();

//		using ( new EditorGUI.PropertyScope( position, label, property ) )
//		{
//			EditorGUIUtility.labelWidth = 80;
//			position.height = EditorGUIUtility.singleLineHeight;
//			float height = EditorGUIUtility.singleLineHeight + 4;
//			float viewWidth = EditorGUIUtility.currentViewWidth;

//			SerializedProperty sp = property.FindPropertyRelative( "mTarget" );
//			Rect rect = new Rect( position );
//			{
//				rect.y += 2f;
//				EditorGUI.ObjectField( rect, sp, new GUIContent( "Target" ) );
//			}
//			/*
//			Rect idRect = new Rect( rect );
//			{
//				idRect.width = 130f;
//			}
//			Rect alphaRect = new Rect( rect );
//			{
//				alphaRect.x += 140;
//				alphaRect.width -= 145f;
//			}

//			SerializedProperty _sp = null;

//			sp = property.FindPropertyRelative( "mNormal" );
//			{
//				_sp = sp.FindPropertyRelative( "mID" );
//				if( null != _sp )
//				{
//					idRect.y += height;
//					_sp.intValue = EditorGUI.IntField( idRect, "Normal", _sp.intValue );
//				}

//				_sp = sp.FindPropertyRelative( "mAlpha" );
//				if( null != _sp )
//				{
//					alphaRect.y += height;
//					_sp.intValue = EditorGUI.IntSlider( alphaRect, "", _sp.intValue, 0, 255 );
//				}
//			}

//			sp = property.FindPropertyRelative( "mPressed" );
//			{
//				_sp = sp.FindPropertyRelative( "mID" );
//				if( null != _sp )
//				{
//					idRect.y += height;
//					_sp.intValue = EditorGUI.IntField( idRect, "Pressed", _sp.intValue );
//				}

//				_sp = sp.FindPropertyRelative( "mAlpha" );
//				if( null != _sp )
//				{
//					alphaRect.y += height;
//					_sp.intValue = EditorGUI.IntSlider( alphaRect, "", _sp.intValue, 0, 255 );
//				}
//			}

//			sp = property.FindPropertyRelative( "mDisable" );
//			{
//				_sp = sp.FindPropertyRelative( "mID" );
//				if( null != _sp )
//				{
//					idRect.y += height;
//					_sp.intValue = EditorGUI.IntField( idRect, "Disable", _sp.intValue );
//				}

//				_sp = sp.FindPropertyRelative( "mAlpha" );
//				if( null != _sp )
//				{
//					alphaRect.y += height;
//					_sp.intValue = EditorGUI.IntSlider( alphaRect, "", _sp.intValue, 0, 255 );
//				}
//			}
//			*/
//		}
//	}
//}
