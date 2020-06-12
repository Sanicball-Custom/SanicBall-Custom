using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour {
	[SerializeField]
	public Vector3 NewPosition;
	private void OnTriggerEnter(Collider other) {
		other.transform.position = NewPosition;
		var bc = other.GetComponent<Ball>();
        if (bc != null) {
			bc.rb.angularVelocity = Vector3.zero;
			bc.rb.velocity = Vector3.zero;
		}
	}
}
