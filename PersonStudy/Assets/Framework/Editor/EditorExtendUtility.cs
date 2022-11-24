#if UNITY_EDITOR

namespace EditTool
{
	using UnityEditor;

	public enum MsgType
	{
		Ok,
		OkCancel,
	}

	static public class Utility
	{
		static public void ShowMessageBox( MsgType msgType, System.Action<bool> onAction, string message, string title = "Notice" )
		{
			bool bRes = false;

			switch( msgType )
			{
			case MsgType.Ok:
				{
					bRes = EditorUtility.DisplayDialog( title, message, "Ok" );
				}
				break;
			case MsgType.OkCancel:
				{
					bRes = EditorUtility.DisplayDialog( title, message, "Ok", "Cancel" );
				}
				break;
			}

			if( null != onAction )
				onAction( bRes );
		}
	}
}

#endif