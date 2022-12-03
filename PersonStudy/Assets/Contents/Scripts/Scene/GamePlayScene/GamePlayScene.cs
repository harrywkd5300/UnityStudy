using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScene : MonoBehaviour, IScene
{
	public void Start()
	{
		try
		{
			AppScene.Push( this );
		}
		catch( System.Exception e )
		{
			Utility.Log.CrashProcess( e );
		}
	}

	public void OnBack()
	{

	}
}
