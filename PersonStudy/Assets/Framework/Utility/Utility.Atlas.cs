
public enum SpriteAtlasType
{
	None,

	Common,
	Icon,

	Max
}

namespace Utility
{
	using UnityEngine.U2D;
	using AssetBundle;

	static public class Atlas
	{
		static public SpriteAtlas GetSpriteAtlas( SpriteAtlasType type )
		{
			return GetSpriteAtlas( type.ToString() );
		}
		static public SpriteAtlas GetSpriteAtlas( string name )
		{
			return AssetManager.Inst.LoadResources<SpriteAtlas>( "Atlas", name );
		}
	}
}