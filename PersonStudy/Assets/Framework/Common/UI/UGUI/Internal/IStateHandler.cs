using UI;

public interface IStateHandler
{
	void UpdateState( State state, bool applyChild = false );
	void UpdateState( int state, bool applyChild = false );
}