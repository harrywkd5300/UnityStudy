using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScene : MonoBehaviour, IScene
{
	public void Start()
	{
		AppScene.Inst.LoadScene( "OcclustionScene", true );
	}

	public void OnBack()
	{

	}
}
