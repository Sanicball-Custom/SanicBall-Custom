using System;
using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using Sanicball.UI;
using Sanicball.Logic;
using UnityEngine;

public class StageModifier : MonoBehaviour {
	[System.Serializable]
	private class BallProperties : System.Object {
		public float size = 1;
		public float jumpHeight = 14;
		public float rollSpeedPercent = 1;
		public float airSpeedPercent = 1;
		public bool scaleTrailToMatch = true;
        public Mesh overrideMesh = null;
        public Mesh overrideCollisionMesh = null;
	}
	
	[System.Serializable]
	private class CameraProperties : System.Object {
		public bool zoomCamera = true;
		[Range(0,10)]
		public float zoomAmount = 4;
	}
	
	[System.Serializable]
	private class UserInterfaceProperties : System.Object {
		public string speedUnits = "";
		public string speedUnitsPlural = "";
		public float speedShownMult = 1.0f;
	}
	
	[SerializeField]
	private BallProperties ballProperties;
	[SerializeField]
	private CameraProperties cameraProperties;
	[SerializeField]
	private UserInterfaceProperties UIProperties;
	
	public void ModifyBall(GameObject gameobj) {
		var ball = gameobj.GetComponent<Ball>();
		if(ball != null){
			ball.gameObject.transform.localScale *= ballProperties.size;
			ball.characterStats.jumpHeight *= ballProperties.size;
			ball.characterStats.rollSpeed *= ballProperties.size;
			ball.characterStats.airSpeed *= ballProperties.size;
			if(cameraProperties.zoomCamera) {
				ball.prefabs.Camera.orbitHeight = ballProperties.size;
				ball.prefabs.Camera.orbitDistance = ballProperties.size*(10-cameraProperties.zoomAmount);
			}
			if(ballProperties.jumpHeight > 0) ball.characterStats.jumpHeight = ballProperties.jumpHeight;
			ball.characterStats.rollSpeed *= ballProperties.rollSpeedPercent;
			ball.characterStats.airSpeed *= ballProperties.airSpeedPercent;
			
			ball.speedFire.Init(ball);
			
			ball.GetComponent<SpeedTrail>().changeWithSize = ballProperties.scaleTrailToMatch;
			ball.GetComponent<SpeedTrail>().size = ballProperties.size;
			
			if (ballProperties.overrideMesh != null) {
                ball.GetComponent<MeshFilter>().mesh = ballProperties.overrideMesh;
            }
			
			if (ballProperties.overrideCollisionMesh != null) {
				if (ballProperties.overrideCollisionMesh.vertexCount <= 255) {
					Destroy(ball.GetComponent<Collider>());
					MeshCollider mc = ball.gameObject.AddComponent<MeshCollider>();
					mc.sharedMesh = ballProperties.overrideCollisionMesh;
					mc.convex = true;
				}else {
					Debug.LogWarning("[StageModifier] Vertex count for collision mesh is bigger than 255!");
				}
			}
		}
	}
	
	public void ModifyUI(GameObject gameobj) {
		var ui = gameobj.GetComponent<PlayerUI>();
		if(ui != null){
			if(UIProperties.speedUnits != "" && UIProperties.speedUnitsPlural != "") {
				ui.speedUnits = UIProperties.speedUnits;
				ui.speedUnitsPlural = UIProperties.speedUnitsPlural;
				ui.speedUnitsFontSize = 7*96/UIProperties.speedUnitsPlural.Length;
				ui.speedPercent = 1/ballProperties.rollSpeedPercent*UIProperties.speedShownMult;
			}
		}
	}
}
