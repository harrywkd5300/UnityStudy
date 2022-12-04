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
			Utility.Editor.OnLayoutLabelField( Color.green, "* Version Info" );

			EditorGUILayout.BeginVertical( "box" );
			{
				Utility.Editor.OnLayoutLabelField( Color.white, "* Game" );
				Utility.Editor.OnLayoutLabelField( Color.white, "v." + configData.gameVersion );
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical( "box" );
			{
				configData.isAssetBundle	= EditorGUILayout.Toggle( "AssetBundle", configData.isAssetBundle );
				configData.isPatchMode		= EditorGUILayout.Toggle( "PatchMode", configData.isPatchMode );
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical( "box" );
			{
				if( GUILayout.Button( new GUIContent( "Save" ), GUILayout.Width( 50f ) ) )
				{
					if( true == JsonWriter.Write( configData, Config.rootPath, Config.rootfile ) )
					{
						Utility.Editor.ShowMessageBox( " Save success!" );
					}
				}
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();
	}
}