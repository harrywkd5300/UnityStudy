using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
	MainScene,
	GamePlayScene,
	OcclustionScene,
}

public class AppScene : Singleton<AppScene>
{
	static public IScene scene { private set; get; }

	protected AppScene()
	{

	}

	static public void Load( SceneType type, bool force = false )
	{
		Inst.OnNextScene( scene as MonoBehaviour, type, 0, force );
	}

	public void OnNextScene( MonoBehaviour mono, SceneType gameScene, int subType, bool force )
	{
		//!< special codes.~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		switch( gameScene )
		{
		case SceneType.GamePlayScene:
			{

			}
			break;
		}
		//!< ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

		LoadScene( gameScene.ToString() );
	}
	public void LoadScene( string sceneName, bool isAdd = false )
	{
		SceneManager.LoadSceneAsync( sceneName, isAdd ? LoadSceneMode.Additive : LoadSceneMode.Single );
	}
}
