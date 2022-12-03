using UnityEngine;
using AssetBundle;

public class ConfigData
{
	public string	gameVersion		{ get { return Application.version; } }
	public bool		isPatchMode;
	public bool		isAssetBundle;

	public ConfigData()
	{

	}
}

public class Config
{
	public const string path = "Config";
	public const string file = "Config";

	private ConfigData data;
	public ConfigData appConfig
	{
		get
		{
			if( data == null )
				data = Load();

			return data;
		}
	}

	public Config()
	{
		data = Load();
	}

	static public ConfigData Load()
	{
		ConfigData config;

		TextAsset ta = AssetManager.Inst.LoadResources<TextAsset>( path, file );
		if( null == ta )
		{
			Utility.Log.Error( "[Config]", "TextAsset load failed.." );
			return null;
		}

		config = JsonReader.Read<ConfigData>( ta.text );
		if( null == config )
		{
			Utility.Log.Error( "[Config]", "JsonReader failed.." );
			return null;
		}

		return config;
	}
}
