using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Holoville.HOTween;


public class Symbol : MonoBehaviour
{
	float m_fBottom = -float.MaxValue;
	float m_fTop = -float.MaxValue;

	public void SetBoundary(float fTop, float fBottom)
	{
		m_fTop = fTop;
		m_fBottom = fBottom;
	}

	public void StepDown(float fDIst, float fDura)
	{
		ThrowTopIfBottom(m_fBottom, m_fTop + 110);
		Vector3 pt = transform.localPosition;
		pt.y += fDIst;
		HOTween.To (transform, fDura, new TweenParms ().Prop ("localPosition", pt).Ease(EaseType.Linear).OnComplete(()=>
			{
//				ThrowTopIfBottom

			}));
	}





	public void TranslateY(float fDelta)
	{
		Vector3 pt = transform.localPosition;
		transform.localPosition = pt + Vector3.down * fDelta;
	}

	public void SetY(float fY)
	{
		Vector3 pt = transform.localPosition;
		pt.y = fY;
		transform.localPosition = pt;
	}

//	public void ThrowTopIfBottom(float fBottom, List<Symbol> lst, float fInterval)
//	{
//		if (transform.localPosition.y < fBottom) {
//			float fTop = Helper.GetTopSymbolPosition (lst) + fInterval;
//			SetY (fTop);
//		}
//	}

	void ThrowTopIfBottom(float fBottom, float fTop)
	{
		if (transform.localPosition.y <= fBottom) {
//			float fTop = Helper.GetTopSymbolPosition (lst) + fInterval;
			SetY (fTop);
		}
	}
}
