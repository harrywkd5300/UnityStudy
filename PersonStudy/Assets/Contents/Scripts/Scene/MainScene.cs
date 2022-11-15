using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour, IScene
{
	public void Awake()
	{
		AppMaster.Initialize();

		MainUI.ActiveUI<MainSceneUI>( UI.Root.Scene, UI.Type.SceneUI, new UIResc( "UI/Scene", "MainSceneUI" ), 1, true );
	}
	public void OnBack()
	{

	}
}