using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using UnityEngine;

[ExecuteInEditMode]
public class BoostRing : MonoBehaviour {
	public GameObject target;
	public float speedMultiplier = 100.0f;
	private Vector3 vector;
	
	private void Update() {
		if(target != null) {
			vector = (transform.position - target.transform.position);
			vector /= vector.magnitude;
			transform.rotation = Quaternion.FromToRotation(new Vector3(0,0,1), -vector);
		}
	}
	
	private void OnTriggerEnter(Collider other) {
		other.transform.position = transform.position;
		var bc = other.GetComponent<Ball>();
		if (bc != null) {
			bc.rb.angularVelocity = Vector3.zero;
			bc.rb.velocity = -vector*speedMultiplier;
			bc.rb.useGravity = false;
			bc.canMove = false;
		}
		var sound = GetComponent<AudioSource>();
		if(sound != null) {
			sound.Play();
		}
	}
	
	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, target.transform.position);
	}
}
