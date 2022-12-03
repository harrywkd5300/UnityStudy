using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
	using System.Diagnostics;

	static public class Log
	{
		[Conditional( "LOG" ), Conditional( "UNITY_EDITOR" )]
		static public void Output( string tag, string format, params object[] args )
		{
			UnityEngine.Debug.LogFormat( "{0:f5}\t{1} {2}"
				, UnityEngine.Time.realtimeSinceStartup
				, tag
				, string.Format( format, args )
				);
		}
		static public void Warning( string tag, string format, params object[] args )
		{
			UnityEngine.Debug.LogWarningFormat( "{0:f5}\t{1} {2}"
				, UnityEngine.Time.realtimeSinceStartup
				, tag
				, string.Format( format, args )
				);
		}
		static public void Error( string tag, string format, params object[] args )
		{
			UnityEngine.Debug.LogErrorFormat( "{0:f5}\t{1} {2} {2}"
				, UnityEngine.Time.realtimeSinceStartup
				, tag
				, string.Format( format, args )
				);
		}
		static public void Break( string tag, string format, params object[] args )
		{
			UnityEngine.Debug.LogErrorFormat( "{0:f5}\t{1} {2}"
				, UnityEngine.Time.realtimeSinceStartup
				, tag
				, string.Format( format, args )
				);

			UnityEngine.Debug.DebugBreak();
		}

		static public void CrashProcess( System.Exception e )
		{
			if( null == e )
				return;

			string error = ErrorLogBuild( e );

			Error( "[Crash]", error );
		}
		static private string ErrorLogBuild( System.Exception e )
		{
			string log = string.Format( "[{0}]\n", e.StackTrace );

			string[] lines = e.StackTrace.Split( '\r' );

			int lineCount = 0;
			int maxStackCount = 6;

			foreach( string line in lines )
			{
				if( line.Length <= 0 )
					continue;

				string		methodName	= line.Substring( 0, line.IndexOf( " (" ) );
				string[]	split		= line.Split( '\\' );
				string		scriptPos	= string.Format( "( {0})", split[ split.Length - 1 ] );

				log += methodName + scriptPos;

				lineCount++;

				if( maxStackCount <= lineCount )
					break;
			}

			return log;
		}
	}
}