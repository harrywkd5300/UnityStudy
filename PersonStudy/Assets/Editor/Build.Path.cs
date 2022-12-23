namespace Gong.Build
{
	using UnityEngine;
	using UnityEditor;

	static public partial class Path
	{
		static public string GetFullProjectPath()
		{
			string fullPath = System.IO.Path.GetDirectoryName( Application.dataPath );
			return fullPath.Replace( '\\', '/' );
		}
		static public string GetFullPathFromProjectPath( string pjSubPath )
		{
			string subPath = pjSubPath.Trim( '.', '\\', '/' );
			string fullPath = System.IO.Path.Combine( GetFullProjectPath(), subPath );
			return fullPath.Replace( '\\', '/' );
		}
	}

	static public partial class Path
	{
		const char StartKey 			= '[';
		const char FinalKey				= ']';

		const string Target				= "_TARGET_";
		const string TargetGroup		= "_TAR_GROUP_";
		const string Version			= "_VERSION_";
		const string VerDotX			= "_VER_DOT_X_";
		const string VerCode			= "_VER_CODE_";
		const string NowDate			= "_NOW_";
		const string NextRevision		= "_NEXT_REV_";
		const string LastRevision		= "_LAST_REV_";

		static public string GetPath( string fmtPath, Gong.Build.BuildInfo info )
		{
			System.Func<string, string, string> convert = ( _front, _fmt ) =>
			{
				int _index = _fmt.IndexOf( FinalKey );
				if( _index < 0 )
				{
					Log.Warning( "Split-Path FinalKey('{0}') is not exists! : {1}", FinalKey, _fmt );
					return _front + _fmt;
				}

				string _orgTemp = _fmt.Replace( FinalKey, '}' );
				//!< default
				{
					string _chgTemp = _orgTemp;
					{
						_chgTemp = _chgTemp.Replace( Target, "{0" );
						_chgTemp = _chgTemp.Replace( TargetGroup, "{1" );
						_chgTemp = _chgTemp.Replace( Version, "{2" );
						_chgTemp = _chgTemp.Replace( VerDotX, "{3" );
						_chgTemp = _chgTemp.Replace( VerCode, "{4" );
						_chgTemp = _chgTemp.Replace( NowDate, "{5" );
					}
					if( false == _chgTemp.Equals( _orgTemp ) )
					{
						string _out = string.Format( _chgTemp
							, info.buildTarget
							, info.buildTargetGroup
							, getVersion()
							, getVersion().Replace( ".", "" )
							, getVersionCode( info.buildTargetGroup )
							, System.DateTime.Now
						);

						Log.Debug( "Split-Path default converting! : '{0}' >> '{1}'", _fmt, _out );
						return _front + _out;
					}
				}
				//!< revision
				{
					string _chgTemp = _orgTemp;
					{
						_chgTemp = _chgTemp.Replace( NextRevision, "{0" );
						_chgTemp = _chgTemp.Replace( LastRevision, "{1" );
					}
					if( false == _chgTemp.Equals( _orgTemp ) )
					{
						long _rev = getRevision( _front );

						string _out = string.Format( _chgTemp
							, _rev + 1
							, _rev
							);

						Log.Debug( "Split-Path revision converting! : '{0}' >> '{1}'", _fmt, _out );
						return _front + _out;
					}
				}
				//!< non-chunk
				{
					Log.Warning( "Split-Path Chunk is not exists! : '{0}'", _fmt.Substring( 0, _index ) );
					return _front + _fmt;
				}
			};

			//!< path converting
			string _tmpPath = fmtPath.Replace( '\\', '/' ).Trim( '/', '.' );
			{
				string[] _texts = _tmpPath.Split( StartKey );
				if( 1 < _texts.Length )
				{
					_tmpPath = _texts[ 0 ];

					for( int i = 1; i < _texts.Length; ++i )
					{
						_tmpPath = convert( _tmpPath, _texts[i] );
					}

					Log.Debug( "Path : '{0}' >> '{1}'", fmtPath, _tmpPath );
				}
			}
			return _tmpPath;
		}
		static public string GetSubPath( string fmtSubPath, string rootPath, Gong.Build.BuildInfo info )
		{
			string _rootDir = rootPath.Trim( '\\', '/', '.' ) + "/";
			string _fmtSubPath = fmtSubPath.Trim( '\\', '/', '.' );

			string _outPath = GetPath( _rootDir + _fmtSubPath, info );
			string _subPath = _outPath.Replace( _rootDir, "" );
			{
				Log.Debug( "Sub Path : '{0}' >> '{1}'", _fmtSubPath, _subPath );
			}
			return _subPath;
		}

		static public string GetPathFromBehind( string fmtPath, string orgPath )
		{
			const char startKey = '{';
			const char finalKey = '}';

			string[] paths = orgPath.Split( '\\', '/' );

			System.Func<string, string, string> convert = ( _front, _fmt ) =>
			{
				int _index = _fmt.IndexOf( finalKey );
				if( _index < 0 )
				{
					Log.Warning( "Split-Path FinalKey('{0}') is not exists! : {1}", finalKey, _fmt );
					return _front + _fmt;
				}

				string _srcPath = string.Empty;
				{
					int _idxValue = 0;
					string _idxText = _fmt.Substring( 0, _index );
					if( false == int.TryParse( _idxText, out _idxValue ) )
					{
						Log.Warning( "Split-Path Index parsing failed! : {0}", _idxText );
						return _front + _fmt;
					}
					if( 0 <= _idxValue && _idxValue < paths.Length )
					{
						_srcPath = paths[ paths.Length - 1 - _idxValue ];
					}
					if( string.IsNullOrEmpty( _srcPath ) )
					{
						Log.Warning( "" );
						return _front + _fmt;
					}
				}
				string _out = string.Format( "{0" + _fmt.Substring( _index ), _srcPath );
				{
					Log.Debug( "Split-Path converting! : '{0}' >> '{1}'", _fmt, _out );
				}
				return _front + _out;
			};

			//!< path converting
			string _tmpPath = fmtPath.Replace( '\\', '/' ).Trim( '/', '.' );
			{
				string[] _texts = _tmpPath.Split( startKey );
				if( 1 < _texts.Length )
				{
					_tmpPath = _texts[ 0 ];

					for( int i = 1; i < _texts.Length; ++i )
					{
						_tmpPath = convert( _tmpPath, _texts[i] );
					}

					Log.Debug( "Path : '{0}' >> '{1}' from {2}", fmtPath, _tmpPath, orgPath );
				}
			}
			return _tmpPath;
		}

		//!< get from ProjectSetting
		static private string getVersion()
		{
			return PlayerSettings.bundleVersion;
		}
		static private string getVersionCode( BuildTargetGroup targetGroup )
		{
			string verCode = string.Empty;
			{
				switch( targetGroup )
				{
				case BuildTargetGroup.Android:
					verCode = PlayerSettings.Android.bundleVersionCode.ToString();
					break;

				case BuildTargetGroup.iOS:
					verCode = PlayerSettings.iOS.buildNumber;
					break;

				default:
					verCode = "_x_";
					break;
				}
			}
			return verCode;
		}

		//!< get from disk
		static private long getRevision( string path )
		{
			long revision = 0;
			{
				string _tag = System.IO.Path.GetFileName( path );
				string _dir = System.IO.Path.GetDirectoryName( path );

				if( System.IO.Directory.Exists( _dir ) )
				{
					string[] _dirs = System.IO.Directory.GetDirectories( _dir );
					foreach( string it in _dirs )
					{
						string rev = System.IO.Path.GetFileName( it );
						if( false == string.IsNullOrEmpty( _tag ) )
							rev = rev.Replace( _tag, "" );

						int tmp;
						if( int.TryParse( rev, out tmp ) )
						{
							if( revision < tmp )
								revision = tmp;
						}
					}
				}
			}
			return revision;
		}
	}
}
