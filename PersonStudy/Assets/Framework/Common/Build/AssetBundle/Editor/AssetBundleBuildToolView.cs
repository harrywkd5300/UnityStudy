#if UNITY_EDITOR
namespace AssetBundle
{
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.IMGUI.Controls;

	public class AssetBundleBuildToolView : TreeView
	{
		public AssetBundleBuildToolView( TreeViewState state ) : base( state )
		{
			Initialize();
		}
		public AssetBundleBuildToolView( TreeViewState state, MultiColumnHeader multiColumnHeader )
			: base( state, multiColumnHeader )
		{
			Initialize();
		}

		private void Initialize()
		{
		}

		protected override TreeViewItem BuildRoot()
		{
			return null;
		}
		protected override void RowGUI( TreeView.RowGUIArgs args )
		{
		}
	}
}
#endif