namespace Gong.Build
{
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using System.Linq;

	public partial class Utility //!< AssetBundle
	{
		//!< Setup
		static public void RemoveAllAssetBundle()
		{
			string[] allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
			foreach( string it in allAssetBundleNames )
			{
				AssetDatabase.RemoveAssetBundleName( it, true );
			}
		}
		static public void SetupAssetBundleToScenes( string rootPath )
		{
			string[] scenes = System.IO.Directory.GetFiles( rootPath
				, "*.unity"
				, System.IO.SearchOption.AllDirectories
				);

			if( scenes.Length <= 0 )
				return;

			SetupAssetBundleToScenes( scenes );
		}
		static public void SetupAssetBundleToScenes( string[] scenes )
		{
			foreach( string it in scenes )
			{
				string abName = System.IO.Path.GetFileNameWithoutExtension( it ).ToLower();
				AssetImporter importer = AssetImporter.GetAtPath( it );
				if( null != importer )
				{
					importer.assetBundleName = abName;
					{
						Log.Output( "<<set>> AssetBundle : {0}, {1}", abName, it );
					}
				}
			}
		}
		static public void SetupAssetBundleToFolders( string rootPath, string exclude )
		{
			string root = rootPath.Replace( '\\', '/' );

			SetupAssetBundleToFolder( root, root.ToLower(), exclude );
		}
		static private void SetupAssetBundleToFolder( string path, string lwRoot, string exclude )
		{
			const string skipKeyword = ".svn";

			//!< skip.
			if( path.ToLower().Contains( skipKeyword ) )
				return;

			//!< check.
			bool isAB = false;
			{
				List<string> dirs = new List<string>( System.IO.Directory.GetDirectories( path ) );
				{
					dirs = dirs.FindAll( ( obj ) => { return !obj.ToLower().Contains( skipKeyword ); } );
				}
				if( 0 < dirs.Count )
				{
					if( false == string.IsNullOrWhiteSpace( exclude ) )
					{
						foreach( string it in dirs )
						{
							if( it.Contains( exclude ) )
							{
								isAB = true;
								break;
							}
						}
					}
				}
				else
				{
					string[] files = System.IO.Directory.GetFileSystemEntries( path );
					if( 0 < files.Length )
						isAB = true;
				}
			}

			//!< set or sub.
			if( isAB )
			{
				string abPath = path.Replace( '\\', '/' ).Replace( "./", "" );
				string abName = string.Empty;
				{
					string lwPath = abPath.ToLower();
					int index = lwPath.IndexOf( lwRoot );
					if( -1 < index )
					{
						Log.Output( "[STRING] {2} <length> {0}, <lwRoot> {3} <startIndex> {1}", lwPath.Length, index + lwRoot.Length + 1, lwPath, lwRoot );
						lwPath = lwPath.Substring( index + lwRoot.Length + 1 );
					}
					abName = lwPath.Replace( '/', '_' ).Replace( '.', '_' );
				}

				AssetImporter importer = AssetImporter.GetAtPath( abPath );
				if( null != importer )
				{
					importer.assetBundleName = abName;
					{
						Log.Output( "<<set>> AssetBundle : {0}, {1}", abName, abPath );
					}
				}
			}
			else
			{
				foreach( string it in System.IO.Directory.GetDirectories( path ) )
				{
					SetupAssetBundleToFolder( it, lwRoot, exclude );
				}
			}
		}

