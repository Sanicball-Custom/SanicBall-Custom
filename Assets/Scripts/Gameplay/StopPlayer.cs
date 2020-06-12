using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using UnityEngine;

public class StopPlayer : MonoBehaviour {
	public bool gradual = false;
	public float angularDecrementPercent = 5.0f;
	public float velocityDecrementPercent = 10.0f;
	private void OnTriggerEnter(Collider other) {
		var bc = other.GetComponent<Ball>();
        if (bc != null && !gradual) {
			bc.rb.angularVelocity = Vector3.zero;
			bc.rb.velocity = Vector3.zero;
		}
	}

	private void OnTriggerStay(Collider other) {
		var bc = other.GetComponent<Ball>();
        if (bc != null && gradual){
			if(bc.rb.angularVelocity.magnitude > 25) bc.rb.angularVelocity *= 1-angularDecrementPercent/100.0f * Time.deltaTime;
			if(bc.rb.velocity.magnitude > 50) bc.rb.velocity *= 1-velocityDecrementPercent/100.0f * Time.deltaTime;
		}
	}
}
