using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;
using UI;

[CanEditMultipleObjects]
[CustomEditor( typeof( UI.UIButton ), true )]
public class ButtonGxEditor : ButtonEditor
{
	public ReorderableList		colorList;
	public SerializedProperty	colorProp;

	UIButton t = null;

	protected override void OnEnable()
	{
		base.OnEnable();

		t = target as UIButton;

		DrawColorReorderbleList();
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUI.contentColor = new Color( 215f / 255f, 255f / 255f, 150f / 255f, 1f );
		GUI.backgroundColor = new Color( 137f / 255f, 206f / 255f, 110f / 255f, 1f );

		serializedObject.Update();

		DrawTweenReorderbleList();

		colorList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}

	private void DrawColorReorderbleList()
	{
		if( null == colorProp )
			colorProp = serializedObject.FindProperty( "mColorOptions" );

		if( null == colorList )
		{
			colorList = new ReorderableList( serializedObject, colorProp );
			colorList.elementHeight = 25f;
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
					SerializedProperty _sp = unitProp.FindPropertyRelative( "mTarget" );
					if( null != _sp ) _sp.objectReferenceValue = null;
				}
			};
			colorList.drawHeaderCallback = ( Rect rect ) =>
			{
				EditorGUI.LabelField( rect, "State Able GameObject" );
			};
		}
	}
	private void DrawTweenReorderbleList()
	{
		EditorGUILayout.Space();
		t.mUseTween = EditorGUILayout.Toggle( "Use Pressed Tween", t.mUseTween );
		EditorGUILayout.Space();
	}
}