		//!< Patch
#if _MLB_
		static public bool OutputAssetBundlePatchList( string abOutputPath, string outputPath, string outputFile, string revision, string urlRoot, string urlSub, string extension )
		{
			if( true == string.IsNullOrEmpty( abOutputPath ) )
				return false;

			string manifest = System.IO.Path.GetFileName( abOutputPath );
			string manifestPath = string.Format( "{0}/{1}", abOutputPath, manifest );
			{
				uint mfCRC = 0;
				if( false == BuildPipeline.GetCRCForAssetBundle( manifestPath, out mfCRC ) )
				{
					Log.Error( "### Manifest file is not exists. : {0}", manifestPath );
					return false;
				}
			}
			AssetBundle ab = AssetBundle.LoadFromFile( manifestPath );
			if( null == ab )
				return false;

			bool result = false;
			{
				AssetBundleManifest mf = ab.LoadAsset<AssetBundleManifest>( "AssetBundleManifest" );
				if( null != mf )
					result = OutputAssetBundlePatchList( mf, abOutputPath, outputPath, outputFile, revision, urlRoot, urlSub, extension );

				ab.Unload( true );
			}
			return result;
		}
		static public bool OutputAssetBundlePatchList( AssetBundleManifest manifest, string abOutputPath, string outputPath, string outputFile, string revision, string urlRoot, string urlSub, string extension )
		{
			if( null == manifest )
				return false;
			if( true == string.IsNullOrEmpty( abOutputPath ) )
				return false;
			if( true == string.IsNullOrEmpty( outputPath ) )
				return false;
			if( true == string.IsNullOrEmpty( outputFile ) )
				return false;
			if( true == string.IsNullOrEmpty( revision ) )
				return false;
			if( true == string.IsNullOrEmpty( urlSub ) )
				return false;

			string[] abNames = manifest.GetAllAssetBundles();
			if( abNames.Length <= 0 )
				return false;

			Log.Output( "### AssetBundle Path : {0}", abOutputPath );
			Log.Output( "### AssetBundle Count : {0}", abNames.Length );
			Log.Output( "### Patch Output Path : {0}", outputPath );
			Log.Output( "### Patch Info(s) : {1}/{2}/*.{3}", outputPath, urlRoot, urlSub, extension );

			PatchDatas oldPatchList = null;
			{
				string oldPatchPath = string.Format( "{0}/{1}", outputPath, outputFile );
				if( System.IO.File.Exists( oldPatchPath ) )
					oldPatchList = JsonUtility.FromJson<PatchDatas>( System.IO.File.ReadAllText( oldPatchPath ) );
			}
			PatchDatas newPatchList = new PatchDatas( abNames.Length );
			{
				newPatchList.SetRevision( revision );
				newPatchList.SetRootURL( urlRoot );
				newPatchList.SetExtension( extension );
			}
			List<PatchData> lstPatch = new List<PatchData>( abNames.Length );

			foreach( string it in abNames )
			{
				string abPath = string.Format( "{0}/{1}", abOutputPath, it );
				string assetHash = manifest.GetAssetBundleHash( it ).ToString();
				uint crc = 0; BuildPipeline.GetCRCForAssetBundle( abPath, out crc );
				int fileBytes = System.IO.File.ReadAllBytes( abPath ).Length;

				PatchData patchData = new PatchData();
				{
					patchData.url = urlSub;
					patchData.name = it;
					patchData.fileHash = assetHash;
					patchData.crc = crc;
					patchData.bytes = fileBytes;
				}

				PatchData oldPatchData = null;
				{
					if( null != oldPatchList )
						oldPatchData = oldPatchList.GetData( it );

					if( null == oldPatchData )
					{
						lstPatch.Add( patchData );
						{
							Log.Output( "\t@ New Bundle : {0}", it );
						}
					}
					else
					{
						if( true == patchData.Equals( oldPatchData ) )
						{
							patchData.url = oldPatchData.url; //!< 경로 유지
						}
						else
						{
							lstPatch.Add( patchData );
							{
								Log.Output( "\t@ Change Bundle : {0}", it );
							}
						}
					}
				}
				newPatchList.AddData( patchData );
			}

			Log.Output( "### Patch Data(s) : Count({0})", lstPatch.Count );

			if( 0 < lstPatch.Count )
			{
				string abPatchFilePath = string.Format( "{0}/_temp_{1}", abOutputPath, outputFile );
				string jsonString = JsonUtility.ToJson( newPatchList, true );
				System.IO.File.WriteAllText( abPatchFilePath, jsonString, System.Text.Encoding.UTF8 );

				string abNewPatchPath = string.Format( "{0}/{1}", outputPath, revision );
				{
					if( false == System.IO.Directory.Exists( abNewPatchPath ) )
						System.IO.Directory.CreateDirectory( abNewPatchPath );
				}

				//!< PatchList(Json) File Copy To PatchOutputRevisinPath
				string srcPath = abPatchFilePath;
				string destPath = string.Format( "{0}/{1}", abNewPatchPath, outputFile );
				System.IO.File.Copy( srcPath, destPath, true );

				//!< PatchData(AssetBundle) File(s) Copy To PatchOutputRevisinPath
				foreach( PatchData data in lstPatch )
				{
					srcPath = string.Format( "{0}/{1}", abOutputPath, data.name );
					destPath = string.Format( "{0}/{1}", abNewPatchPath, data.name );
					{
						if( false == string.IsNullOrEmpty( extension ) )
							destPath = string.Format( "{0}.{1}", destPath, extension );
					}
					System.IO.File.Copy( srcPath, destPath, true );
				}

				//!< PatchList(Json) File Copy To PatchOutputPath
				srcPath = abPatchFilePath;
				destPath = string.Format( "{0}/{1}", outputPath, outputFile );
				System.IO.File.Copy( srcPath, destPath, true );
			}

			Log.Output( "### Patch Data(s) copy complete!!!" );
			return true;
		}
#else
		static public bool OutputAssetBundlePatchList( string abOutputPath, string outputPath, string outputFile, string revision, string urlRoot, string urlSub, string projectType, string extension )
		{
			if( true == string.IsNullOrEmpty( abOutputPath ) )
				return false;

			string manifest = System.IO.Path.GetFileName( abOutputPath );
			string manifestPath = string.Format( "{0}/{1}", abOutputPath, manifest );
			{
				uint mfCRC = 0;
				if( false == BuildPipeline.GetCRCForAssetBundle( manifestPath, out mfCRC ) )
				{
					Log.Error( "### Manifest file is not exists. : {0}", manifestPath );
					return false;
				}
			}
			AssetBundle ab = AssetBundle.LoadFromFile( manifestPath );
			if( null == ab )
				return false;

			bool result = false;
			{
				AssetBundleManifest mf = ab.LoadAsset<AssetBundleManifest>( "AssetBundleManifest" );
				if( null != mf )
					result = OutputAssetBundlePatchList( mf, abOutputPath, outputPath, outputFile, revision, urlRoot, urlSub, projectType, extension );

				ab.Unload( true );
			}
			return result;
		}
		static public bool OutputAssetBundlePatchList( AssetBundleManifest manifest, string abOutputPath, string outputPath, string outputFile, string version, string urlRoot, string urlSub, string projectType, string extension )
		{
			if( null == manifest )
				return false;
			if( true == string.IsNullOrEmpty( abOutputPath ) )
				return false;
			if( true == string.IsNullOrEmpty( outputPath ) )
				return false;
			if( true == string.IsNullOrEmpty( outputFile ) )
				return false;
			if( true == string.IsNullOrEmpty( version ) )
				return false;
			if( true == string.IsNullOrEmpty( urlSub ) )
				return false;

			string[] abNames = manifest.GetAllAssetBundles();
			if( abNames.Length <= 0 )
				return false;

			Log.Output( "### AssetBundle Path : {0}", abOutputPath );
			Log.Output( "### AssetBundle Count : {0}", abNames.Length );
			Log.Output( "### Patch Output Path : {0}", outputPath );
			Log.Output( "### Patch Info(s) : {1}/{2}/*.{3}", outputPath, urlRoot, projectType, extension );

			int lastRevision		= GetLastRevision( abOutputPath );
			int newRevision			= lastRevision + 1;
			string gameVersion		= Application.version;
			string bundleVersion	= gameVersion.Substring( 0, gameVersion.Length - 2 ).Replace( ".", "" ).Replace( "\n", "" );

			string rootUrl			= string.Format( $"{urlRoot}/{projectType}/{bundleVersion}/{urlSub}/{newRevision.ToString()}" );

			//!< 이전 리비전 체크
			PatchDatas oldPatchList = null;
			{
				string oldPatchPath = string.Format( "{0}/{1}/{2}", outputPath, lastRevision, outputFile );
				oldPatchList = PatchDatas.LoadList( oldPatchPath );
				//if( System.IO.File.Exists( oldPatchPath ) )
				//	oldPatchList = JsonUtility.FromJson<PatchDatas>( System.IO.File.ReadAllText( oldPatchPath ) );
			}

			PatchDatas newPatchList = new PatchDatas( abNames.Length );
			List<PatchData> lstPatch = new List<PatchData>( abNames.Length );

			foreach( string it in abNames )
			{
				string abPath			= string.Format( "{0}/{1}", abOutputPath, it );
				string assetHash		= manifest.GetAssetBundleHash( it ).ToString();
				uint crc				= 0;		BuildPipeline.GetCRCForAssetBundle( abPath, out crc );
				int fileBytes			= System.IO.File.ReadAllBytes( abPath ).Length;
				string[] dependencies	= manifest.GetAllDependencies( it );
				string url				= string.Format( $"{rootUrl}/{it}.{extension}" );

				PatchData patchData = new PatchData( url, it, fileBytes, crc, assetHash, newRevision.ToString(), 0, it, 1, false, dependencies );

				PatchData oldPatchData = null;
				{
					if( null != oldPatchList )
						oldPatchData = oldPatchList[ it ];

					if( null == oldPatchData )
					{
						lstPatch.Add( patchData );
						{
							Log.Output( "\t@ New Bundle : {0}", it );
						}
					}
					else
					{
						if( true == patchData.Equals( oldPatchData ) )
						{
							patchData.url = oldPatchData.url; //!< 경로 유지
						}
						else
						{
							lstPatch.Add( patchData );
							{
								Log.Output( "\t@ Change Bundle : {0}", it );
							}
						}
					}
				}
				newPatchList.Add( patchData );
			}

			Log.Output( "### Patch Data(s) : Count({0})", lstPatch.Count );

			if( 0 < lstPatch.Count )
			{
				string abNewPatchPath = string.Format( "{0}/{1}", outputPath, newRevision.ToString() );
				{
					if( false == System.IO.Directory.Exists( abNewPatchPath ) )
						System.IO.Directory.CreateDirectory( abNewPatchPath );
				}

				string abPatchFilePath = string.Format( "{0}/{1}", abNewPatchPath, outputFile );
				string jsonString = JsonUtility.ToJson( newPatchList, true );
				JsonWriter.Write<List<PatchData>>( newPatchList.lstData, abNewPatchPath, outputFile );
				//System.IO.File.WriteAllText( abPatchFilePath, jsonString, System.Text.Encoding.UTF8 );

				//!< PatchData(AssetBundle) File(s) Copy To PatchOutputRevisinPath
				foreach( PatchData data in lstPatch )
				{
					string srcPath	= string.Format( "{0}/{1}", outputPath, data.name );
					string destPath = string.Format( "{0}/{1}", abNewPatchPath, data.name );
					{
						if( false == string.IsNullOrEmpty( extension ) )
							destPath = string.Format( "{0}.{1}", destPath, extension );
					}
					System.IO.File.Copy( srcPath, destPath, true );
				}
			}

			Log.Output( "### Patch Data(s) copy complete!!!" );
			return true;
		}

#endif
	}

