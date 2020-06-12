using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Discord;

public class DiscordController : MonoBehaviour {
	
	[System.NonSerialized]
	public Discord.ActivityManager activityManager;
	public Discord.Discord discord;
	
	void Awake(){
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Use this for initialization
	void Start () {
		discord = new Discord.Discord(691584667219918911, (UInt64)Discord.CreateFlags.Default);
		activityManager = discord.GetActivityManager();
		Sanicball.Extra.DiscordUtils.RemoveActivityIcons();
	}
	
	// Update is called once per frame
	void Update () {
		discord.RunCallbacks();
	}
}
