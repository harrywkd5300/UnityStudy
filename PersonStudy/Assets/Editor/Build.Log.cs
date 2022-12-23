namespace Gong.Build
{
	using System.Diagnostics;

	static public class Log
	{
		const string logChunk = "[BUILD]";

		static public void Output( string format, params object[] args )
		{
			UnityEngine.Debug.LogFormat( "{0:f5}\t{1} {2}"
				, UnityEngine.Time.realtimeSinceStartup
				, logChunk
				, string.Format( format, args )
				);
		}
		static public void Warning( string format, params object[] args )
		{
			UnityEngine.Debug.LogWarningFormat( "{0:f5}\t{1} {2}"
				, UnityEngine.Time.realtimeSinceStartup
				, logChunk
				, string.Format( format, args )
				);
		}
		static public void Error( string format, params object[] args )
		{
			UnityEngine.Debug.LogErrorFormat( "{0:f5}\t{1} {2}"
				, UnityEngine.Time.realtimeSinceStartup
				, logChunk
				, string.Format( format, args )
				);
		}

		[Conditional( "_DEBUG_LOG_" )]
		static public void Debug( string format, params object[] args )
		{
			UnityEngine.Debug.LogWarningFormat( "{0:f5}\t{1} <<debug>> {2}"
				, UnityEngine.Time.realtimeSinceStartup
				, logChunk
				, string.Format( format, args )
				);
		}
	}
}