	public partial class Utility //!< Upload
	{
		static public bool UploadPathToFTP( string localPath, string urlSubPath, string ftpDomain, string account, string password, bool isMakeDir )
		{
			return UploadPathToFTP( localPath, urlSubPath, ftpDomain, new System.Net.NetworkCredential( account, password ), isMakeDir );
		}
		static public bool UploadPathToFTP( string localPath, string urlSubPath, string ftpDomain, System.Net.NetworkCredential credential, bool isMakeDir )
		{
			if( string.IsNullOrEmpty( ftpDomain ) )
				return false;
			if( string.IsNullOrEmpty( urlSubPath ) )
				return false;
			if( string.IsNullOrEmpty( localPath ) )
				return false;

			//!< upload file.
			if( System.IO.File.Exists( localPath ) )
			{
				string _fileName = System.IO.Path.GetFileName( urlSubPath );
				string _urlSubDir = System.IO.Path.GetDirectoryName( urlSubPath ).Replace( '\\', '/' );
				string _fileUrl = string.Format( "{0}/{1}/{2}", ftpDomain, _urlSubDir, _fileName );

				if( true == isMakeDir )
				{
					if( false == MakeDirectoryToFTP( ftpDomain, _urlSubDir, credential ) )
					{
						return false;
					}
				}

				bool result = UploadFileToFTP( localPath, _fileUrl, credential );
				return result;
			}

			//!< upload directory's file(s)
			if( System.IO.Directory.Exists( localPath ) )
			{
				string _urlSubDir = urlSubPath.Replace( '\\', '/' );

				if( isMakeDir )
				{
					if( false == MakeDirectoryToFTP( ftpDomain, _urlSubDir, credential ) )
					{

						return false;
					}
				}

				string[] files = System.IO.Directory.GetFiles( localPath );
				foreach( string it in files )
				{
					string _fileName = System.IO.Path.GetFileName( it );
					string _fileURL = string.Format( "{0}/{1}/{2}", ftpDomain, _urlSubDir, _fileName );

					if( false == UploadFileToFTP( it, _fileURL, credential ) )
					{
						return false;
					}
				}

				//!< upload sub directory.
				foreach( string it in System.IO.Directory.GetDirectories( localPath ) )
				{
					string _subPath = System.IO.Path.GetFileName( it );
					string _localPath = string.Format( "{0}/{1}", localPath, _subPath );
					string _urlSubPath = _subPath;
					string _urlRootPath = string.Format( "{0}/{1}", ftpDomain, _urlSubDir );

					if( false == UploadPathToFTP( _localPath, _urlSubPath, _urlRootPath, credential, true ) )
					{
						return false;
					}
				}
				return true;
			}

			return false;
		}

