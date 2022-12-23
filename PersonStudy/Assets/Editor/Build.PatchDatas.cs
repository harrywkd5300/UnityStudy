namespace Gong.Build
{
	using System.Collections.Generic;
	using UnityEngine;
	using System;

	[System.Serializable]
	public class PatchData
	{
		//!< TODO. HC 패치데이터에 맞게 수정. 추후 이 클래스를 HC에서 상속받아 AssetBundlePatchData 확장하던가 해야할듯..
		public string			url;
		public string			name;
		public int				bytes;			//< 번들 용량
		public uint				crc;			//< 번들 CRC 코드
		public string			fileHash;		//< 번들 hash 값( string )
		public string			revision;		//< 번들 리비전 (버전 체크용)
		public int				obsolete;		//< 번들 사용여부	
		public string			desc;			//< 번들 설명
		public int				phase;			//< 다운로드 페이즈
		public List<string>		dependencies;
		public bool				isStatic;

		public Hash128		hash			{ get { return Hash128.Parse( fileHash ); } }   //< 번들 hash 값( Hash128 )
		public string		fileName		{ get { return ( null != name && "" != name ) ? name + ".abf" : ""; } }
		public bool			isObsolete		{ get { return ( 0 < obsolete ) ? true : false; } }
		public bool			onDependencie	{ get { return 0 < dependencies.Count ? true : false; } }

		public PatchData( string url, string name, int bytes, uint crc, string hash, string revision, int obsolete, string desc, int phase, bool isStatic, string[] dependencies = null )
		{
			this.url			= url;
			this.name			= name;
			this.bytes			= bytes;
			this.crc			= crc;
			this.fileHash		= hash;
			this.revision		= revision;
			this.obsolete		= obsolete;
			this.desc			= desc;
			this.phase			= phase;
			this.isStatic		= isStatic;
			this.dependencies	= new List<string>();
			if( null != dependencies )
			{
				foreach( string dependencie in dependencies )
				{
					this.dependencies.Add( dependencie );
				}
			}
		}

		public bool Equals( PatchData data )
		{
			if( crc != data.crc )
				return false;
			if( false == fileHash.Equals( data.fileHash ) )
				return false;
			if( bytes != data.bytes )
				return false;
			return true;
		}
	}

	[System.Serializable]
	public class PatchDatas
	{
		[SerializeField] public List<PatchData> lstData;

		public int count { get { return ( null != lstData ) ? lstData.Count : 0; } }

		public PatchData this[ string bundleName ]
		{
			get
			{
				if( lstData == null ) return null;

				for( int i = 0; i < lstData.Count; ++i )
				{
					if( lstData[ i ].name.Equals( bundleName ) )
					{
						return lstData[ i ];
					}
				}

				return null;
			}
		}
		public PatchData this[ int idx ]
		{
			get
			{
				return ( lstData != null && 1 <= count && ( 0 <= idx && idx < ( lstData.Count ) ) ) ? lstData[ idx ] : null;
			}
		}

		public PatchDatas( int capacity = 1 )
		{
			this.lstData = new List<PatchData>( capacity );
		}
		public PatchDatas( List<PatchData> lstData )
		{
			this.lstData = lstData;
		}

		public void Add( string url, string name, int bytes, uint crc, string hash, string revision, bool isObsolete, string desc, int phase, bool isStatic, string[] dependencies = null )
		{
			Add( new PatchData( url, name, bytes, crc, hash, revision, isObsolete ? 1 : 0, desc, phase, isStatic, dependencies ) );
		}
		public void Add( PatchData data )
		{
			if( null != data )
				lstData.Add( data );
		}
		static public void CreateList( string path, PatchDatas list )
		{
			if( null != list )
			{
				JsonWriter.Write<List<PatchData>>( list.lstData, path, "PatchList.json" );
			}
		}

		static public PatchDatas LoadList( string path )
		{
			try
			{
				if( System.IO.File.Exists( path ) )
				{
					using( System.IO.StreamReader sr = new System.IO.StreamReader( path ) )
					{
						string json = sr.ReadToEnd();

						return JsonUtility.FromJson<PatchDatas>( json );
					}
				}
			}
			catch( Exception e )
			{
				return null;
			}

			return null;
		}
	}
}
