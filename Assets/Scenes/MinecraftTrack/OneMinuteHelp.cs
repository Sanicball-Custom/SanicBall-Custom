using System;
using System.Collections;
using System.Collections.Generic;
using Sanicball.Logic;
using Sanicball.Gameplay;
using UnityEngine;

public class OneMinuteHelp : MonoBehaviour {
	private RaceManager manager;
	
	void Start() {
		manager = GameObject.FindObjectOfType(typeof(RaceManager)) as RaceManager;
	}
	
	void Update() {
		if(manager != null) {
			if(manager.RaceTime.TotalSeconds >= 5) {
				object[] gameobjects = GameObject.FindSceneObjectsOfType(typeof(GameObject));
				foreach (object obj in gameobjects) {
					GameObject gameobject = (GameObject) obj;
					if(gameobject.name == "1min Help") {
						gameobject.SetActive(true);
						break;
					}
				}
			}
		}else {
			manager = GameObject.FindObjectOfType(typeof(RaceManager)) as RaceManager;
		}
	}
}
