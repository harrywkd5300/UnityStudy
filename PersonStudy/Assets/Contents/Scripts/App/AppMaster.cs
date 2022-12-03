

static public class AppMaster
{
	static public bool isValid = false;

	static public AppData		appData		= null;
	static public AppTables		appTables	= null;
	static public AppConfig		appConfig	= null;

	static public void Initialize()
	{
		if( !isValid )
		{
			if( null == appData )
			{
				appData = new AppData();
				appData.Initialize();
			}

			if( null == appTables )
			{
				appTables = new AppTables();
				appTables.Initialize();
			}

			if( null == appConfig )
			{
				appConfig = new AppConfig();
				appConfig.Initialize();
			}

			if( !MainUI.isValid ) MainUI.Initialize();

			isValid = true;
		}
	}
	static public void Destroy()
	{
		appData?.Destroy();
		appTables?.Destroy();
		appConfig?.Destroy();

		MainUI.Destroy();

		isValid = false;
	}
}
