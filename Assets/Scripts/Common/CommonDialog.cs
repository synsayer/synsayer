﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonDialog : FrameWork.View.DialogBase {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	override public void onClickBackButton()
	{
		throw new UnityException ();
	}
}
