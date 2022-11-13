using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : Singleton<AssetManager>
{
	protected AssetManager()
	{

	}

	public GameObject MakeOBJ( string path )
	{
		GameObject go = LoadResources<GameObject>( path );
		if( null == go )
			return null;

		GameObject it = Object.Instantiate( go ) as GameObject;

		return it;
	}

	public RESC LoadResources<RESC>( string path ) where RESC : UnityEngine.Object
	{
		return UnityEngine.Resources.Load<RESC>( path );
	}
}
