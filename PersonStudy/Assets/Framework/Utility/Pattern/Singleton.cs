using System;
using System.Reflection;

/// <summary>
/// 이 클래스는 Singleton 객체를 관리합니다.
/// 멀티 쓰레드 환경에서 문제가 생길 수 있어서 Thread safe 코드 추가
/// 이 클래스를 상속받은 하위 클래스의 생성자는 반드시 protected로 구현해야합니다.
/// protected로 구현된 생성자는 Singleton<T>을 상속받은 객체의 외부 생성 및 중복 생성을 방지합니다.
/// </summary>
/// 

public class Singleton<T> where T : class
{
	private static T		_instance	= null;
	private static object	_sync		= new object();

	public static T Inst
	{
		get
		{
			//!< lock은 비용이 많이 드니 한번 null 체크
			if( _instance == null )
			{
				//!< Thread safe code
				lock( _sync )
				{
					if( _instance == null )
					{
						Type t = typeof( T );

						//!< public으로 구현된 생성자가 있는지 확인합니다.
						ConstructorInfo[] ctors = t.GetConstructors();
						if( ctors.Length > 0 )
						{
							//!< Singleton<T>을 상속받은 하위 클래스의 생성자는 protected로 구현되어야합니다.
							throw new InvalidOperationException( string.Format( "{0} constructor exception.", t.Name ) );
						}

						_instance = (T)Activator.CreateInstance( t, true );
					}
				}
			}

			return _instance;
		}
	}
}