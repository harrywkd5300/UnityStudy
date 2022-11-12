using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour, IScene
{
	public void Awake()
	{
		AppMaster.Initialize();
	}
	public void OnBack()
	{

	}
}