using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneUI : UIBase
{
	[SerializeField] UI.UILabel			txt_Name = null;

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

	public void Apply()
	{

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
