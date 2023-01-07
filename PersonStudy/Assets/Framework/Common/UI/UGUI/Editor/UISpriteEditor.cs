using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using UI;

[CanEditMultipleObjects]
[CustomEditor( typeof( UI.UISprite ), true )]
public class UISpritelEditor : UnityEditor.UI.ImageEditor
{
	public ReorderableList		colorList;
	public SerializedProperty	colorProp;

	public ReorderableList		spriteList;
	public SerializedProperty	spriteProp;

	UISprite t = null;

	protected override void OnEnable()
	{
		base.OnEnable();

		t = target as UISprite;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUI.contentColor = new Color( 215f / 255f, 255f / 255f, 150f / 255f, 1f );
		GUI.backgroundColor = new Color( 137f / 255f, 206f / 255f, 110f / 255f, 1f );

		serializedObject?.Update();

		DrawColorReorderbleList();
		DrawSpriteReorderbleList();
	
		colorList?.DoLayoutList();
		spriteList?.DoLayoutList();

		serializedObject?.ApplyModifiedProperties();
	}

	private void DrawColorReorderbleList()
	{
		if( null == colorProp )
			colorProp = serializedObject.FindProperty( "mColors" );

		if( null == colorList )
		{
			colorList = new ReorderableList( serializedObject, colorProp );
			colorList.elementHeight = 30f;
			colorList.drawElementCallback = ( Rect rect, int index, bool isActive, bool isFocused ) =>
			{
				SerializedProperty element = colorProp.GetArrayElementAtIndex( index );
				rect.height -= 4;
				rect.y += 2;
				EditorGUI.PropertyField( rect, element );
			};
			colorList.onAddCallback = ( ReorderableList list ) =>
			{
				colorProp.InsertArrayElementAtIndex( colorProp.arraySize );
				list.index = colorProp.arraySize - 1;

				SerializedProperty unitProp = colorProp.GetArrayElementAtIndex( list.index );
				if( null != unitProp )
				{
					SerializedProperty _sp = unitProp.FindPropertyRelative( "mState" );
					if( null != _sp ) _sp.enumValueIndex = 0;

					_sp = unitProp.FindPropertyRelative( "mColor" );
					if( null != _sp ) _sp.colorValue = Color.white;

					_sp = unitProp.FindPropertyRelative( "mAlpha" );
					if( null != _sp ) _sp.intValue = 255;
				}
			};
			colorList.drawHeaderCallback = ( Rect rect ) =>
			{
				EditorGUI.LabelField( rect, "Color" );
			};
		}
	}
	private void DrawSpriteReorderbleList()
	{
		if( null == spriteProp )
			spriteProp = serializedObject.FindProperty( "mSprite" );

		if( null == spriteList )
		{
			spriteList = new ReorderableList( serializedObject, spriteProp );
			spriteList.elementHeight = 30f;
			spriteList.drawElementCallback = ( Rect rect, int index, bool isActive, bool isFocused ) =>
			{
				SerializedProperty element = spriteProp.GetArrayElementAtIndex( index );
				rect.height -= 4;
				rect.y += 2;
				EditorGUI.PropertyField( rect, element );
			};
			spriteList.onAddCallback = ( ReorderableList list ) =>
			{
				spriteProp.InsertArrayElementAtIndex( spriteProp.arraySize );
				list.index = spriteProp.arraySize - 1;

				SerializedProperty unitProp = spriteProp.GetArrayElementAtIndex( list.index );
				if( null != unitProp )
				{
					SerializedProperty _sp = unitProp.FindPropertyRelative( "mState" );
					if( null != _sp ) _sp.enumValueIndex = 0;
				}
			};
			spriteList.drawHeaderCallback = ( Rect rect ) =>
			{
				EditorGUI.LabelField( rect, "Sprite" );
			};
		}
	}
}