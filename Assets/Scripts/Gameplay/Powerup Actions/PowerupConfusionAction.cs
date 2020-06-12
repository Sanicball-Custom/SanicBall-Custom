using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using Sanicball.Logic;
using SanicballCore;
using SanicballCore.MatchMessages;
using UnityEngine;

public class PowerupConfusionAction : MonoBehaviour {

	public void Use(PowerupMessages data) {
		GameObject gameobj = data.input;
		var user = gameobj.GetComponent<Ball>();
		if(user != null){
			// Send message to random user to make it confused
			MatchManager manager = GameObject.FindObjectOfType(typeof(MatchManager)) as MatchManager;
			if(manager != null) {
				MatchPlayer actualPlayer = null;
				foreach(MatchPlayer player in manager.Players) {
					if(player.BallObject == user) {
						actualPlayer = player;
						break;
					}
				}

				if(actualPlayer != null) {
					foreach(MatchPlayer player in manager.Players) {
						float chance = Random.value;
						if(player != actualPlayer) {
							manager.messenger.SendMessage(new PlayerConfusedMessage(player.ClientGuid, player.CtrlType));
						}
					}
				}
			}
			data.output = true;
			return;
		}
		data.output = false;
	}
}
