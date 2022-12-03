using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class ConfigEditorWindow : EditorWindow
{
	[MenuItem( "[Config]/Config Setting" )]
	static public void Open()
	{
		ConfigEditorWindow window = ConfigEditorWindow.GetWindow<ConfigEditorWindow>();
		if( null != window )
			window.Initialize();
	}
}

public partial class ConfigEditorWindow : EditorWindow
{
	private ConfigData data;
	public ConfigData configData { get { return data; } }

	private void Initialize()
	{
		data = Config.Load();
	}
	private void OnGUI()
	{
		if( null == configData )
			return;

		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical( "box" );
		{
			GUI.color = Color.green;
			EditorGUILayout.LabelField( "* Version Info" );
			GUI.color = Color.white;

			EditorGUILayout.BeginVertical( "box" );
			{
				EditorGUILayout.LabelField( "* Game" );
				GUILayout.Label( "v." + configData.gameVersion );
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();
	}
}