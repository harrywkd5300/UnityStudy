using UnityEngine;
using System;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
	public UIScene			uiScene = null;
	public Stack<UIPopup>	uiPopup = new Stack<UIPopup>();

	protected UIManager()
	{

	}
}