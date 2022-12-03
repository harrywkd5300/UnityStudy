using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum SceneType
{
	MainScene,
	GamePlayScene,
	OcclustionScene,
}

public class SceneInfo : IState
{
	public SceneType sceneType { private set; get; }

	public SceneInfo( SceneType type )
	{
		this.sceneType = type;
	}

	public void Enter() => Debug.Log( string.Format( $"[SCENE] {sceneType.ToString()} enter!" ) );
	public void Exit()	=> Debug.Log( string.Format( $"[SCENE] {sceneType.ToString()} exit!" ) );
	public void Execute() { }
}

public class AppScene : Singleton<AppScene>
{
	static public IScene scene { private set; get; }

	private TStateManager<SceneType> stateMgr;
	private AsyncOperation asyncLoad;

	protected AppScene()
	{
		stateMgr = new TStateManager<SceneType>();

		Initialize();
	}
	private bool Initialize()
	{
		AddScene( SceneType.MainScene );
		AddScene( SceneType.GamePlayScene );

		return true;
	}
	public void Destroy()
	{
		if( stateMgr != null )
		{
			stateMgr.Destroy();
			stateMgr = null;
		}
	}
	private void AddScene( SceneType type )
	{
		stateMgr.Add( type, new SceneInfo( type ) );
	}

	static public void Push( IScene obj )
	{
		scene = obj;
	}
	static public void Load( SceneType type, bool force = false )
	{
		Inst.OnNextScene( scene as MonoBehaviour, type, 0, force );
	}

	public void OnNextScene( MonoBehaviour mono, SceneType gameScene, int subType, bool force )
	{
		bool isAdd = false;
		//!< special codes.~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		switch( gameScene )
		{
		case SceneType.OcclustionScene:
			{
				isAdd = true;
				LoadScene( SceneType.GamePlayScene );
			}
			break;
		}
		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		if( asyncLoad != null && asyncLoad.isDone == false )
			return;

		if( true != stateMgr.ChangeState( gameScene, force ) )
			return;

		mono.StartCoroutine( LoadSceneAsync( gameScene, isAdd ) );
	}
	public void LoadScene( SceneType scene )
	{
		SceneManager.LoadScene( scene.ToString() );
	}

	private IEnumerator LoadSceneAsync( SceneType scene, bool isAdd = false )
	{
		asyncLoad = SceneManager.LoadSceneAsync( scene.ToString() , isAdd ? LoadSceneMode.Additive : LoadSceneMode.Single );

		yield return asyncLoad;
	}
}
