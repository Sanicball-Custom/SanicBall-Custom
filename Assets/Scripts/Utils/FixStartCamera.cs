using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Gameplay;

public class FixStartCamera : MonoBehaviour {

	public Vector3 rotation = new Vector3(0,180,0);
	private bool done = false;
	// Use this for initialization
	void Update () {
		if(!done){
			OmniCamera[] cameras = Object.FindObjectsOfType<OmniCamera>();
			if(cameras != null){
				Quaternion dir = Quaternion.Euler(rotation);
				foreach(OmniCamera cam in cameras){
					cam.SetDirection(dir);
				}
			}
			done = true;
		}
	}
}
