
public interface IUibase
{
	void Init( UI.Type type );
	void Clear();
	void Apply();
	void Refresh();
	bool Shuts( bool isShuts );
}