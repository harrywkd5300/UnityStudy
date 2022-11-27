using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UITexture : RawImage
	{
		protected override void Awake()
		{
			base.Awake();
			if( Application.isPlaying )
			{
			}
		}
	}
}
