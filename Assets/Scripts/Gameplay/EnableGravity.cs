using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using UnityEngine;

public class EnableGravity : MonoBehaviour {
	private void OnTriggerEnter(Collider other) {
		var bc = other.GetComponent<Ball>();
        if (bc != null) {
			bc.rb.useGravity = true;
		}
	}
}
