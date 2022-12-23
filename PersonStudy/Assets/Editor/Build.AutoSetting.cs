namespace Gong.Build
{
	using System;
	using System.IO;
	using System.Text;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	//using GooglePlayServices;

	[Flags]
	public enum BuildType
	{
		None = 0,
		AppBuild = 1,
		AssetBundle = 2,
		PatchOutput = 4,
		Upload = 8,

		IosSimulatorSDK = 64,

		NonBuild = 256,
		NonCopy = 512,
	}

	public struct BuildInfo
	{
		public BuildType				buildType;
		public string					locationPathName;
		public BuildOptions				buildOptions;
		public string[]					buildScenes;
		public BuildTarget				buildTarget;
		public BuildTargetGroup			buildTargetGroup;
		public string					projectType;

		public string					abSetupExclude;
		public BuildAssetBundleOptions	abBuildOptions;
		public string					abBuildOutputPath;

		public string					abPatchOutputPath;
		public string					abPatchListFile;
		public string					abPatchRevision;
		public string					abPatchRootURL;
		public string					abPatchSubURL;
		public string					abPatchFileExtension;

		public string					ftpDomain;
		public string					ftpUserName;
		public string					ftpPassword;

		public string					keystoreName;
		public string					keystorePass;
		public string					keyaliasName;
		public string					keyaliasPass;

		public BuildInfo( BuildType buildType = BuildType.None )
		{
			this.buildType = buildType;
			this.locationPathName = string.Empty;
			this.buildOptions = BuildOptions.None;
			this.buildScenes = null;
			this.buildTarget = BuildTarget.NoTarget;
			this.buildTargetGroup = BuildTargetGroup.Unknown;
			this.projectType	= string.Empty;

			this.abSetupExclude = string.Empty;
			this.abBuildOptions = BuildAssetBundleOptions.None;
			this.abBuildOutputPath = string.Empty;

			this.abPatchOutputPath = string.Empty;
			this.abPatchListFile = string.Empty;
			this.abPatchRevision = string.Empty;
			this.abPatchRootURL = string.Empty;
			this.abPatchSubURL = string.Empty;
			this.abPatchFileExtension = string.Empty;

			this.ftpDomain = string.Empty;
			this.ftpUserName = string.Empty;
			this.ftpPassword = string.Empty;

			this.keystoreName = string.Empty;
			this.keystorePass = string.Empty;
			this.keyaliasName = string.Empty;
			this.keyaliasPass = string.Empty;
		}
	}

	public partial class AutoSetting
	{
		static public bool ApplyFromFile( ref BuildInfo info, string file )
		{
			Log.Output( "Setting file load. : {0}", file );

			ISetter it = CreateSetter( KeyType.BuildSettingFile, file );
			if( it == null )
				return false;

			if( false == it.Action( ref info ) )
			{
				Log.Error( "Setting abort. ~~~~ ~~~~ ~~~~ ~~~~ ~~~~" );
				return false;
			}

			Log.Output( "Setting complete. ~~~~ ~~~~ ~~~~ ~~~~ ~~~~" );
			return true;
		}
		static public bool ApplyFromArgs( ref BuildInfo info )
		{
			List<ISetter> list = new List<ISetter>( (int)KeyType.Max );
			{
				string[] args = System.Environment.GetCommandLineArgs();
				for( int i = 0; i < args.Length; i++ )
				{
					if( args[ i ].Contains( "--" ) && args.Length > i + 1 )
					{
						ISetter obj = CreateSetter( args[ i ], args[ i + 1 ] );
						if( null != obj )
							list.Add( obj );
					}
				}
				if( 0 == list.Count )
					return false;
			}

			foreach( ISetter it in list )
			{
				if( false == it.Action( ref info ) )
				{
					Log.Error( "Setting abort. ~~~~ ~~~~ ~~~~ ~~~~ ~~~~" );
					return false;
				}
			}

			Log.Output( "Stting complete. ~~~~ ~~~~ ~~~~ ~~~~ ~~~~" );
			return true;
		}
	}

	public partial class AutoSetting
	{
		interface ISetter
		{
			bool Active( string value );
			bool Action( ref BuildInfo info );
		}
		enum KeyType
		{
			None,
			//!< Build
			AutoBuilder				, //!< 자동 빌드 실행.
			//!< Common
			BuildSettingFile		, //!< 파일 로드하여 아래 설정자들을 생성함.
			BuildType				, //!< 빌드 타입 설정 조합.
			BuildTargetGroup		, //!< 빌드 타겟 그룹 설정.
			BuildTarget				, //!< 빌드 타겟 설정.
			AppID					, //!< AppID 설정. 기본(com.com2ususa.mlbpiul.android.google.global.normal)
			ProjectType				, //!< 프로젝트 타입

			//!< AppBuild
			LocationPathName		, //!< 빌드 파일 출력 경로명. Assets 상위 폴더에 위치함.
			BuildOptions			, //!< 빌드 옵션 설정. 복수의 옵션 가능 (구분자 ,;)
			BuildSkipScenes			, //!< 빌드 씬들 설정. 키워드 포함시 무시. 복수의 키워드 가능 (구분자 ,;)
			BuildVersion			, //!< 빌드 버전
			BuildVersionCode		, //!< 빌드 버전 코드.
			BuildAppBundle			, //!< 안드로이드 AAB 빌드. (Ture/False)
			BuildScriptSymbols		, //!< 빌드 스크립팅 심벌 설정.
			//!< AssetBundle Setup
			BundleRemoveAll			, //!< 에셋 번들 모두 해제.
			BundleSetupExclude		, //!< 에셋 번들 설정 제외 폴더.
			BundleSetupFolder		, //!< 에셋 번들 자산 설정 폴더.
			BundleSetupScenes		, //!< 에셋 번들 장면 설정 폴더.
			//!< AssetBundle Build
			BundleOutputPath		, //!< 에셋 번들 빌드 경로. Assets 상위 폴더에 위치함.
			BundleOptions			, //!< 에셋 번들 빌드 옵션.
			//!< AssetBundle Patch
			PatchOutputPath			, //!< 에셋 번들 - 패치 출력 경로. Assets 상위 폴더에 위치함.
			PatchListFile			, //!< 에셋 번들 - 패치 목록 파일.
			PatchRevision			, //!< 에셋 번들 - 패치 리비전.
			PatchRootURL			, //!< 에셋 번들 - 패치 루트 URL.
			PatchSubURL				, //!< 에셋 번들 - 패치 서브 URL.
			PatchFileExtension		, //!< 에셋 번들 - 패치 파일 확장자.
			//!< Upload To FTP
			UploadDomain			, //!< 업로드 도메인
			UploadUserName			, //!< 업로드 유저명
			UploadPassword			, //!< 업로드 비밀번호
			UploadPath				, //!< 업로드 경로	(Json Format: "src":"...", "url":"...")
			//!< IO
			CopyFile				, //!< 파일 복제. (Json Format)
			//!< Keystore
			Keystore				, //!< Android Keystore 정보 등록
			Max,
		}

		static ISetter CreateSetter( string key, string val )
		{
			if( true == string.IsNullOrEmpty( key ) )
				return null;

			KeyType tmp;
			key = key.Trim( ' ', '-', '*', '\t' );
			if( true != Enum.TryParse<KeyType>( key, out tmp ) )
			{
				Log.Warning( "Setter KeyType parsing failed! : {0}", key );
				return null;
			}

			return CreateSetter( tmp, val );
		}
		static ISetter CreateSetter( KeyType key, string val )
		{
			ISetter obj = null;
			{
				if( true == string.IsNullOrEmpty( val ) )
					return null;

				switch( key )
				{
					//!< Build
				case KeyType.AutoBuilder 			: obj = new xAutoBuilder			(); break;
					//!< Common
				case KeyType.BuildSettingFile 		: obj = new xBuildSettingFile		(); break;
				case KeyType.BuildType				: obj = new xBuildType				(); break;
				case KeyType.BuildTargetGroup		: obj = new xBuildTargetGroup		(); break;
				case KeyType.BuildTarget			: obj = new xBuildTarget			(); break;
				case KeyType.AppID					: obj = new xAppID					(); break;
				case KeyType.ProjectType			: obj = new xProjectType			(); break;
					//!< AppBuild
				case KeyType.LocationPathName		: obj = new xLocationPathName		(); break;
				case KeyType.BuildOptions			: obj = new xBuildOptions			(); break;
				case KeyType.BuildSkipScenes		: obj = new xBuildSkipScenes		(); break;
				case KeyType.BuildVersion			: obj = new xBuildVersion			(); break;
				case KeyType.BuildVersionCode		: obj = new xBuildVersionCode		(); break;
				case KeyType.BuildAppBundle			: obj = new xBuildAppBundle			(); break;
				case KeyType.BuildScriptSymbols		: obj = new xBuildScriptSymbols		(); break;
					//!< AssetBundle Setup
				case KeyType.BundleRemoveAll		: obj = new xBundleRemoveAll		(); break;
				case KeyType.BundleSetupExclude		: obj = new xBundleSetupExclude		(); break;
				case KeyType.BundleSetupFolder		: obj = new xBundleSetupFolder		(); break;
				case KeyType.BundleSetupScenes		: obj = new xBundleSteupScenes		(); break;
					//!< AssetBundle Build
				case KeyType.BundleOutputPath		: obj = new xBundleOutputPath		(); break;
				case KeyType.BundleOptions			: obj = new xBundleOptions			(); break;
					//!< AssetBundle Patch
				case KeyType.PatchOutputPath		: obj = new xPatchOutputPath		(); break;
				case KeyType.PatchListFile			: obj = new xPatchListFile			(); break;
				case KeyType.PatchRevision			: obj = new xPatchRevision			(); break;
				case KeyType.PatchRootURL			: obj = new xPatchRootURL			(); break;
				case KeyType.PatchSubURL			: obj = new xPatchSubURL			(); break;
				case KeyType.PatchFileExtension		: obj = new xPatchFileExtension		(); break;
					//!< Upload To FTP
				case KeyType.UploadDomain			: obj = new xUploadDomain			(); break;
				case KeyType.UploadUserName			: obj = new xUploadUserName			(); break;
				case KeyType.UploadPassword			: obj = new xUploadPassword			(); break;
				case KeyType.UploadPath				: obj = new xUploadPath				(); break;
					//!< IO
				case KeyType.CopyFile				: obj = new xCopyFile				(); break;
					//!< Keystore
				case KeyType.Keystore				: obj = new xKeystore				(); break;
				}
				if( null == obj )
				{
					Log.Error( "Setter create failed! : {0}, {1}", key, val );
					return null;
				}

				val = val.Trim( ' ', '"', '\'', '\t' );
				if( true == string.IsNullOrEmpty( val ) )
					return null;

				if( true != obj.Active( val ) )
				{
					Log.Warning( "Setter active failed! : {0}, {1}", key, val );
					return null;
				}
			}
			return obj;
		}

		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region //!< AutoBuilder

		class xAutoBuilder : ISetter
		{
			private BuildType value = BuildType.None;

			public bool Active( string val )
			{
				this.value = BuildType.None;
				{
					foreach( string it in val.Split( ';', ',' ) )
					{
						BuildType tmp;
						if( true != Enum.TryParse<BuildType>( it, out tmp ) )
							return false;

						this.value |= tmp;
					}
				}
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				if( false == info.buildType.HasFlag( value ) )
				{
					Log.Error( "BuildType( {0} ) is not set! : SetType( {1} )"
						, this.value
						, info.buildType
						);
					return false; //!< problem. abort next step.
				}
				if( true == info.buildType.HasFlag( BuildType.NonBuild ) )
				{
					Log.Output( "<<{0}>> AutoBuilder skip!!!", this.value );
					return true; //!< no problem. go next step.
				}

				Log.Output( "<<{0}>> AutoBuilder Begin!!!", this.value );
				{
					BuildType setType = info.buildType;
					info.buildType = this.value;

					int result = Gong.Build.AutoBuilder.BuildFromInfo( info );
					if( 0 != result )
					{
						Log.Error( "<<{0}>> AutoBuilder failed! : result code ({0})"
							, this.value
							, result
							);
						return false; //!< problem. abort next step.
					}

					setType &= ~( this.value ); //!< builded type clear.
					info.buildType = setType;
				}
				Log.Output( "<<{0}>> AutoBuilder End!!!", this.value );
				return true;
			}
		}

		#endregion
		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region //!< Common Setter(s)

		class xBuildSettingFile : ISetter
		{
			private const string chunk = "auto build setting";
			private List<ISetter> values = new List<ISetter>( (int)KeyType.Max );
			private string filePath = string.Empty;

			public bool Active( string val )
			{
				val = val.Replace( '\\', '/' );

				//!< cutting : path after the project folder. ~~~~~~~~~~~~~~~~~~~~~~~~~~~~
				string projPath = System.IO.Path.GetDirectoryName( Application.dataPath );
				string filePath = val.Replace( projPath.Replace( '\\', '/' ) + "/", "" );
				//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

				if( false == System.IO.File.Exists( filePath ) )
				{
					Log.Error( "Setting file does not exist!! : {0}", filePath );
					return false;
				}

				using( StreamReader sr = new StreamReader( filePath, Encoding.UTF8 ) )
				{
					//!< checking : file chunk. ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
					string text = sr.ReadLine();
					if( true == string.IsNullOrEmpty( text ) ||
						false == text.ToLower().Contains( chunk ) )
					{
						Log.Error( "Setting file chunk read failed!! : {0}", filePath );
						return false;
					}
					//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

					while( true != sr.EndOfStream )
					{
						string _text = sr.ReadLine();
						{
							int index = _text.IndexOf( "///" );
							if( -1 < index )
								_text = _text.Substring( 0, index );

							if( string.IsNullOrEmpty( _text ) )
								continue;
						}

						string[] texts = _text.Split( '=' );
						if( 1 < texts.Length )
						{
							ISetter obj = CreateSetter( texts[ 0 ], texts[ 1 ] );
							if( null != obj )
								this.values.Add( obj );
						}
					}
					sr.Close();
				}
				this.filePath = filePath;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				foreach( ISetter it in this.values )
				{
					if( false == it.Action( ref info ) )
					{
						Log.Error( "Abort at file: {0}", this.filePath );
						return false; //!< problem. abort next step.
					}
				}
				return true;
			}
		}
		class xBuildType : ISetter
		{
			private BuildType value = BuildType.None;

			public bool Active( string val )
			{
				this.value = BuildType.None;
				{
					foreach( string it in val.Split( ';', ',' ) )
					{
						BuildType tmp;
						if( true != Enum.TryParse<BuildType>( it, out tmp ) )
							return false;

						this.value |= tmp;
					}
				}
				if( this.value.HasFlag( BuildType.AppBuild ) &&
					this.value.HasFlag( BuildType.AssetBundle ) )
				{
					Log.Warning( "Build cannot proceed at the same time." );
					return false;
				}
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				if( info.buildType != this.value )
				{
					Log.Output( "<<set>> BuildType : [ {0} ] + [ {1} ]"
						, info.buildType
						, this.value
						);

					info.buildType |= this.value;
				}
				return true;
			}
		}
		class xBuildTargetGroup : ISetter
		{
			private BuildTargetGroup value = BuildTargetGroup.Unknown;

			public bool Active( string val )
			{
				BuildTargetGroup tmp;
				if( true != Enum.TryParse<BuildTargetGroup>( val, out tmp ) )
					return false;

				this.value = tmp;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				info.buildTargetGroup = this.value;
				{
					Log.Output( "<<set>> BuildTargetGroup : {0}", this.value );
				}
				return true;
			}
		}
		class xBuildTarget : ISetter
		{
			private BuildTarget value	= BuildTarget.NoTarget;
			private string name			= string.Empty;

			public bool Active( string val )
			{
				BuildTarget tmp;
				if( true != Enum.TryParse<BuildTarget>( val, out tmp ) )
					return false;

				this.value = tmp;

				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				info.buildTarget = this.value;
				{
					Log.Output( "<<set>> BuildTarget : {0}", this.value );
				}
				return true;
			}
		}
		class xAppID : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				PlayerSettings.SetApplicationIdentifier( BuildTargetGroup.Android, this.value );
				Log.Output( "<<set>> AppID : {0}", this.value );
				
				return true;
			}
		}

		class xProjectType : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				info.projectType = value;

				Log.Output( "<<set>> ProjectType : {0}", this.value );

				return true;
			}
		}

		#endregion
		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region //!< AppBuild Setter(s)

		class xLocationPathName : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				this.value = val; //!< Gong.Build.Path.GetFullPathFromProjectPath( val );
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				this.value = Gong.Build.Path.GetPath( this.value, info );

				info.locationPathName = this.value;
				{
					Log.Output( "<<set>> LocationPathName : {0}", this.value );
				}
				return true;
			}
		}
		class xBuildOptions : ISetter
		{
			private struct Value
			{
				public string name;
				public BuildOptions option;
				public Value( string name, BuildOptions option )
				{
					this.name = name;
					this.option = option;
				}
			}
			private List<Value> values = null;

			public bool Active( string val )
			{
				string[] arr = val.Replace( " ", "" ).Split( ',', ';' );
				{
					this.values = new List<Value>( arr.Length );
				}
				foreach( string it in arr )
				{
					BuildOptions tmp;
					if( true == Enum.TryParse<BuildOptions>( it, out tmp ) )
					{
						this.values.Add( new Value( it, tmp ) );
					}
				}
				return ( 0 < this.values.Count ) ? true : false;
			}
			public bool Action( ref BuildInfo info )
			{
				int index = 0;
				foreach( Value it in this.values )
				{
					info.buildOptions |= it.option;
					{
						Log.Output( "<<set>> BuildOptions : ({0:00}) {1}", ++index, it.name );
					}
				}
				return true;
			}
		}
		class xBuildSkipScenes : ISetter
		{
			private List<string> values = null;

			public bool Active( string val )
			{
				this.values = new List<string>( EditorBuildSettings.scenes.Length );
				{
					string[] arr = val.Replace( " ", "" ).Split( ";,"[ 0 ] );

					foreach( EditorBuildSettingsScene scene in EditorBuildSettings.scenes )
					{
						if( true != scene.enabled )
							continue;

						if( null != arr && 0 < arr.Length )
						{
							bool skip = false;
							foreach( string it in arr )
							{
								if( scene.path.Contains( it ) )
								{
									skip = true;
									break;
								}
							}
							if( true == skip )
								continue;
						}

						this.values.Add( scene.path );
					}
				}
				return ( 0 < this.values.Count ) ? true : false;
			}
			public bool Action( ref BuildInfo info )
			{
				info.buildScenes = values.ToArray();
				{
					int index = 0;
					foreach( string it in values )
					{
						Log.Output( "<<set>> BuildScenes : ({0:00}) {1}", ++index, it );
					}
				}
				return true;
			}
		}
		class xBuildVersion : ISetter
		{
			private string value = null;

			public bool Active( string val )
			{
				//if( 18 < val.Length )
				//	return false;

				//string[] arr = val.Split( '.' );
				//foreach( string it in arr )
				//{
				//	if( true == string.IsNullOrEmpty( it ) )
				//		return false;

				//	uint tmp;
				//	if( true != uint.TryParse( it, out tmp ) )
				//		return false;
				//}

				value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
			//	string version = value.ToString().Insert( 2, "." );
			//	version = version.Insert( 1, "." );
			//	string[] temp = version.Split( '.', (char)StringSplitOptions.RemoveEmptyEntries );
			////	temp[ temp.Length - 1 ] = temp[ temp.Length - 1 ].Remove( 1 );
			//	version = ( temp[ 0 ] + "." + temp[ 1 ] + "." + temp[ 2 ] );

				PlayerSettings.bundleVersion = value;
				{
					Log.Output( "<<set>> PlayerSettings.bundleVersion : {0}", value );
				}
				return true;
			}
		}
		class xBuildVersionCode : ISetter
		{
			private int value = 0;

			public bool Active( string val )
			{
				int tmp = 0;
				if( true != int.TryParse( val, out tmp ) )
					return false;

				this.value = tmp;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				switch( info.buildTargetGroup )
				{
				case BuildTargetGroup.Android:
					{
						PlayerSettings.Android.bundleVersionCode = value;

						Log.Output( "<<set>> PlayerSettings.Android.bundleVersionCode : {0}", this.value );
					}
					break;

				case BuildTargetGroup.iOS:
					{
						PlayerSettings.iOS.buildNumber = value.ToString();

						Log.Output( "<<set>> PlayerSettings.iOS.buildNumber : {0}", this.value );
					}
					break;

				default:
					{
						Log.Warning( "BundleVersionCode set failed! : {0}, {1}"
							, info.buildTargetGroup
							, this.value
							);
					}
					break;
				}
				return true;
			}
		}
		class xBuildAppBundle : ISetter
		{
			private bool value = false;

			public bool Active( string val )
			{
				bool tmp = false;
				if( true != bool.TryParse( val, out tmp ) )
					return false;

				this.value = tmp;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				if( BuildTargetGroup.Android != info.buildTargetGroup )
				{
					Log.Warning( "EditorUserBuildSettings.buildAppBundle set failed! : {0}"
						, info.buildTargetGroup
						);
					return true; //!< no problem. go next step.
				}

				EditorUserBuildSettings.buildAppBundle = this.value;
				if( EditorUserBuildSettings.buildAppBundle &&
					EditorUserBuildSettings.androidBuildSystem != AndroidBuildSystem.Gradle )
				{
					EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
				}

				Log.Output( "<<set>> EditorUserBuildSettings.buildAppBundle : {1}"
					, info.buildTarget
					, this.value
					);
				return true;
			}
		}
		class xBuildScriptSymbols : ISetter
		{
			private string value = null;

			public bool Active( string val )
			{
				value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				if( BuildTargetGroup.Unknown == info.buildTargetGroup )
				{
					Log.Error( "PlayerSettings.ScriptingDefineSymbols set failed! : {0}"
						, info.buildTargetGroup
						);
					return false; //!< problem. abort next step.
				}

				PlayerSettings.SetScriptingDefineSymbols( UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup( info.buildTargetGroup ), value );
				{
					Log.Output( "<<set>> PlayerSettings.SetScriptingDefineSymbols : {0} << {1}"
						, info.buildTargetGroup
						, value
						);
				}
				return true;
			}
		}

		#endregion
		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region //!< AssetBundle Setup Setter(s)

		class xBundleRemoveAll : ISetter
		{
			private bool value = false;

			public bool Active( string val )
			{
				bool tmp = false;
				if( true != bool.TryParse( val, out tmp ) )
					return false;

				this.value = tmp;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				Log.Output( "<<set>> BundleRemoveAll : {0}", this.value );

				if( this.value )
				{
					Gong.Build.Utility.RemoveAllAssetBundle();
				}
				return true;
			}
		}
		class xBundleSetupExclude : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				this.value = val.TrimStart( '.' );
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				info.abSetupExclude = this.value;
				{
					Log.Output( "<<set>> BundleSetupExclude : {0}", this.value );
				}
				return true;
			}
		}
		class xBundleSetupFolder : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				this.value = val.Replace( '\\', '/' );
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				if( false == System.IO.Directory.Exists( this.value ) )
				{
					Log.Warning( "Folder is not exists! : {0}", this.value );
					return true; //!< no problem. go next step.
				}

				if( info.buildType.HasFlag( BuildType.AppBuild ) )
				{
					AssetDatabase.DeleteAsset( this.value );
					AssetDatabase.Refresh();
					{
						Log.Output( "<<set>> AssetDatabase.DeleteAsset : {0}", this.value );
					}
				}
				else if( info.buildType.HasFlag( BuildType.AssetBundle ) )
				{
					Gong.Build.Utility.SetupAssetBundleToFolders( value, info.abSetupExclude );
				}
				return true;
			}
		}
		class xBundleSteupScenes : ISetter
		{
			private List<string> values = null;

			public bool Active( string val )
			{
				if( false == System.IO.Directory.Exists( val ) )
					return false;

				string[] scenes = System.IO.Directory.GetFiles( val, "*.unity", SearchOption.AllDirectories );
				if( scenes.Length <= 0 )
					return false;

				this.values = new List<string>( scenes.Length );
				{
					foreach( string it in scenes )
						this.values.Add( it.Replace( '\\', '/' ) );
				}
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				if( info.buildType.HasFlag( BuildType.AppBuild ) )
				{
					List<string> buildScenes = new List<string>();
					{
						int index = 0;
						foreach( EditorBuildSettingsScene it in EditorBuildSettings.scenes )
						{
							if( true == it.enabled &&
								true != this.values.Contains( it.path ) )
							{
								if( 0 < it.path.Length )
								{
									buildScenes.Add( it.path );
									{
										Log.Output( "<<set>> BuildScenes : ({0:00}) {1}", ++index, it.path );
									}
								}
							}
						}
					}
					info.buildScenes = buildScenes.ToArray();
				}
				else if( info.buildType.HasFlag( BuildType.AssetBundle ) )
				{
					Gong.Build.Utility.SetupAssetBundleToScenes( values.ToArray() );
				}
				return true;
			}
		}

		#endregion
		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region //!< AssetBundle Build Setter(s)

		class xBundleOutputPath : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				if( string.IsNullOrEmpty( val ) )
					return false;

				this.value = val; //!< Gong.Build.Path.GetFullPathFromProjectPath( val );
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				this.value = Gong.Build.Path.GetPath( this.value, info );

				info.abBuildOutputPath = this.value;
				{
					Log.Output( "<<set>> Bunlde OutputPath : {0}", this.value );
				}
				return true;
			}
		}
		class xBundleOptions : ISetter
		{
			private BuildAssetBundleOptions value = BuildAssetBundleOptions.None;

			public bool Active( string val )
			{
				string[] arr = val.Replace( " ", "" ).Split( ',', ';' );
				foreach( string it in arr )
				{
					BuildAssetBundleOptions tmp;
					if( true == Enum.TryParse<BuildAssetBundleOptions>( it, out tmp ) )
					{
						this.value |= tmp;
					}
				}
				return ( BuildAssetBundleOptions.None != this.value ) ? true : false;
			}
			public bool Action( ref BuildInfo info )
			{
				info.abBuildOptions = this.value;
				{
					Log.Output( "<<set>> BuildAssetBundleOptions : {0}", this.value );
				}
				return true;
			}
		}

		#endregion
		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region //!< AssetBundle Patch Setter(s)

		class xPatchOutputPath : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				if( string.IsNullOrEmpty( val ) )
					return false;

				this.value = val; //!<Gong.Build.Path.GetFullPathFromProjectPath( val );
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				this.value = Gong.Build.Path.GetPath( this.value, info );

				info.abPatchOutputPath = this.value;
				{
					Log.Output( "<<set>> Patch OutputPath : {0}", this.value );
				}
				return true;
			}
		}
		class xPatchListFile : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				this.value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				this.value = Gong.Build.Path.GetSubPath
					( this.value
					, info.abPatchOutputPath
					, info
					);

				info.abPatchListFile = this.value;
				{
					Log.Output( "<<set>> PatchListFile : {0}", this.value );
				}
				return true;
			}
		}
		class xPatchRevision : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				if( string.IsNullOrEmpty( val ) )
					return false;

				this.value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				this.value = Gong.Build.Path.GetSubPath
					( this.value
					, info.abPatchOutputPath
					, info
					);

				info.abPatchRevision = this.value;
				{
					Log.Output( "<<set>> Patch Revision : {0}", this.value );
				}
				return true;
			}
		}
		class xPatchRootURL : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				this.value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
