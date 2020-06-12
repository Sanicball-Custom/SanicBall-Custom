using Sanicball.Data;
using Sanicball.Gameplay;
using Sanicball.Logic;
using SanicballCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInLobby : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject spawnerGO = GameObject.Find("BallSpawner");
		if(spawnerGO != null){
			LobbyBallSpawner spawner = spawnerGO.GetComponent<LobbyBallSpawner>(); 
			if(spawner != null){
				spawner.SpawnBall(PlayerType.Normal, ControlType.Keyboard, 0, "TEST");
			}
		}
	}
}