		static private bool UploadFileToFTP( string localFilePath, string urlFilePath, System.Net.NetworkCredential credential )
		{
			if( string.IsNullOrEmpty( urlFilePath ) )
				return false;
			if( string.IsNullOrEmpty( localFilePath ) )
				return false;
			if( false == System.IO.File.Exists( localFilePath ) )
				return false;

			var request = System.Net.WebRequest.Create( urlFilePath ) as System.Net.FtpWebRequest;
			if( null == request )
			{
				Log.Error( "### [FTP] FtpWebRequest create failed! : url({0})", urlFilePath );
				return false;
			}

			request.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
			request.Credentials = credential;
			request.UseBinary = true;
			{
				System.IO.Stream requestStream = request.GetRequestStream();
				if( requestStream != null )
				{
					byte[] fileBytes = System.IO.File.ReadAllBytes( localFilePath );
					requestStream.Write( fileBytes, 0, fileBytes.Length );
				}
				requestStream.Close();
			}

			try
			{
				using( var response = request.GetResponse() as System.Net.FtpWebResponse )
				{
					Log.Output( "### [FTP] File upload success! : file({1}) > url({2}), {0}"
						, response.StatusDescription
						, localFilePath
						, urlFilePath
						);

					response.Close();
					return true;
				}
			}
			catch( System.Exception e )
			{
				Log.Error( "### [FTP] : file({1}) > url({2}), {0}"
					, e.Message
					, localFilePath
					, urlFilePath
					);

				return false;
			}
		}
		static private bool MakeDirectoryToFTP( string ftpDomain, string subUrlDir, System.Net.NetworkCredential credential )
		{
			if( string.IsNullOrEmpty( ftpDomain ) )
				return false;
			if( string.IsNullOrEmpty( subUrlDir ) )
				return false;

			string url = ftpDomain;
			string[] token = subUrlDir.Split( '/' );

			foreach( string it in token )
			{
				url = string.Format( "{0}/{1}", url, it );

				int exists = IsExistsDirectoryToFTP( url + "/", credential );
				if( exists < 0 )
					return false;
				if( exists == 1 )
					continue;

				var request = System.Net.WebRequest.Create( url ) as System.Net.FtpWebRequest;
				if( null == request )
				{
					Log.Error( "### [FTP] FtpWebRequest create failed! : url({0})", url );
					return false;
				}

				request.Method = System.Net.WebRequestMethods.Ftp.MakeDirectory;
				request.Credentials = credential;

				try
				{
					using( var response = request.GetResponse() as System.Net.FtpWebResponse )
					{
						Log.Output( "### [FTP] Directory make success! : {0}"
							, response.StatusDescription
							);
						response.Close();
					}
				}
				catch( System.Exception e )
				{
					Log.Error( "### [FTP] {0}", e.Message );

					return false;
				}
			}
			return true;
		}
		static private int IsExistsDirectoryToFTP( string url, System.Net.NetworkCredential credential )
		{
			var request = System.Net.WebRequest.Create( url ) as System.Net.FtpWebRequest;
			if( null == request )
			{
				Log.Error( "### [FTP] FtpWebRequest create failed! : url({0})", url );
				return -1;
			}

			request.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
			request.Credentials = credential;

			try
			{
				using( var response = request.GetResponse() as System.Net.FtpWebResponse )
				{
					response.Close();
					return 1;
				}
			}
			catch( System.Net.WebException e )
			{
				var response = e.Response as System.Net.FtpWebResponse;
				if( response.StatusCode == System.Net.FtpStatusCode.ActionNotTakenFileUnavailable )
				{
					return 0;
				}
				else
				{
					Log.Error( "### [FTP] {0}", e.Message );
					return -1;
				}
			}
		}

		static public int GetLastRevision( string abOutputPath )
		{
			int revision = 0;

			if( System.IO.Directory.Exists( "./" + abOutputPath ) )
			{
				string[] files = System.IO.Directory.GetDirectories( "./" + abOutputPath );

				foreach( string file in files )
				{
					string[] split = file.Split( '/', '\\' );

					int temp = int.Parse( split[ split.Length - 1 ] );

					revision = ( revision < temp ) ? temp : revision;
				}
			}
			return revision;
		}
	}
}
