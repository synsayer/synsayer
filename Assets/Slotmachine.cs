﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slotmachine : MonoBehaviour {

	[SerializeField]
	List<Reelmachine> m_oReels;
	// Use this for initialization
	void Start () {
		
	}

	public void Spin()
	{
		StartCoroutine (crt_Spin ());
	}

	IEnumerator crt_Spin()
	{
		if (null != m_oReels) {
			for (int i = 0; i < m_oReels.Count; i++) {
				m_oReels [i].Roll ();
				yield return new WaitForSeconds (0.05f);
			}

			yield return new WaitForSeconds (2f);

			for (int i = 0; i < m_oReels.Count; i++) {
				m_oReels [i].ReserveStop ();
				yield return new WaitForSeconds (0.5f);
			}
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}