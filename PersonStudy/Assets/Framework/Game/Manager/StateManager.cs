using System;
using System.Collections.Generic;

public class TStateManager<EState> where EState : struct, IComparable
{
	private		Dictionary<EState, IState>	m_dicState		= null;
	protected	Dictionary<EState, IState>	dicState		{ get { return m_dicState; } }

	public		EState						curType			{ private set; get; }
	public		IState						curState		{ private set; get; }
	public		EState						prevType		{ private set; get; }
	public		IState						prevState		{ private set; get; }

	public TStateManager()
	{
		m_dicState = new Dictionary<EState, IState>();
	}

	public void Add( EState eState, IState oState ) => dicState.Add( eState, oState );
	public void Remove( EState eState ) => dicState.Remove( eState );
	public void Destroy()
	{
		if( curState != null )
		{
			curState.Exit();
			curState = null;
		}
		if( prevState != null )
			prevState = null;

		dicState.Clear();
	}

	public IState Find( EState eState )
	{
		if( true != dicState.ContainsKey( eState ) )
			return null;

		return dicState[ eState ];
	}
	public bool IsState( EState eState )
	{
		return ( 0 == curType.CompareTo( eState ) ) ? true : false;
	}

	public bool ChangeState( EState eState, bool forced = false )
	{
		if( !forced )
		{
			if( IsState( eState ) )
				return false;
		}

		if( curState != null )
			curState.Exit();

		prevType = curType;
		prevState = curState;

		curType = eState;
		curState = Find( eState );

		if( curState != null )
			curState.Enter();

		return true;
	}
	public void Execute()
	{
		curState?.Execute();
	}
}