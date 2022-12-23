namespace Gong.Build
{
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.Build.Reporting;

	public partial class AutoBuilder
	{
		static public int BuildFromFile( string path )
		{
			BuildInfo info = new BuildInfo();
			if( false == AutoSetting.ApplyFromFile( ref info, path ) )
				return -1;

			return BuildFromInfo( info );
		}
		static public int BuildFromArgs()
		{
			BuildInfo info = new BuildInfo();
			if( false == AutoSetting.ApplyFromArgs( ref info ) )
				return -1;

			return BuildFromInfo( info );
		}
		static public int BuildFromInfo( Gong.Build.BuildInfo info )
		{
			Log.Output( "####################################################################" );
			int result = GenericBuild( info );
			Log.Output( "####################################################################" );
			return result;
		}

		static private int GenericBuild( Gong.Build.BuildInfo info )
		{
			//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			if( info.buildType.HasFlag( Gong.Build.BuildType.NonBuild ) )
				info.buildType = Gong.Build.BuildType.None;

			if( info.buildType.HasFlag( Gong.Build.BuildType.AppBuild ) &&
				info.buildType.HasFlag( Gong.Build.BuildType.AssetBundle ) )
			{
				Log.Error( "<<error>> Build cannot proceed at the same time. : {0}"
					, info.buildType
					);

				info.buildType = Gong.Build.BuildType.None;
			}
			//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

			if( Gong.Build.BuildType.None == info.buildType )
			{
				Log.Output( "### Non Build : only build setting complete!!!" );
				return -1;
			}

			if( info.buildType.HasFlag( Gong.Build.BuildType.AppBuild ) )
			{
				int result = GenericAppBuild( info );
				{
					Log.Output( "### App Build {0}({1})!!! <<{2}>>"
						, result == 0 ? "success" : "failed"
						, result
						, info.buildTargetGroup
						);
				}
				return result;
			}
			if( info.buildType.HasFlag( Gong.Build.BuildType.AssetBundle ) )
			{
				int result = GenericABBuild( info );
				{
					Log.Output( "### AssetBundle Build {0}({1})!!! <<{2}>>"
						, result == 0 ? "success" : "failed"
						, result
						, info.buildTarget
						);
				}
				return result;
			}
			if( info.buildType.HasFlag( Gong.Build.BuildType.PatchOutput ) )
			{
				int result = GenericABPatch( info );
				{
					Log.Output( "### AssetBundle PatchOutput {0}({1})!!! <<{2}>>"
						, result == 0 ? "success" : "failed"
						, result
						, info.buildTarget
						);
				}
				return result;
			}
			//!< Upload ???
			{
				Log.Output( "### {0} : complete!!!", info.buildType );
				return 0;
			}
		}
	}

	public partial class AutoBuilder
	{
		static private int GenericAppBuild( Gong.Build.BuildInfo info )
		{
			if( false == info.buildType.HasFlag( BuildType.AppBuild ) )
				return 10;

			if( string.IsNullOrEmpty( info.locationPathName ) )
				return 11;

			switch( info.buildTargetGroup )
			{
			case BuildTargetGroup.Android:
				break;
			case BuildTargetGroup.iOS:
				PlayerSettings.iOS.sdkVersion = info.buildType.HasFlag( Gong.Build.BuildType.IosSimulatorSDK ) ? iOSSdkVersion.SimulatorSDK : iOSSdkVersion.DeviceSDK;
				Log.Output( "**** SET iOS SDK version : {0}!!", PlayerSettings.iOS.sdkVersion );

				break;
			}

			BuildPlayerOptions options = new BuildPlayerOptions();
			{
				options.locationPathName = info.locationPathName;
				options.targetGroup = info.buildTargetGroup;
				options.target = info.buildTarget;
				options.scenes = info.buildScenes;
				options.options = info.buildOptions;
			}
			return GenericAppBuild( options );
		}
		static private int GenericAppBuild( BuildPlayerOptions options )
		{
			if( options.scenes == null )
			{
				List<string> scenes = new List<string>();
				{
					foreach( EditorBuildSettingsScene scene in EditorBuildSettings.scenes )
					{
						if( scene.enabled )
						{
							if( 0 < scene.path.Length )
								scenes.Add( scene.path );
						}
							
					}
				}
				options.scenes = scenes.ToArray();
			}

			Log.Output( "**** local_path_name : {0}", options.locationPathName );
			Log.Output( "**** app_target_group : {0}", options.targetGroup );
			Log.Output( "**** app_target : {0}", options.target );
			
			int index = 0;
			foreach( string scene in options.scenes )
			{
				Log.Output( "**** {0} app_Scene : {1}", ++index, scene );
			}			
			Log.Output( "**** app_Options : {0}", options.options );
			Log.Output( "**** app_assetBundleManifestPath : {0}", options.assetBundleManifestPath );
			
			var report = BuildPipeline.BuildPlayer( options );
			var summary = report.summary;

			Log.Output( "**** summary.result : {0}", summary.result );

			switch( summary.result )
			{
			case BuildResult.Succeeded:
				return 0;

			case BuildResult.Failed:
				{
					foreach( var step in report.steps )
					{
						foreach( var message in step.messages )
						{
							Log.Output( "**** {0}", message );
						}
					}
				}
				return 1;

			case BuildResult.Cancelled:
			case BuildResult.Unknown:
			default:
				return 2;
			}
		}
	}

	public partial class AutoBuilder
	{
#if _MLB_
		static private int GenericABBuild( Gong.Build.BuildInfo info )
		{
			if( false == info.buildType.HasFlag( BuildType.AssetBundle ) )
				return 10;

			if( string.IsNullOrEmpty( info.abBuildOutputPath ) )
				return 11;

			if( false == System.IO.Directory.Exists( info.abBuildOutputPath ) )
			{
				System.IO.Directory.CreateDirectory( info.abBuildOutputPath );
			}

			BuildTarget activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
			if( activeBuildTarget != info.buildTarget )
			{
				EditorUserBuildSettings.SwitchActiveBuildTarget( info.buildTargetGroup, info.buildTarget );
				{
					Log.Output( "Platform switched to {0}({1})"
						, info.buildTargetGroup
						, info.buildTarget
						);
				}
			}

			//!< AssetBundle Build.
			AssetBundleManifest mf = BuildPipeline.BuildAssetBundles
				( info.abBuildOutputPath
				, info.abBuildOptions
				, info.buildTarget
				);

			if( null == mf )
				return 1;

			if( false == info.buildType.HasFlag( BuildType.PatchOutput ) )
				return 0;

			//!< AssetBundle Patch.
			bool result = Gong.Build.Utility.OutputAssetBundlePatchList
				( mf
				, info.abBuildOutputPath
				, info.abPatchOutputPath
				, info.abPatchListFile
				, info.abPatchRevision
				, info.abPatchRootURL
				, info.abPatchSubURL
				, info.abPatchFileExtension
				);
			if( false == result )
				return 2;

			return 0;
		}
		static private int GenericABPatch( Gong.Build.BuildInfo info )
		{
			bool result = Gong.Build.Utility.OutputAssetBundlePatchList
				( info.abBuildOutputPath
				, info.abPatchOutputPath
				, info.abPatchListFile
				, info.abPatchRevision
				, info.abPatchRootURL
				, info.abPatchSubURL
				, info.abPatchFileExtension
				);

			if( false == result )
				return 2;

			return 0;
		}
#else
		static private int GenericABBuild( Gong.Build.BuildInfo info )
		{
			if( false == info.buildType.HasFlag( BuildType.AssetBundle ) )
				return 10;

			if( string.IsNullOrEmpty( info.abBuildOutputPath ) )
				return 11;

			if( false == System.IO.Directory.Exists( info.abBuildOutputPath ) )
			{
				System.IO.Directory.CreateDirectory( info.abBuildOutputPath );
			}

			BuildTarget activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
			if( activeBuildTarget != info.buildTarget )
			{
				EditorUserBuildSettings.SwitchActiveBuildTarget( info.buildTargetGroup, info.buildTarget );
				{
					Log.Output( "Platform switched to {0}({1})"
						, info.buildTargetGroup
						, info.buildTarget
						);
				}
			}

			//!< AssetBundle Build.
			AssetBundleManifest mf = BuildPipeline.BuildAssetBundles
				( info.abBuildOutputPath
				, info.abBuildOptions
				, info.buildTarget
				);

			if( null == mf )
				return 1;

			if( false == info.buildType.HasFlag( BuildType.PatchOutput ) )
				return 0;

			//!< AssetBundle Patch.
			bool result = Gong.Build.Utility.OutputAssetBundlePatchList
				( mf
				, info.abBuildOutputPath
				, info.abPatchOutputPath
				, info.abPatchListFile
				, info.abPatchRevision
				, info.abPatchRootURL
				, info.abPatchSubURL
				, info.projectType
				, info.abPatchFileExtension
				);
			if( false == result )
				return 2;

			return 0;
		}

		static private int GenericABPatch( Gong.Build.BuildInfo info )
		{
			bool result = Gong.Build.Utility.OutputAssetBundlePatchList
				( info.abBuildOutputPath
				, info.abPatchOutputPath
				, info.abPatchListFile
				, info.abPatchRevision
				, info.abPatchRootURL
				, info.abPatchSubURL
				, info.projectType
				, info.abPatchFileExtension
				);

			if( false == result )
				return 2;

			return 0;
		}
#endif
	}
}
