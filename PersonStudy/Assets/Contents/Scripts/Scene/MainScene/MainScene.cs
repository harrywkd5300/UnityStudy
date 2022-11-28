using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour, IScene
{
	private MainSceneUI sceneUI = null;

	public void Start()
	{
		try
		{
			AppMaster.Initialize();

			AppScene.Push( this );

			Initialize();
		}
		catch( System.Exception e )
		{
			Utility.Log.CrashProcess( e );
		}
	}

	private void Initialize()
	{
		sceneUI = MainUI.ActiveUI<MainSceneUI>( UI.Root.Scene, UI.Type.MainSceneUI, new UIResc( "UI/Scene", "MainSceneUI" ), 1, true );
		{
			sceneUI.onClickAssetBundle	= OnClickAssetBundle;
			sceneUI.onClickAddressable	= OnClickAddressable;
			sceneUI.onClickGamePlay		= OnClickGamePlay;
		}
	}
	public void OnDestroy()
	{
		MainUI.InactiveUI<MainSceneUI>( UI.Type.MainSceneUI );
	}

	public void OnBack()
	{

	}

	private void OnClickAssetBundle()
	{
		Utility.Log.Error( "[MainScene]", "AssetBundle Load" );
	}
	private void OnClickAddressable()
	{
		Utility.Log.Error( "[MainScene]", "Addressable Load" );
	}
	private void OnClickGamePlay()
	{
		Utility.Log.Error( "[MainScene]", "Game Start" );

		AppScene.Load( SceneType.OcclustionScene );
	}
}