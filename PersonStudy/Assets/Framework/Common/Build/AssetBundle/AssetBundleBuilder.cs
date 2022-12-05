
namespace AssetBundle
{
	using System.IO;
	using System.Collections.Generic;

	public class AssetBundleData
	{
		public int		tID		{ private set; get; }
		public string	path	{ private set; get; }
		public string	name	{ private set; get; }
		public bool		isFile	{ private set; get; }
		public bool		isVaild { private set; get; }

		public AssetBundleData( int _TID, string _path, string _name, bool _isFile )
		{
			this.tID		= _TID;
			this.path		= _path;
			this.name		= _name;
			this.isFile		= _isFile;
			this.isVaild	= CheckValid( path );
		}
		private bool CheckValid( string path )
		{
			if( string.Empty == path )
				return false;

			if( true == Directory.Exists( path ) )
				return true;

			if( true == File.Exists( path ) )
				return true;

			return false;
		}
	}

	public class AssetBundleBuilder : Singleton<AssetBundleBuilder>
	{
		public List<AssetBundleData> lst { private set; get; }

		protected AssetBundleBuilder()
		{
			lst = new List<AssetBundleData>();
		}

		public void Initialize()
		{
			Utility.CreateDirectory( Utility.RESULT_ROOT_DIR );

			lst.Clear();

		}
	}
}