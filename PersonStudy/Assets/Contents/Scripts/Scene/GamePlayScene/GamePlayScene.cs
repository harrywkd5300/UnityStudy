using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScene : MonoBehaviour, IScene
{
	[SerializeField] Camera cam = null;

	public void Start()
	{
		try
		{
			AppScene.Push( this );

			CameraManager.Inst.AddCameraStack( cam, true );
		}
		catch( System.Exception e )
		{
			Utility.Log.CrashProcess( e );
		}
	}
	public void OnDestroy()
	{
		CameraManager.Inst.RemoveCameraStack( cam );
	}

	public void OnBack()
	{

	}
}
