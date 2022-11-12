

public class AppData
{
	public bool isValid = false;

	public void Initialize()
	{
		if( !isValid )
		{
			isValid = true;
		}
	}
	public void Destroy()
	{
		isValid = false;
	}
}
