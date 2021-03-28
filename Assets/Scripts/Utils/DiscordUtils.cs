using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sanicball.Extra {
	public static class DiscordUtils {

		public static string GetStageName(int sceneIndex) {
			if (sceneIndex == 1) {
				return "Main Menu";
			} else if (sceneIndex == 2) {
				return "Lobby";
			} else if (sceneIndex == 3) {
				return "Green Hill Zone";
			} else if (sceneIndex == 4) {
				return "Flame Core";
			} else if (sceneIndex == 5) {
				return "Dusty Desert";
			} else if (sceneIndex == 6) {
				return "Rainbow Road";
			} else if (sceneIndex == 7) {
				return "Ice Mountain";
			} else if (sceneIndex == 8) {
				return "Luigi's Circuit";
			} else if (sceneIndex == 9) {
				return "Waluigi's Pinball";
			} else if (sceneIndex == 10) {
				return "Waluigi's Stadium";
			} else if (sceneIndex == 11) {
				return "Volcano Valley";
			} else if (sceneIndex == 12) {
				return "Peach's Gardens";
			} else if (sceneIndex == 13) {
				return "Volcano Circuit";
			} else if (sceneIndex == 14) {
				return "Toad Circuit";
			} else if (sceneIndex == 15) {
				return "Bowser's Castle";
			} else if (sceneIndex == 16) {
				return "Mario's Course";
			} else if (sceneIndex == 17) {
				return "Wario's Stadium";
			} else if (sceneIndex == 18) {
				return "Mario's Track";
			} else if (sceneIndex == 19) {
				return "Rainbow Road 2";
			} else if (sceneIndex == 20) {
				return "Sanic Loops";
			} else if (sceneIndex == 21) {
				return "Old Green Hill Zone";
			}
			return "Unknown Stage";
		}

		public static void RemoveActivityIcons(){
			//Execute the Discord Activity Update
			GameObject controllerGO = GameObject.Find("DiscordController");
			if (controllerGO == null) return;
			DiscordController discordGO = GameObject.Find("DiscordController").GetComponent<DiscordController>();
			
			if(discordGO && discordGO.discord != null){
				var sceneIndex = SceneManager.GetActiveScene().buildIndex;
				var stage = GetStageName(sceneIndex);
				
				var activity = new Discord.Activity
				{
					State = "In "+stage,
					Details = "No Character",
					Timestamps = {
						Start = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds,
					},
					Assets = {
						LargeImage = "appicon",
						LargeText = "Sanicball",
					},
					Instance = true,
				};
				discordGO.activityManager.UpdateActivity(activity, (result) => {
					if (result == Discord.Result.Ok){
						Console.WriteLine("DISCORD ACTIVITY: Update! Current Stage: "+sceneIndex+" / "+stage);
					}else{
						Console.WriteLine("DISCORD ACTIVITY: Failed to update");
					}
				});
			}
		}

		public static void GenericActivity(string state, string details){
			//Execute the Discord Activity Update
			GameObject controllerGO = GameObject.Find("DiscordController");
			if (controllerGO == null) return;
			DiscordController discordGO = controllerGO.GetComponent<DiscordController>();

			if (discordGO && discordGO.discord != null)
			{
				var activity = new Discord.Activity
				{
					State = state,
					Details = details,
					Timestamps = {
						Start = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds,
					},
					Assets = {
						LargeImage = "appicon",
						LargeText = "Sanicball",
					},
					Instance = true,
				};
				discordGO.activityManager.UpdateActivity(activity, (result) => {
					if (result == Discord.Result.Ok){
						Console.WriteLine("DISCORD ACTIVITY: Update!");
					}else{
						Console.WriteLine("DISCORD ACTIVITY: Failed to update");
					}
				});
			}
		}

		public static void UpdateActivity(string characterName){
			//Execute the Discord Activity Update
			GameObject controllerGO = GameObject.Find("DiscordController");
			if (controllerGO == null) return;
			DiscordController discordGO = GameObject.Find("DiscordController").GetComponent<DiscordController>();

			if (discordGO && discordGO.discord != null)
			{
				DiscordController discord = (DiscordController)discordGO.GetComponent(typeof(DiscordController));
				var sceneIndex = SceneManager.GetActiveScene().buildIndex;
				var stage = GetStageName(sceneIndex);
				var activity = new Discord.Activity
				{
					State = "In "+stage,
					Timestamps = {
						Start = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds,
					},
					Assets = {
						LargeImage = characterName.ToLower().Replace(" ","").Replace(".","_") + "icon",
						LargeText = characterName,
						SmallImage = "appicon",
						SmallText = "Sanicball",
					},
					Instance = true,
				};
				discord.activityManager.UpdateActivity(activity, (result) => {
					if (result == Discord.Result.Ok){
						Console.WriteLine("DISCORD ACTIVITY: Update! Current Stage: "+sceneIndex+" / "+stage);
					}else{
						Console.WriteLine("DISCORD ACTIVITY: Failed to update");
					}
				});
			}
		}
	}
}