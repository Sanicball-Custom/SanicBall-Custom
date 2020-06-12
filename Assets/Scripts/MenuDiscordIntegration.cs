using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDiscordIntegration : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Sanicball.Extra.DiscordUtils.GenericActivity("Main Menu", "");
	}
}
