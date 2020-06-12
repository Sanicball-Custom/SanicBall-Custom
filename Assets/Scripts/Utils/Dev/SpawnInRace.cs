using Sanicball.Data;
using Sanicball.Gameplay;
using Sanicball.Logic;
using SanicballCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInRace : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject spawnerGO = GameObject.Find("Spawnpoint");
		if(spawnerGO != null){
			RaceBallSpawner spawner = spawnerGO.GetComponent<RaceBallSpawner>(); 
			if(spawner != null){
				spawner.SpawnBall(0, BallType.Player, ControlType.Keyboard, 0, "TEST");
			}
		}
	}
}
