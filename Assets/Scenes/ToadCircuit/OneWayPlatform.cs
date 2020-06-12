using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Gameplay;
using SanicballCore;

public class OneWayPlatform : MonoBehaviour {

	public Collider removeCollider;

	void OnTriggerEnter(Collider other) {
		var ball = other.gameObject.GetComponent<Ball>();
		if(ball != null){
			removeCollider.enabled = false;
		}
	}

	void OnTriggerExit(Collider other) {
		var ball = other.gameObject.GetComponent<Ball>();
		if(ball != null){
			removeCollider.enabled = true;
		}
	}
}
