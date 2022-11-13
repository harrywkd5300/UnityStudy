using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour, IScene
{
	public void Awake()
	{
		AppMaster.Initialize();

		GameObject ui = AssetManager.Inst.MakeOBJ( "UI/Common/MainUI" );

		GameObject.DontDestroyOnLoad( ui );
	}
	public void OnBack()
	{

	}
}