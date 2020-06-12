using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using Sanicball.Logic;
using SanicballCore;
using SanicballCore.MatchMessages;
using UnityEngine;

public class PowerupLightningBoltAction : MonoBehaviour {
	
	public void Use(PowerupMessages data) {
		GameObject gameobj = data.input;
		var user = gameobj.GetComponent<Ball>();
		if(user != null){
			//Update other users' view
			MatchManager manager = GameObject.FindObjectOfType(typeof(MatchManager)) as MatchManager;
			if(manager != null) {
				Debug.Log("MATCH MANAGER(manager) FOUND!");
				MatchPlayer actualPlayer = null;
				foreach(MatchPlayer player in manager.Players) {
					if(player.BallObject == user) {
						actualPlayer = player;
						break;
					}
				}
				if(actualPlayer != null) {
					Debug.Log("ACTUAL PLAYER(actualPlayer) FOUND!");
					foreach(MatchPlayer player in manager.Players) {
						if(player != actualPlayer) {
							Debug.Log("PLAYERS ARE DIFFERERNT. ZAPPING OTHER PLAYER(player)!");
							manager.messenger.SendMessage(new PlayerZappedMessage(player.ClientGuid, player.CtrlType));
						}
					}
				}
			}
			
			//Update user's view
			object[] gameobjects = GameObject.FindSceneObjectsOfType(typeof(GameObject));
			foreach (object obj in gameobjects) {
				GameObject gameobject = (GameObject) obj;
				Ball opponent = gameobject.GetComponent<Ball>();
				if(opponent != null){
					if(opponent != user) {
						opponent.zapped = true;
					}
				}
			}
			data.output = true;
			return;
		}
		data.output = false;
	}
}
