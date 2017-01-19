using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Reflection;
using System.Net.Json;
//using LitJson;

public partial class LocalPrefs
{
	public class Account : LocalPrefsContainer
	{
		//public string		m_strUID					= string.Empty;
		public string		m_strAccessToken			= string.Empty;
		public string		m_strExpireTime				= string.Empty;
		public string		m_strFBID					= string.Empty;
		public string		m_strFBEditorAccessToken	= string.Empty;
		public string		m_strFBUID					= string.Empty;
		public string		m_strFBpOdinAccessToken		= string.Empty;
		public string		m_strGuestUID				= string.Empty;
		public string		m_strGuestpOdinAccessToken	= string.Empty;
		public string		m_strGuestNoticePopup		= string.Empty;
		public bool			m_bGuestNoticePopup			= false;
		public bool			m_bAlreadyConvertAirData	= false;			// 에어 데이터를 이미 cookielist 파일에서 가져 와서 저장 했는지.

		public void ClearFBPrefs()
		{
			m_strFBID				= string.Empty;
			m_strFBEditorAccessToken= string.Empty;
			m_strFBUID				= string.Empty;
			m_strFBpOdinAccessToken	= string.Empty;
			Save();

//            CGlobalInfo.gs_oInstance.gs_oAppInfo.gs_llUID = 0;
//            CGlobalInfo.gs_oInstance.gs_oAppInfo.gs_strodinAccessToken = "";                
		}

		public bool IsFBPrefsAreValid()
		{
			if( string.IsNullOrEmpty( m_strFBID ) || string.IsNullOrEmpty(m_strFBEditorAccessToken) || string.IsNullOrEmpty(m_strFBUID) || string.IsNullOrEmpty( m_strFBpOdinAccessToken ))
			{
				return false;
			}
			
			return true;
		}
	}
}
