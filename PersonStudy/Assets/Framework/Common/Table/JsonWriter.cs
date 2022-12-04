using System.IO;
using Newtonsoft.Json;

public class JsonWriter
{
	static public bool Write<T>( T target, string path, string file )
	{
		if( null != target )
		{
			try
			{
				string convertToJson = JsonConvert.SerializeObject( target, Formatting.Indented );

				DirectoryInfo dir = new DirectoryInfo( path );
				if( null != dir )
					dir.Create();

				StreamWriter wr = new StreamWriter( string.Format( $"{path}/{file}" ), false );

				wr.WriteLine( convertToJson );
				wr.Close();
			}
			catch
			{
				return false;
			}
		}

		return true;
	}
}
