using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : UIBase
{
	[SerializeField] Image image = null;

	public override void Init()
	{
	}
	public override void Clear()
	{
	}
	public override bool Shuts()
	{
		return false;
	}

	public void OnClick()
	{
		image.color = Color.red;
	}
}
