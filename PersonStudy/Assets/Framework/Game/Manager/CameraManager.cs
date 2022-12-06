using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : Singleton<CameraManager>
{
	private Camera							mainCamera { get { return Camera.main; } }
	private UniversalAdditionalCameraData	cameraData = null;

	protected CameraManager()
	{
		cameraData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
	}

	public void AddCameraStack( Camera cam, bool is3D = false )
	{
		if( null == mainCamera )
			return;

		cameraData.cameraStack.Remove( cam );

		if( is3D )
		{
			cameraData.cameraStack.Insert( 0, cam );
		}
		else
		{
			if( cameraData.cameraStack.Count == 0 )
				cameraData.cameraStack.Insert( 0, cam );
			else
				cameraData.cameraStack.Insert( cameraData.cameraStack.Count - 1, cam );
		}
	}
	public void RemoveCameraStack( Camera cam)
	{
		cameraData.cameraStack.Remove( cam );
	}
}