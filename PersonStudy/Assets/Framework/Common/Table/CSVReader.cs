using System.IO;
using System.Text;
using UnityEngine;
using AssetBundle;

public class CSVReader : System.IDisposable
{
	private MemoryStream ms = null;
	private StreamReader sr = null;

	public bool Open( string path, string file )
	{
		var ta = AssetManager.Inst.LoadResources<TextAsset>( path, file );
		if( ta == null )
		{
			Utility.Log.Error( "[CSVReader]", $"Table open falied! >> file : {path}/{file}" );
			return false;
		}

		this.ms = new MemoryStream( ta.bytes );
		this.sr = new StreamReader( ms, Encoding.UTF8 );

		return true;
	}

	public void Close()
	{
		if( sr != null )
		{
			sr.Dispose();
			sr.Close();
			sr = null;
		}
		if( ms != null )
		{
			ms.Dispose();
			ms.Close();
			ms = null;
		}
	}
	public void Dispose()
	{
		Close();
	}

	public bool ReadLine( out string[] aryText )
	{
		aryText = null;

		if( null == sr )
			return false;

		if( true == sr.EndOfStream )
			return false;

		string strLine = sr.ReadLine();
		if( true == string.IsNullOrEmpty( strLine ) )
			return false;

		aryText = strLine.Split( ","[ 0 ] );
		if( 0 >= aryText.Length )
			return false;

		return true;
	}
}
