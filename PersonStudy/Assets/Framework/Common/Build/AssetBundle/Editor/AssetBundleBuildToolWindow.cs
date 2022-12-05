#if UNITY_EDITOR
namespace AssetBundle
{
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.IMGUI.Controls;

	public partial class AssetBundleBuildToolWindow : EditorWindow
	{
		[MenuItem( "[AssetBundle]/Build" )]
		static public void ShowWindow()
		{
			AssetBundleBuildToolWindow window = AssetBundleBuildToolWindow.GetWindow<AssetBundleBuildToolWindow>( "Tool" );
			window.Initialize();
		}
	}

	public partial class AssetBundleBuildToolWindow : EditorWindow
	{
		private TreeViewState _treeViewState;
		private TreeViewState treeViewState { get { if( null == _treeViewState ) { _treeViewState = new TreeViewState(); } return _treeViewState; } }


		public void Initialize()
		{
			minSize = new Vector2( 450f, 800f );
		}
	}
}
#endif