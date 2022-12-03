using LitJson;

public class JsonReader
{
	static public T Read<T>( string json )
	{
		try
		{
			T jsonClass = JsonMapper.ToObject<T>( json );
			if( jsonClass == null )
			{
				throw new System.Exception( "[ JSON LOADER ] jsonClass is Null!" );
			}
			return jsonClass;
		}
		catch( System.Exception e )
		{
			throw e;
		}
	}
}
