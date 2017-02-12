using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectView : MonoBehaviour 
{
	[SerializeField]
	GameObject moneyEffect;


	public void showMoneyEffect()
	{
		StartCoroutine (CrtShowMoneyEffect());
	}


	IEnumerator CrtShowMoneyEffect()
	{
		if (null != moneyEffect) {
			moneyEffect.SetActive (true);
			yield return new WaitForSeconds (3f);
			moneyEffect.SetActive (false);
		}
	}

	// Use this for initialization
	void Start () {
		if (null != moneyEffect) {
			moneyEffect.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
