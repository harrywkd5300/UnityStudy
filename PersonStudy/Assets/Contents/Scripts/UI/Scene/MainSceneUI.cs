using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : UIBase
{
	public System.Action onClickAssetBundle;
	public System.Action onClickAddressable;
	public System.Action onClickGamePlay;

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

	public void OnClickAssetBundleLoad()
	{
		onClickAssetBundle?.Invoke();
	}
	public void OnClickAddressable()
	{
		onClickAddressable?.Invoke();
	}
	public void OnClickInGame()
	{
		onClickGamePlay?.Invoke();
	}
}
