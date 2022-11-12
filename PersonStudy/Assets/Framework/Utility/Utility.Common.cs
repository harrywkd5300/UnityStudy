namespace Utility
{
	using System.Collections.Generic;

	static public class Common
	{
		/// <summary>
		/// List���� match ���ǿ� �ش� �Ǵ� ���Ҹ� out ���ڷ� ��ȯ���ش�.
		/// </summary>
		static public bool TryGetGeneric<T>( this List<T> generic, System.Predicate<T> match, out T output ) where T : class
		{
			output = null;
			output = generic.Find( ( it ) => { return match?.Invoke( it ) ?? false; } );

			return ( null != output );
		}
		
		/// <summary>
		/// ��Ʈ���� �Ľ����ִ� �Լ���
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

			Log.Error( "System.Enum.TryParse()" );

			return false;
		}
		static public bool TryParse( string s, out int result )
		{
			if( true == int.TryParse( s, out result ) )
				return true;

			Log.Error( "int.TryParse()" );

			return false;
		}
		static public bool TryParse( string s, out float result )
		{
			if( true == float.TryParse( s, out result ) )
				return true;

			Log.Error( "float.TryParse()" );

			return false;
		}
		static public bool TryParse( string s, out long result )
		{
			if( true == long.TryParse( s, out result ) )
				return true;

			Log.Error( "long.TryParse()" );

			return false;
		}
		static public bool TryParse( string s, out System.DateTime result )
		{
			if( true == System.DateTime.TryParse( s, out result ) )
				return true;

			Log.Error( "Time.TryParse()" );

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

			Log.Error( "bool.TryParse()" );

			return false;
		}	
	}
}