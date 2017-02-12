using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrameWork.Controller;

public class Slotmachine : MonoBehaviour {

	[SerializeField]
	List<Reelmachine> reels;
	// Use this for initialization
	void Start () {
		
	}

	public void Spin()
	{
		StartCoroutine (crt_Spin ());
	}

	IEnumerator crt_Spin()
	{
		if (null != reels) {
			for (int i = 0; i < reels.Count; i++) {
				reels [i].Roll ();
				yield return new WaitForSeconds (0.05f);
			}

			yield return new WaitForSeconds (2f);

			for (int i = 0; i < reels.Count; i++) {
				reels [i].ReserveStop ();
				yield return new WaitForSeconds (0.5f);
			}
		}
		yield return new WaitForSeconds (1f);


		SceneManager sceneManager = ControllerBase.getView<SceneManager> ();
		if (null != sceneManager) {
			sceneManager.BroadcastMessage ("showMoneyEffect");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
