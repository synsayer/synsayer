using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class LocalPrefs
{
	public class Debugging : LocalPrefsContainer
	{
		public string		logServerIP			= string.Empty;
		public int			logServerPort		= 8087;
		public string		logServerUI			= string.Empty;
		public string		slotLastSendChaet	= string.Empty;

#if UNITY_EDITOR
		public string		editorFBToken		= string.Empty;
#endif
	}
}
