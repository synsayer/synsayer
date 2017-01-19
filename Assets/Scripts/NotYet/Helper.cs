using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;

public class Helper
{
	public static float GetBottomSymbolPosition(List<Symbol> lst)
	{
		float fBottom = float.MaxValue;
		for (int i = 0; i < lst.Count; i++) {
			Symbol oSymbol = lst [i];
			if (null != oSymbol) {
				if (oSymbol.transform.localPosition.y < fBottom) {
					fBottom = oSymbol.transform.localPosition.y;
				}
			}
		}
		return fBottom;
	}

	public static float GetTopSymbolPosition(List<Symbol> lst)
	{
		float fTop = -float.MaxValue;
		for (int i = 0; i < lst.Count; i++) {
			Symbol oSymbol = lst [i];
			if (null != oSymbol) {
				if (oSymbol.transform.localPosition.y > fTop) {
					fTop = oSymbol.transform.localPosition.y;
				}
			}
		}
		return fTop;
	}



    /// <summary>
    /// 레퍼런스 타입만 추려내서 스태틱이고 뭐고간에 전부 null로 비움.
    /// </summary>
    public static void ClearAllFields<T>(MonoBehaviour mono) where T : class
	{
		Type instType = mono.GetType ();
		FieldInfo[] allFields = instType.GetFields (BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		for (int i = 0; i < allFields.Length; i++) {
			if (allFields [i].FieldType.IsValueType == false) {
				//object obj = allFields[i].GetValue(mono);
				//if(null != obj && obj is UnityEngine.Object)
				//{
				//    NGUITools.DestroyImmediate(obj as UnityEngine.Object);
				//}

				allFields [i].SetValue (mono, null);
				Debug.Log ("Set null to: " + allFields [i].Name);
			}
		}


		Type staticType = typeof(T);
		FieldInfo[] allStatics = staticType.GetFields (BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
		for (int i = 0; i < allStatics.Length; i++) {
			if (allStatics [i].FieldType.IsValueType == false) {
				allStatics [i].SetValue (null, null);
				Debug.Log ("Set null to: " + allStatics [i].Name);
			}
		}
	}
//    public static SlotEffectController.WinEffectType GetWinEffectType(long llCurMultiplier)
//    {
//        if (llCurMultiplier >= 30)
//            return SlotEffectController.WinEffectType.MEGAWIN;
//        else if (llCurMultiplier >= 20)
//            return SlotEffectController.WinEffectType.SUPERWIN;
//        else if (llCurMultiplier >= 10)
//            return SlotEffectController.WinEffectType.BIGWIN;
//        else if (llCurMultiplier >= 5)
//            return SlotEffectController.WinEffectType.NICEWIN;
//        return SlotEffectController.WinEffectType.NONE;
//    }
//
//    public static NewSound.CommonFXType GetSFXType(long llCurMultiplier)
//    {
//        switch (GetWinEffectType(llCurMultiplier))
//        {
//            case SlotEffectController.WinEffectType.MEGAWIN: return NewSound.CommonFXType.MegaWin;
//            case SlotEffectController.WinEffectType.SUPERWIN: return NewSound.CommonFXType.SuperWin;
//            case SlotEffectController.WinEffectType.BIGWIN: return NewSound.CommonFXType.BigWin;
//            case SlotEffectController.WinEffectType.NICEWIN: return NewSound.CommonFXType.NiceWin;
//        }
//        return NewSound.CommonFXType.NONE;
//    }
//
//    
//
//    public static SlotWinnerChanceManager.ChanceType GetChanceType(long llCurMultiplier)
//    {
//        if (llCurMultiplier >= 30)
//            return SlotWinnerChanceManager.ChanceType.MEGA;
//        else if (llCurMultiplier >= 20)
//            return SlotWinnerChanceManager.ChanceType.SUPER;
//        else if (llCurMultiplier >= 10)
//            return SlotWinnerChanceManager.ChanceType.BIG;
//
//        return SlotWinnerChanceManager.ChanceType.NONE;
//    }
//
//    public static void MoneyFlyEffect(GameObject goFrom, System.Action fnOnArrive)
//    {
//        if (goFrom)
//        {
//            SplineTweener.PlayTween(new SplineTweenParam().SetFrom(goFrom.transform).
//                                                           SetTo(CHomeUI.gs_oCoinEffectTarget).
//                                                           SetMoveEffect(CHomeUI.gs_oCoinMoveEffect).
//                                                           SetArriveEffect(CHomeUI.gs_oCoinArriveEffect).
//                                                           SetOnArrive(fnOnArrive).
//                                                           SetStartSound(NewSound.CommonFXType.CoinSplineMove).
//                                                           SetArriveSound(NewSound.CommonFXType.CoinSplineArrive));
//        }
//    }

    public static int GetIntegerVersion(string value)
    {
        if (null == value || "" == value)
        {
            return 0;
        }
        string m_strAppVersion = value;

        string[] aTemp = m_strAppVersion.Split('.');

        int nMajorVersion = 0;
        int nMinorVersion = 0;
        int nHotfixVersion = 0;


        if (null != aTemp && aTemp.Length > 0)
        {
            if (1 == aTemp.Length)
            {
                nHotfixVersion = aTemp[0].ToInt() * 1;
            }
            else if (2 == aTemp.Length)
            {
                nMinorVersion = aTemp[0].ToInt() * 1000;
                nHotfixVersion = aTemp[1].ToInt() * 1;
            }
            else
            {
                nMajorVersion = aTemp[0].ToInt() * 1000000;
                nMinorVersion = aTemp[1].ToInt() * 1000;
                nHotfixVersion = aTemp[2].ToInt() * 1;
            }

            return nMajorVersion + nMinorVersion + nHotfixVersion;
        }

        return 0;
    }


    public static string setCipher(int time)
    {
        string strTime = time.ToString();
        if (time < 10)
        {
            strTime = "0" + time.ToString();
        }
        return strTime;
    }


    public static string GetTimeString(int nTotalTime)
    {
        int iRemainHour = nTotalTime / 60 / 60 % 24;
        int iRemainMinute = nTotalTime / 60 % 60;
        int iRemainSecond = nTotalTime % 60;

        string strHour = setCipher(iRemainHour);
        string strMinute = setCipher(iRemainMinute);
        string strSecond = setCipher(iRemainSecond);

        string m_oTimeText = strHour + ":" + strMinute + ":" + strSecond;

        return m_oTimeText;
    }
}
