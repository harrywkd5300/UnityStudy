namespace Utility
{
	using UnityEditor;
	using UnityEngine;

	static public class Editor
	{
		public enum MsgType
		{
			Ok,
			OkCancel,
		}

		static public void OnLayoutLabelField( Color drawColor, string value, GUILayoutOption[] options = null )
		{
			GUI.color = drawColor;
			EditorGUILayout.LabelField( value, options );
			GUI.color = Color.white;
		}

		static public void ShowMessageBox( string message, string ok = "Ok", string title = "Notice" )
		{
#if UNITY_EDITOR
			ShowMessageBox( MsgType.Ok, null, message, ok, "", title );
#endif
		}
		static public void ShowMessageBox( MsgType msgType, System.Action<bool> onAction, string message, string ok = "Ok", string cancel = "Cancel", string title = "Notice" )
		{
			bool bRes = false;
			switch( msgType )
			{
			case MsgType.Ok:
				{
					bRes = EditorUtility.DisplayDialog( title, message, ok );
				}
				break;
			case MsgType.OkCancel:
				{
					bRes = EditorUtility.DisplayDialog( title, message, ok, cancel );
				}
				break;
			}

			if( null != onAction )
				onAction( bRes );
		}
	}
}