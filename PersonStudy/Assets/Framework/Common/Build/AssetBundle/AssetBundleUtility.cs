
namespace AssetBundle
{
	using System;
	using System.IO;
	using System.Collections.Generic;

	static public class Utility
	{
		static public string RESULT_ROOT_DIR	= "PatchResources/assetbundles";

		static public string BASE_ROOT_DIR		= "./Assets/Res_Patch/Resources";

		static public string CreateDirectory( string path )
		{
			try
			{
				if( !Directory.Exists( path ) )
				{
					Directory.CreateDirectory( path );
				}

				return path;
			}
			catch( Exception e )
			{
				throw e;
			}
		}
	}
}