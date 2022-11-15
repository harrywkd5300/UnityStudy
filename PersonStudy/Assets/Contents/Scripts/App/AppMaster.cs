

static public class AppMaster
{
	static public bool isValid = false;

	static public AppData	 appData = null;

	static public void Initialize()
	{
		if( !isValid )
		{
			if( null != appData )
			{
				appData = new AppData();
				appData.Initialize();
			}

			if( !MainUI.isValid ) MainUI.Initialize();

			isValid = true;
		}
	}
	static public void Destroy()
	{
		appData?.Destroy();

		MainUI.Destroy();

		isValid = false;
	}
}
