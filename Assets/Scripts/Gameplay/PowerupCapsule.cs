using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using Sanicball.Logic;
using UnityEngine;

public class PowerupCapsule : MonoBehaviour {
	private float pickedUpTimer = 0;
	private float reappearDelay = 5.0f;
	private bool pickedUp = false;
	private Component[] childMeshRenderers;
	private Collider capsuleCollider;
	[SerializeField]
	private LayerMask placementLayers;
	
	private void Start() {
		var manager = FindObjectOfType<MatchManager>();
		if (manager && !manager.CurrentSettings.PowerupsEnabled) gameObject.SetActive(false);

		childMeshRenderers = GetComponentsInChildren(typeof(SkinnedMeshRenderer)); // NULL
		capsuleCollider = GetComponent(typeof(Collider)) as Collider;
		//Debug.Log(childMeshRenderers);

		PosRot placement = CalcTargetPlacement();
		transform.position = placement.Position+new Vector3(0,3.5f,0);
		transform.rotation = placement.Rotation;
	}
	
	private void OnTriggerEnter(Collider other) {
		var sound = GetComponent<AudioSource>();
		if(sound != null) {
			sound.Play();
		}
		var availablePowerups = GameObject.Find("Powerups").GetComponentsInChildren<Powerup>();
		var bc = other.GetComponent<Ball>();
		int powerupIndex = Random.Range(0,availablePowerups.Length);
		if (bc != null) {
			if(!bc.powerups[0]) {
				bc.powerups[0] = availablePowerups[powerupIndex];
				bc.changeUIPowerups = true;
			}else if(!bc.powerups[1]) {
				bc.powerups[1] = availablePowerups[powerupIndex];
				bc.changeUIPowerups = true;
			}
			SetState(false);
		}
	}
	
	private void Update() {
		if(pickedUp){
			pickedUpTimer += Time.deltaTime;
			if(pickedUpTimer > reappearDelay) {
				SetState(true);
				
				pickedUpTimer = 0;
			}
		}
	}
	
	private void SetState(bool state){
		if(childMeshRenderers != null && capsuleCollider != null) {
			foreach(Component c in childMeshRenderers) {
				SkinnedMeshRenderer r = (SkinnedMeshRenderer) c;
				r.enabled = state;
			}
			
			capsuleCollider.enabled = state;
			pickedUp = !state;
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		PosRot placement = CalcTargetPlacement();

		Gizmos.DrawLine(transform.position, placement.Position);

		Gizmos.matrix = Matrix4x4.TRS(placement.Position, placement.Rotation, Vector3.one);
		Gizmos.DrawSphere(Vector3.zero, transform.localScale.x/2);
		Gizmos.matrix = Matrix4x4.identity;
	}

	//Copied from the other stage bonuses like BoostPad
	private PosRot CalcTargetPlacement() {
		Ray ray = new Ray(transform.position, transform.rotation * Vector3.down);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, placementLayers.value))
		{
			PosRot placement = new PosRot();
			placement.Position = hit.point;

			Quaternion alongNormal = Quaternion.FromToRotation(Vector3.up, hit.normal);
			float angle = transform.rotation.eulerAngles.y;
			placement.Rotation = Quaternion.AngleAxis(angle, hit.normal) * alongNormal;

			return placement;
		}
		return new PosRot(transform.position, transform.rotation);
	}
}