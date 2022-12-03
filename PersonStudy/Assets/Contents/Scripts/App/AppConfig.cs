using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConfig
{
	public bool isValid		= false;

	public Config config	= null;

	public void Initialize()
	{
		if( !isValid )
		{
			try
			{
				if( null == config )	config = new Config();
			}
			catch( System.Exception e )
			{
				Utility.Log.CrashProcess( e );
			}

			isValid = true;
		}
	}
	public void Destroy()
	{
		isValid = false;
	}
}
