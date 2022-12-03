using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundle
{
	public class AssetManager : Singleton<AssetManager>
	{
		protected AssetManager()
		{

		}

		public GameObject MakeOBJ( GameObject go )
		{
			GameObject it = Object.Instantiate( go ) as GameObject;

			return it;
		}
		public GameObject MakeOBJ( string path, string file )
		{
			GameObject go = LoadResources<GameObject>( path, file );
			if( null == go )
				return null;

			return MakeOBJ( go );
		}

		public RESC LoadResources<RESC>( string path, string file ) where RESC : UnityEngine.Object
		{
			return LoadResources<RESC>( string.Format( $"{path}/{file}" ) );
		}
		public RESC LoadResources<RESC>( string path ) where RESC : UnityEngine.Object
		{
			return UnityEngine.Resources.Load<RESC>( path );
		}
	}
}
