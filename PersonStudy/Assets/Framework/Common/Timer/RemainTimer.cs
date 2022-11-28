using UnityEngine;
using System.Collections;

public class RemainTimer
{
	private ExpireTimer					expireTimer;
	private System.Action				onExpire			= null;
	private System.Action<ExpireTimer>	onUpdate			= null;

	private IEnumerator					coRemainTimer		= null;
	private MonoBehaviour				mono				= null;
	private WaitForSeconds				waitTime			= new WaitForSeconds( 0.5f );

	public void Apply( MonoBehaviour behaviour, ExpireTimer timer, System.Action _onExpire, System.Action<ExpireTimer> _onUpdate )
	{
		this.mono			= behaviour;
		this.expireTimer	= timer;
		this.onExpire		= _onExpire;
		this.onUpdate		= _onUpdate;

		if ( null != coRemainTimer )
		{
			mono.StopCoroutine( coRemainTimer );
			coRemainTimer = null;
		}

		if( expireTimer.isExpire )
			return;

		coRemainTimer = CoRemainTimer();
		mono.StartCoroutine( coRemainTimer );
	}
	IEnumerator CoRemainTimer()
	{
		while( true )
		{
			if( expireTimer.isExpire )
			{
				onExpire?.Invoke();
				yield break;
			}

			onUpdate?.Invoke( expireTimer );

			yield return waitTime;
		}
	}
}
