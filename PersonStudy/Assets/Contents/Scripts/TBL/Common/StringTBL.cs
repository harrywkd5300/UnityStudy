using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringData
{
	public readonly int		tID			= 0;
	public readonly string	szString	= "";

	public StringData( string[] args )
	{
		int index = 0;

		Utility.Common.TryParse( args[ index++ ], out tID );
		Utility.Common.TryParse( args[ index++ ], out szString );
	}
}

public class StringTBL : Singleton<StringTBL>, ITable
{
	private List<StringData>	lst = null;

	protected StringTBL() { lst = new List<StringData>(); }

	public override string ToString()
	{
		return string.Format( $"{"[StringTBL] : "}({lst.Count})" );
	}
	public bool Initialize( bool isBinary )
	{
		return LoadData( "TBL/Common", "String" ); ;
	}
	public void Destroy()
	{
		lst.Clear();
	}

	private bool LoadData( string path, string file )
	{
		using( CSVReader tr = new CSVReader() )
		{
			if( true != tr.Open( path, file ) )
				return false;

			string[] texts;
			while( tr.ReadLine( out texts ) )
			{
				lst.Add( new StringData( texts ) );
			}

			tr.Close();
		}
		return true;
	}

	static public string GetString( int id )
	{
		StringData data = Inst.lst.Find( a => a.tID == id );

		return data == null ? "" : data.szString;
	}
}
