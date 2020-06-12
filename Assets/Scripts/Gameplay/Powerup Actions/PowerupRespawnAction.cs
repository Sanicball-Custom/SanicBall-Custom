using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using Sanicball.Logic;
using SanicballCore;
using SanicballCore.MatchMessages;
using UnityEngine;

public class PowerupRespawnAction : MonoBehaviour {

	public void Use(PowerupMessages data) {
		GameObject gameobj = data.input;
		var user = gameobj.GetComponent<Ball>();
		if(user != null){
			MatchManager manager = GameObject.FindObjectOfType(typeof(MatchManager)) as MatchManager;
			if(manager != null) {
				foreach(MatchPlayer player in manager.Players) {
					manager.messenger.SendMessage(new PlayerForcedRespawnMessage(player.ClientGuid, player.CtrlType));
				}
			}
			data.output = true;
			return;
		}
		data.output = false;
	}
}
