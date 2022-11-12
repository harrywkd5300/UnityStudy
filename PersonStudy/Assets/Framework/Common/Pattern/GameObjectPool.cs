using UnityEngine;
using System.Collections.Generic;

public class GameObjectPool : MonoBehaviour
{
	private GameObject			goRoot			= null;
	private List<GameObject>	goList			= null;

	public	GameObject			goPrefab		= null;
	public  int					initSize 		= 16;
	public	int					stepSize		= 8;

	void Awake()
	{
		Init();
	}
	void OnDestroy()
	{
		goList?.Clear();
		goList		= null;
		goRoot		= null;
		goPrefab	= null;
	}

	public void Init()
	{
		if( goRoot != null )
			return;

		goRoot = new GameObject( "_GoPool_" );
		if( goRoot != null )
		{
			goRoot.transform.parent			= transform;
			goRoot.transform.localPosition	= Vector3.zero;
			goRoot.transform.localRotation	= Quaternion.identity;
			goRoot.transform.localScale		= Vector3.one;
			goRoot.layer					= gameObject.layer;
			goRoot.SetActive( false );
		}

		goList = goList ?? new List<GameObject>( );

		if( goPrefab != null )
		{
			for( int i = 0; i < initSize; ++i )
			{
				Push( Clone() );
			}
		}
	}
	public void Setting( GameObject _goPrefab, int _initSize, int _stepSize )
	{
		goPrefab = _goPrefab;
		initSize = _initSize;
		stepSize = _stepSize;

		Init();

		if( goPrefab != null && goRoot != null && goList.Count == 0 )
		{
			for( int i = 0; i < initSize; ++i )
			{
				Push( Clone() );
			}
		}
	}

	public GameObject New()
	{
		GameObject go = Pop();
		if( go != null )
		{
			return go;
		}

		if( goPrefab != null )
		{
			for( int i = 0; i < stepSize; ++i )
			{
				Push( Clone() );
			}
		}
		return Pop();
	}
	public void Delete( GameObject go )
	{
		Push( go );
	}

	private GameObject Clone()
	{
		var go = GameObject.Instantiate( goPrefab ) as GameObject;
		{
		}
		return go;
	}
	private void Push( GameObject go )
	{
		if( goRoot != null && go != null )
		{
			go.transform.SetParent( goRoot.transform );
			go.layer = goRoot.layer;
			go.SetActive( false );
			goList.Add( go );
		}
	}
	private GameObject Pop()
	{
		if( 0 < goList.Count )
		{
			GameObject go = goList[ 0 ];
			goList.Remove( go );
			return go;
		}
		return null;
	}

	public GameObject AddChild( GameObject goParent )
	{
		GameObject goItem = this.New();
		if( goItem != null )
		{
			goItem.SetActive( true );
			Utility.Object.AttachUIObject( goParent, goItem, true );
		}
		return goItem;
	}
	public bool Remove( GameObject goParent )
	{
		if( 0 == goParent.transform.childCount )
			return false;

		Transform[] items = new Transform[ goParent.transform.childCount ];
		for( int i = 0; i < items.Length; ++i )
		{
			items[ i ] = goParent.transform.GetChild( i );
		}

		foreach( Transform item in items )
		{
			this.Delete( item.gameObject );
		}
		return true;
	}

	static public GameObjectPool MakeComponent( GameObject go, GameObject goPrefab, int initSize, int stepSize )
	{
		GameObjectPool goPool = go.AddComponent<GameObjectPool>();
		if( goPool == null )
		{
			Debug.LogError( " >>> [GameObjectPool] component add failed!!" );
			return null;
		}

		goPool.Setting( goPrefab, initSize, stepSize );
		return goPool;
	}
}