using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class Menu
{
	public class AutoBuilder
	{
		const string MenuPath = "[Gong.Modules]/Auto Builder/";

		[MenuItem( MenuPath + "Setup From File" )]
		static void OnSetupFromFile()
		{
			_BuildFromFile( true );
		}

		[MenuItem( MenuPath + "Build From File" )]
		static void OnBuildFromFile()
		{
			_BuildFromFile( false );
		}

		static private string _buildSettingFileExtension = "gBuild";
		static private void _BuildFromFile( bool isNonBuild )
		{
			string fileFullPath = EditorUtility.OpenFilePanel( "Build Setting File", null, _buildSettingFileExtension );
			if( string.IsNullOrEmpty( fileFullPath ) )
				return;

			Gong.Build.BuildInfo info = isNonBuild
				? new Gong.Build.BuildInfo( Gong.Build.BuildType.NonBuild )
				: new Gong.Build.BuildInfo()
				;

			if( Gong.Build.AutoSetting.ApplyFromFile( ref info, fileFullPath ) )
			{
				Gong.Build.AutoBuilder.BuildFromInfo( info );
			}
		}
	}
}
