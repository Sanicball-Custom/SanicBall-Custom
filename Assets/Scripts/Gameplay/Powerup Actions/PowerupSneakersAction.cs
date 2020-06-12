using System.Collections;
using System.Collections.Generic;
using Sanicball.Gameplay;
using UnityEngine;

public class PowerupSneakersAction : MonoBehaviour {
	
	public void Use(PowerupMessages data) {
		GameObject gameobj = data.input;
		var user = gameobj.GetComponent<Ball>();
		if(user != null){
			user.rb.velocity *= 2.5f;
			data.output = true;
			return;
		}
		data.output = false;
	}
}
