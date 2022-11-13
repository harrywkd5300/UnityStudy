using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MainUI : MonoBehaviour
{
	[SerializeField] Camera rootScene = null;
	[SerializeField] Camera rootPopup = null;

	public void Awake()
	{
		var mainCam = Camera.main?.GetUniversalAdditionalCameraData();

		mainCam?.cameraStack.Add( rootScene );
		mainCam?.cameraStack.Add( rootPopup );
	}
}