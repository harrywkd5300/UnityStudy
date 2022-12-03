
public class AppTables
{
	public bool isValid = false;

	public void Initialize()
	{
		if( !isValid )
		{
			try
			{
				OnRegisters();

				TableManager.Inst.Initialize( false );
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
		TableManager.Inst.Destroy();

		isValid = false;
	}

	private void OnRegisters()
	{
		OnRegister_Common();
	}
	private void OnRegister_Common()
	{
		TableManager.Inst.Register( StringTBL.Inst );
	}
}
