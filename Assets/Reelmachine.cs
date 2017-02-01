using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Reelmachine : MonoBehaviour 
{
	[SerializeField]
	List<Symbol> m_oSymbols;

	bool readyToStop;

	// Use this for initialization
	void Start () 
	{	
		if (null != m_oSymbols) {
			float fTop = Helper.GetTopSymbolPosition (m_oSymbols);
			float fBottom = Helper.GetBottomSymbolPosition (m_oSymbols);

			for (int i = 0; i < m_oSymbols.Count; i++) {
				Symbol oSymbol = m_oSymbols [i];
				if (null != oSymbol) {
					oSymbol.SetBoundary (fTop, fBottom);
				}
			}
		}


//		StartCoroutine (crt_Roll ());
	}

	public void Roll()
	{
		readyToStop = false;
		StartCoroutine (crt_Roll ());
	}


	public void ReserveStop()
	{
		readyToStop = true;
	}

	IEnumerator crt_Prepare()
	{
		Vector3 pt = transform.localPosition;
		pt.y += 30;
		HOTween.To (transform, 0.3f, new TweenParms ().Prop ("localPosition", pt));
		yield return new WaitForSeconds(.5f);
		pt.y -= 30;
		HOTween.To (transform, 0.025f, new TweenParms ().Prop ("localPosition", pt));
		yield return new WaitForSeconds(0.025f);

	}

	IEnumerator crt_Roll()
	{
		yield return StartCoroutine (crt_Prepare ());

//		int nTotalStepCnt = 50;
		float fStepDura = 0.05f;
		while (false == readyToStop) {
//			nTotalStepCnt--;

			for (int i = 0; i < m_oSymbols.Count; i++) {
				Symbol oSymbol = m_oSymbols [i];
				if (null != oSymbol) {
					oSymbol.StepDown (-110f, fStepDura);
				}
			}

			yield return new WaitForSeconds(fStepDura);		
		}

		yield return StartCoroutine (crt_Finish ());

	}

	IEnumerator crt_Finish()
	{
		Vector3 pt = transform.localPosition;
		pt.y -= 55;
		Sequence sequence = new Sequence(new SequenceParms());
		// "Append" will add a tween after the previous one/s have completed
		sequence.Append(HOTween.To(transform, 0.02f, new TweenParms().Prop("localPosition", pt)));
		pt.y += 55;
		sequence.Append(HOTween.To(transform, 0.5f, new TweenParms().Prop("localPosition", pt)));

		sequence.Play();

//		HOTween.Punch (transform, 0.1f, new TweenParms (), .5f, 0.5f);
		yield break;
//		Vector3 pt = transform.localPosition;
//		pt.y += 55;
//		HOTween.To (transform, 1, new TweenParms ().Prop ("localPosition", pt));
//		yield return new WaitForSeconds(1);
//		pt.y -= 55;
//		HOTween.To (transform, 0.025f, new TweenParms ().Prop ("localPosition", pt));
//		yield return new WaitForSeconds(0.025f);

	}

	// Update is called once per frame
//	void FixedUpdate () 
//	{
//		Roll (1000f * Time.deltaTime);
//	}
//
//
//	void Roll(float fDelta)
//	{
//		if (null != m_oSymbols) {
//			for (int i = 0; i < m_oSymbols.Count; i++) {
//				Symbol oSymbol = m_oSymbols [i];
//				if (null != oSymbol) {
//					oSymbol.TranslateY (fDelta);
//				}
//			}
//
//			for (int i = 0; i < m_oSymbols.Count; i++) {
//				Symbol oSymbol = m_oSymbols [i];
//				if (null != oSymbol) {
//					oSymbol.ThrowTopIfBottom (m_fBottom, m_oSymbols, 110);
//				}
//			}
//		}
//	}



}
