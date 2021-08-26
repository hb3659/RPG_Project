﻿using UnityEngine;
using System.Collections;

// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 						TargetMaker 2.0, Copyright © 2017, Ripcord Development
//												Spinner.cs
//										   info@ripcorddev.com
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

public class Spinner : MonoBehaviour {

	public float spinRate;

	void Update () {
		
		transform.Rotate(0.0f, (spinRate * Time.deltaTime * 1.2f), 0.0f);
	}
}