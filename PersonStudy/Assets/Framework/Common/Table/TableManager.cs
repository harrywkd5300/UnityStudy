using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : Singleton<TableManager>, ITable
{
	private List<ITable>	lstTable	= new List<ITable>();
	public bool				isValid		{ get { return ( 0 < lstTable.Count ) ? true : false; } }

	const string			logChunk	= "<color=white>[TBL]</color>";

	protected TableManager() { }

	public bool Initialize( bool isBinary )
	{
		foreach( ITable it in lstTable )
		{
			if( null != it )
			{
				try
				{
					if( it.Initialize( isBinary ) )
					{
						Utility.Log.Output( logChunk, "Table initialize success! : {0}", it.ToString() );
					}
					else
					{
						Utility.Log.Error( logChunk, "Table initialize failed! : {0}", it.ToString() );
					}
				}
				catch( System.Exception e )
				{
					Utility.Log.Error( logChunk, "Table read failed!! : {0}\n{1}", it.ToString(), e.StackTrace );
				}
			}
		}
		return true;
	}
	public void Destroy()
	{
		foreach( ITable it in lstTable )
		{
			if( null != it )
			{
				it.Destroy();
			}
		}
		lstTable.Clear();
	}
	public override string ToString()
	{
		return string.Format( $"TableManager({lstTable.Count})" );
	}

	public bool Register( ITable tbl )
	{
		if( null == tbl )
			return false;

		foreach( ITable it in lstTable )
		{
			if( it == tbl )
			{
				Utility.Log.Error( logChunk, "Table is exists! : {0}", it.ToString() );
				return false;
			}
		}

		lstTable.Add( tbl );
		return true;
	}
}
