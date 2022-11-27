//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEditor.UI;
//using UnityEditorInternal;
//using Gong.UI;
//using System.Linq;

//[CanEditMultipleObjects]
//[CustomEditor( typeof( Gong.UI.UIToggle ), true )]
//public class ToggleGxEditor : ToggleEditor
//{
//	public ReorderableList		colorList;
//	public SerializedProperty	colorProp;
	
//	public SerializedProperty	uIEffectSound;
//	public SerializedProperty	linkedObject;
//	public SerializedProperty	useTween;
//	public SerializedProperty	tweenScale;

//	protected override void OnEnable()
//	{
//		base.OnEnable();

//		DrawColorReorderbleList();
//	}

//	public override void OnInspectorGUI()
//	{
//		base.OnInspectorGUI();

//		GUI.contentColor = new Color( 215f / 255f, 255f / 255f, 150f / 255f, 1f );
//		GUI.backgroundColor = new Color( 137f / 255f, 206f / 255f, 110f / 255f, 1f );

//		serializedObject.Update();
//		DrawUIEffectSound();
//		DrawActiveObject();
//		DrawTweenReorderbleList();
//		colorList?.DoLayoutList();
//		serializedObject.ApplyModifiedProperties();
//	}
//	private void DrawUIEffectSound()
//	{
//		if( null == uIEffectSound )
//			uIEffectSound = serializedObject.FindProperty( "mUIEffectSound" );
//		EditorGUILayout.PropertyField( uIEffectSound, new GUIContent( "* Toggle Click Sound" ) );

//		EditorGUILayout.Separator();
//	}
//	private void DrawActiveObject()
//	{
//		if( null == linkedObject )
//			linkedObject = serializedObject.FindProperty( "mObject" );
//		EditorGUILayout.PropertyField( linkedObject, new GUIContent( "Linked Object" ) );

//		EditorGUILayout.Separator();
//	}
//	private void DrawTweenReorderbleList()
//	{
//		EditorGUILayout.BeginHorizontal();

//		if( null == useTween )
//			useTween = serializedObject.FindProperty( "mUseTween" );
//		EditorGUILayout.PropertyField( useTween, new GUIContent( "Use Pressed Tween" ) );

//		if( null == tweenScale )
//			tweenScale = serializedObject.FindProperty( "mTweenScale" );
//		EditorGUILayout.PropertyField( tweenScale, new GUIContent( "" ) );

//		EditorGUILayout.EndHorizontal();
//		EditorGUILayout.Separator();
//	}
//	private void DrawColorReorderbleList()
//	{
//		if ( null == colorProp )
//			colorProp = serializedObject.FindProperty( "mColorOptions" );

//		if ( null == colorList )
//		{
//			colorList = new ReorderableList( serializedObject, colorProp );
//			colorList.elementHeight = 25f;
//			colorList.drawElementCallback = ( Rect rect, int index, bool isActive, bool isFocused ) =>
//			{
//				SerializedProperty element = colorProp.GetArrayElementAtIndex( index );
//				rect.height -= 4;
//				rect.y += 2;
//				EditorGUI.PropertyField( rect, element );
//			};
//			colorList.onAddCallback = ( ReorderableList list ) =>
//			{
//				colorProp.InsertArrayElementAtIndex( colorProp.arraySize );
//				list.index = colorProp.arraySize - 1;

//				SerializedProperty unitProp = colorProp.GetArrayElementAtIndex( list.index );
//				if ( null != unitProp )
//				{
//					SerializedProperty _sp = unitProp.FindPropertyRelative( "mTarget" );
//					if ( null != _sp ) _sp.objectReferenceValue = null;
//				}
//			};
//			colorList.drawHeaderCallback = ( Rect rect ) =>
//			{
//				EditorGUI.LabelField( rect, "State Able GameObject" );
//			};
//		}
//	}
//}
