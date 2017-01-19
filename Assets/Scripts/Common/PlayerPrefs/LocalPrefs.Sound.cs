using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Reflection;
//using System.Net.Json;
//using LitJson;

public partial class LocalPrefs
{
	public class Sound : LocalPrefsContainer
	{
		public double		m_fBGMVol	= 0;		
		public double		m_fFXVol	= 0;
		public bool			m_bBGMMute	= false;
		public bool			m_bFXMute	= false;
	}
}
