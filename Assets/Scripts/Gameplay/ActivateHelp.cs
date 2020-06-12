using System;
using System.Collections;
using System.Collections.Generic;
using Sanicball.Logic;
using Sanicball.Gameplay;
using UnityEngine;

public class ActivateHelp : MonoBehaviour {
	[SerializeField]
	public int helpTimeMinutes = 1;
	public int helpTimeSeconds = 0;
	[SerializeField]
	public GameObject[] objectsToActivate = {};
	
	private RaceManager manager;
	private bool done = false;
	private int objectsActivated = 0;
	
	void OnValidate() {
		if(helpTimeSeconds >= 60){
			var newMins = (int)Math.Floor(((double)helpTimeSeconds)/60d);
			var newSeconds = (int)Math.Floor((((double)helpTimeSeconds)/60d - newMins) * 60);
			helpTimeMinutes = newMins;
			helpTimeSeconds = newSeconds;
		}
	}
	
	void Start() {
		manager = GameObject.FindObjectOfType(typeof(RaceManager)) as RaceManager;
	}
	
	void Update() {
		if(manager != null) {
			if(manager.raceTimer >= helpTimeMinutes*60 + helpTimeSeconds && !done) {
				object[] gameobjects = GameObject.FindObjectsOfTypeAll(typeof(GameObject));
				foreach (object obj in gameobjects) {
					GameObject gameobject = (GameObject) obj;
					foreach (GameObject toActivate in objectsToActivate) {
						if(gameobject == toActivate && !gameobject.activeSelf) {
							gameobject.SetActive(true);
							objectsActivated++;
							break;
						}
					}
				}
			}
		}else {
			manager = GameObject.FindObjectOfType(typeof(RaceManager)) as RaceManager;
		}
		done = objectsActivated == objectsToActivate.Length;
	}
}
