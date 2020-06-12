using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using UnityEngine;

public class EnableMovement : MonoBehaviour {
	private void OnTriggerEnter(Collider other) {
		var bc = other.GetComponent<Ball>();
        if (bc != null) {
			bc.canMove = true;
			bc.rb.useGravity = true;
		}
	}
}
