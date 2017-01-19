using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Net.Json;
using LitJson;


public partial class LocalPrefs
{
	public abstract class LocalPrefsContainer
	{
		protected JsonData		m_oJson	= null;

		public LocalPrefsContainer()
		{
			LocalPrefs.s_dicCategory.Add( this.GetType(), this );
		}


		/// <summary>
		/// 필드 초기화.
		/// </summary>
		void InitFields()
		{
			Type		thisType	= this.GetType();
			FieldInfo[]	fields		= thisType.GetFields();
			for(int i=0; i<fields.Length; i++)
			{
				SetField( fields[i].Name );
			}
		}


		/// <summary>
		/// 이 인스턴스를 Json으로 Serialize화 하여 PlayerPrefs에 저장.
		/// </summary>
		public void Save()
		{
			Type	thisType	= this.GetType();
			string	registerKey	= string.Format( "{0}{1}", s_strLocalPrefsRoot, thisType.Name );
			string	toJson		= JsonMapper.ToJson( this );

			if( Application.isPlaying == true )
			{
				Debug.Log( string.Format( "LocalPrefsContainer.cs: Save() -> {0}", toJson ) );
			}
			
			PlayerPrefs.SetString( registerKey, toJson );

#if UNITY_EDITOR
			LocalPrefsViewer.Refresh();
#endif
		}


		/// <summary>
		/// PlayerPrefs에 저장된 Json으로 Deserialize.
		/// </summary>
		public void Load()
		{
			Type	thisType	= this.GetType();
			string	registerKey	= string.Format( "{0}{1}", s_strLocalPrefsRoot, thisType.Name );

			if( PlayerPrefs.HasKey( registerKey ) == false )
			{
				m_oJson	= null;
			}
			else
			{
				string	jsonStr	= PlayerPrefs.GetString( registerKey );
				m_oJson			= JsonMapper.ToObject( jsonStr );

				if( Application.isPlaying == true )
				{
					Debug.Log( string.Format( "LocalPrefsContainer.cs: Load() -> {0}", jsonStr ) );
				}
			}

			
			InitFields();
		}


		/// <summary>
		/// 모든 필드 Clear.
		/// </summary>
		public void Clear()
		{
			Type	thisType	= this.GetType();
			string	registerKey	= string.Format( "{0}{1}", s_strLocalPrefsRoot, thisType.Name );

			if( PlayerPrefs.HasKey( registerKey ) == true )
			{
				m_oJson	= null;
				PlayerPrefs.DeleteKey( registerKey );
			}

			s_dicCategory.Remove( thisType );
			Activator.CreateInstance( thisType );

			LocalPrefsContainer	newContainer	= null;
			if( s_dicCategory.TryGetValue( thisType, out newContainer ) == true )
			{
				newContainer.Save();
			}
		}


		/// <summary>
		/// Json 데이터로 필드 초기화. 
		/// Json에 필드 데이터가 없으면 기본값으로 set.
		/// </summary>
		protected void SetField(string _strFieldName)
		{
			FieldInfo	field	= this.GetType().GetField( _strFieldName );
			object		outVal;

			if( m_oJson == null )
			{
				return;
			}

			if( m_oJson.TryGetValue( _strFieldName, field, out outVal ) == true )
			{
				field.SetValue( this, outVal );
			}
		}
	}
}