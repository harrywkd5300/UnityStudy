using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer( typeof( UI.UIButton.ColorOptions ) )]
public class ButtonGxColorUnitEditor : PropertyDrawer
{
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
	{
		GUIContent gc = new GUIContent();

		using( new EditorGUI.PropertyScope( position, label, property ) )
		{
			EditorGUIUtility.labelWidth = 80;
			position.height = EditorGUIUtility.singleLineHeight;
			float height = EditorGUIUtility.singleLineHeight + 4;
			float viewWidth = EditorGUIUtility.currentViewWidth;

			SerializedProperty sp = property.FindPropertyRelative( "mTarget" );
			Rect rect = new Rect( position );
			{
				rect.y += 2f;
				EditorGUI.ObjectField( rect, sp, new GUIContent( "Target" ) );
			}
		}
	}
}