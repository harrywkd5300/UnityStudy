namespace Utility
{
	using System.Collections.Generic;

	static public class Common
	{
		static public string GetString( int stringID )
		{
			return StringTBL.GetString( stringID );
		}
		static public string GetString( int stringID, params object[] args )
		{
			return string.Format( GetString( stringID ), args );
		}

		/// <summary>
		/// List에서 match 조건에 해당 되는 원소를 out 인자로 반환해준다.
		/// </summary>
		static public bool TryGetGeneric<T>( this List<T> generic, System.Predicate<T> match, out T output ) where T : class
		{
			output = null;
			output = generic.Find( ( it ) => { return match?.Invoke( it ) ?? false; } );

			return ( null != output );
		}
		
		/// <summary>
		/// 스트링을 파싱해주는 함수들
		/// </summary>
		static public bool TryParse( string s, out string result )
		{
			result = s;
			return true;
		}
		static public bool TryParse<TEnum>( string s, out TEnum result ) where TEnum : struct
		{
			if( true == System.Enum.TryParse( s, out result ) )
				return true;

			Log.Error( "[Parse]", "System.Enum.TryParse()" );

			return false;
		}
		static public bool TryParse( string s, out int result )
		{
			if( true == int.TryParse( s, out result ) )
				return true;

			Log.Error( "[Parse]", "int.TryParse()" );

			return false;
		}
		static public bool TryParse( string s, out float result )
		{
			if( true == float.TryParse( s, out result ) )
				return true;

			Log.Error( "[Parse]", "float.TryParse()" );

			return false;
		}
		static public bool TryParse( string s, out long result )
		{
			if( true == long.TryParse( s, out result ) )
				return true;

			Log.Error( "[Parse]", "long.TryParse()" );

			return false;
		}
		static public bool TryParse( string s, out System.DateTime result )
		{
			if( true == System.DateTime.TryParse( s, out result ) )
				return true;

			Log.Error( "[Parse]", "Time.TryParse()" );

			return false;
		}
		static public bool TryParse( string s, out bool result )
		{
			int isUse = 0;
			result = false;

			if( true == int.TryParse( s, out isUse ) )
			{
				result = ( 0 < isUse );
				return true;
			}
			else if( true == bool.TryParse( s, out result ) )
			{
				return true;
			}

			Log.Error( "[Parse]", "bool.TryParse()" );

			return false;
		}	
	}
}