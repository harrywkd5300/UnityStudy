using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace UI
{
	public class UIToggle : Toggle
	{		
		protected override void Start()
		{
			base.Start();
			if( Application.isPlaying ) 
			{
			}
		}
	}
}