#if _BUILD_AM_
				string buildType = "AM";
#elif _BUILD_DEV_
				string buildType = "DEV";
#elif _BUILD_QA_
				string buildType = "QA";
#elif _BUILD_IOS_
				string buildType = "iOS";
#else
				string buildType = "LIVE";
#endif
				//info.abPatchRootURL = string.Format( "{0}/{1}/{2}{3}", this.value, buildType,"v", AppConfig.appConfigData.bundleVersion );

				info.abPatchRootURL = this.value;
				{
					Log.Output( "<<set>> Patch Root URL : {0}", info.abPatchRootURL );
				}
				return true;
			}
		}
		class xPatchSubURL : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				this.value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				//!< TODO. HC에서는 SubURL로만 사용되어서 수정. 추후 MLB와 맞춘다고하면 풀어야함
				//string output = Gong.Build.Path.GetPathFromBehind
				//	( this.value
				//	, string.Format( "{0}/{1}", info.abPatchOutputPath, info.abPatchRevision )
				//	);

				info.abPatchSubURL = value;
				{
					Log.Output( "<<set>> Patch Sub URL : {0} ", this.value );
				}
				return true;
			}
		}
		class xPatchFileExtension : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				System.Func<string, bool> check = ( ext ) =>
				{
					foreach( char ch in ext )
					{
						if( 0x61 <= ch && ch <= 0x7A ) continue;
						if( 0x41 <= ch && ch <= 0x5A ) continue;
						if( 0x30 <= ch && ch <= 0x39 ) continue;
						return false;
					}
					return true;
				};

				if( false == check( val ) )
					return false;

				this.value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				info.abPatchFileExtension = this.value;
				{
					Log.Output( "<<set>> PatchFileExtension : {0}", this.value );
				}
				return true;
			}
		}

		#endregion
		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region //!< Upload Setter(s)

		class xUploadDomain : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				if( string.IsNullOrEmpty( val ) )
					return false;

				this.value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				info.ftpDomain = this.value;
				{
					Log.Output( "<<set>> Upload FTP Domain : {0}", this.value );
				}
				return true;
			}
		}
		class xUploadUserName : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				if( string.IsNullOrEmpty( val ) )
					return false;

				this.value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				info.ftpUserName = this.value;
				{
					Log.Output( "<<set>> Upload FTP UserName : {0}", this.value );
				}
				return true;
			}
		}
		class xUploadPassword : ISetter
		{
			private string value = string.Empty;

			public bool Active( string val )
			{
				if( string.IsNullOrEmpty( val ) )
					return false;

				this.value = val;
				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				info.ftpPassword = this.value;
				{
					Log.Output( "<<set>> Upload FTP Password : {0}", this.value );
				}
				return true;
			}
		}
		class xUploadPath : ISetter
		{
			private struct UploadInfo
			{
				public string src;
				public string url;
			}
			private UploadInfo value;

			public bool Active( string val )
			{
				try
				{
					this.value = JsonUtility.FromJson<UploadInfo>( val );
					{
						this.value.src = this.value.src.Trim( ' ', '\t', '/', '\\' );
						this.value.url = this.value.url.Trim( ' ', '\t', '/', '\\' );
					}
				}
				catch( Exception e )
				{
					Log.Warning( "{0} Exception caught.", e );
				}

				if( string.IsNullOrEmpty( this.value.src ) )
					return false;
				if( string.IsNullOrEmpty( this.value.url ) )
					return false;

				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				string src = this.value.src.Replace( '\\', '/' );
				string url = this.value.url;
				{
					src = Gong.Build.Path.GetPath( src, info );
					url = Gong.Build.Path.GetPathFromBehind( url, src );
					url = Gong.Build.Path.GetPath( url, info );
				}

				if( false == info.buildType.HasFlag( BuildType.Upload ) )
				{
					Log.Warning( "<<set>> Upload skip!!! : {0}", src );
					return true; //!< no problem. go next step.
				}

				if( url.Contains( info.ftpDomain ) )
					url = url.Replace( info.ftpDomain, "" ).Trim( '/' );

				bool result = Gong.Build.Utility.UploadPathToFTP
					( src
					, url
					, info.ftpDomain
					, info.ftpUserName
					, info.ftpPassword
					, true
					);

				if( result )
				{
					Log.Output( "<<set>> Upload success! : {0} >> {1}", src, url );
					return true;
				}
				else
				{
					Log.Error( "Upload failed! : {0} >> {1}", src, url );
					return false; //!< abort next step.
				}
			}
		}

		#endregion
		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region //!< IO Setter(s)

		class xCopyFile : ISetter
		{
			private struct CopyInfo
			{
				public string src;
				public string dest;
			}
			private CopyInfo value;

			public bool Active( string val )
			{
				try
				{
					this.value = JsonUtility.FromJson<CopyInfo>( val );
					{
						this.value.src = this.value.src.Trim( ' ', '\t' );
						this.value.dest = this.value.dest.Trim( ' ', '\t' );
					}
				}
				catch( Exception e )
				{
					Log.Warning( "Exception caught.", e );
				}

				if( string.IsNullOrEmpty( this.value.src ) )
					return false;
				if( string.IsNullOrEmpty( this.value.dest ) )
					return false;

				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				string src = Gong.Build.Path.GetPath( this.value.src, info );
				string dest = Gong.Build.Path.GetPathFromBehind( this.value.dest, src );
				dest = Gong.Build.Path.GetPath( dest, info );

				if( true == info.buildType.HasFlag( BuildType.NonCopy ) )
				{
					Log.Warning( "<<set>> File copy skip!!! : {0} >> {1}", src, dest );
					return true; //!< no problem. go next step.
				}

				if( false == System.IO.File.Exists( src ) )
				{
					Log.Error( "File is not exists! : {0}", src );
					return false; //!< abort next step.
				}
				string dir = System.IO.Path.GetDirectoryName( dest );
				if( false == System.IO.Directory.Exists( dir ) )
				{
					System.IO.Directory.CreateDirectory( dir );
				}

				System.IO.File.Copy( src, dest, true );
				{
					Log.Output( "<<set>> Copy File : {0} >> {1}", src, dest );
				}
				return true;
			}
		}

		#endregion
		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region //!< Keystore

		class xKeystore : ISetter
		{
			private struct KeystoreInfo
			{
				public string keystoreName;
				public string keystorePass;
				public string keyaliasName;
				public string keyaliasPass;
			}
			private KeystoreInfo jsonData;

			public bool Active( string val )
			{
				try
				{
					this.jsonData = JsonUtility.FromJson<KeystoreInfo>( val );
					{
						this.jsonData.keystoreName = this.jsonData.keystoreName.Trim( ' ', '\t' );
						this.jsonData.keystorePass = this.jsonData.keystorePass.Trim( ' ', '\t' );
						this.jsonData.keyaliasName = this.jsonData.keyaliasName.Trim( ' ', '\t' );
						this.jsonData.keyaliasPass = this.jsonData.keyaliasPass.Trim( ' ', '\t' );
					}
				}
				catch( Exception e )
				{
					Log.Warning( "Exception caught.", e );
				}

				if( string.IsNullOrEmpty( this.jsonData.keystoreName ) )
					return false;
				if( string.IsNullOrEmpty( this.jsonData.keystorePass ) )
					return false;
				if( string.IsNullOrEmpty( this.jsonData.keyaliasName ) )
					return false;
				if( string.IsNullOrEmpty( this.jsonData.keyaliasPass ) )
					return false;

				return true;
			}
			public bool Action( ref BuildInfo info )
			{
				if( BuildTarget.Android == info.buildTarget )
				{
					PlayerSettings.Android.keystoreName = this.jsonData.keystoreName;
					PlayerSettings.Android.keystorePass = this.jsonData.keystorePass;
					PlayerSettings.Android.keyaliasName = this.jsonData.keyaliasName;
					PlayerSettings.Android.keyaliasPass = this.jsonData.keyaliasPass;

					Log.Output( "<<set>> Keystore Info : {0}/{1}", PlayerSettings.Android.keystoreName, PlayerSettings.Android.keystorePass );
					Log.Output( "<<set>> Keyalias Info : {0}/{1}", PlayerSettings.Android.keyaliasName, PlayerSettings.Android.keyaliasPass );
				}
				else
				{
					Log.Warning( "This buildTarget is Android!!" );
				}
				return true;
			}
		}

		#endregion
	}
}
