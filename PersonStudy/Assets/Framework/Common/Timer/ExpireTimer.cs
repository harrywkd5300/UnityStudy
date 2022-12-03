using UnityEngine;
using System;
using System.Collections;

public struct ExpireTimer
{
	private	DateTime	_expireTime;
	public DateTime		expireTime		{ get { return _expireTime; } }
	public TimeSpan		remainTime		{ get { return ( _expireTime - DateTime.Now ); } }
	public bool			isExpire		{ get { return ( _expireTime <= DateTime.Now ) ? true : false; } }	

	public int			days			{ get { return remainTime.Days; } }
	public int			hours			{ get { return remainTime.Hours; } }
	public int			minutes			{ get { return remainTime.Minutes; } }
	public int			seconds			{ get { return remainTime.Seconds; } }
	public double		totalDays		{ get { return remainTime.TotalDays; } }
	public double		totalHours		{ get { return remainTime.TotalHours; } }
	public double		totalMinutes	{ get { return remainTime.TotalMinutes; } }
	public double		totalSeconds	{ get { return remainTime.TotalSeconds; } }

	public void Active( float seconds )
	{
		if( float.MinValue == seconds )
		{
			Active( TimeSpan.MinValue );
			return;
		}
		if( float.MaxValue == seconds )
		{
			Active( TimeSpan.MaxValue );
			return;
		}

		Active( new TimeSpan( (long)( seconds * 10000000 ) ) );
	}
	public void Active( int seconds )
	{
		Active( new TimeSpan( 0, 0, seconds ) );
	}
	public void Active( int hours, int minutes, int seconds )
	{
		Active( new TimeSpan( hours, minutes, seconds ) );
	}
	public void Active( TimeSpan time )
	{
		if( TimeSpan.MinValue == time )
		{
			_expireTime = System.DateTime.MinValue;
			return;
		}
		if( TimeSpan.MaxValue == time )
		{
			_expireTime = System.DateTime.MaxValue;
			return;
		}

		_expireTime = System.DateTime.Now + time;
	}
	public void Active( DateTime dtTime )
	{
		TimeSpan calc = dtTime - DateTime.Now;
		Active( calc );
	}
	public void Active( double endDate )
	{
		_expireTime = System.DateTime.FromOADate( endDate );
	}
}

