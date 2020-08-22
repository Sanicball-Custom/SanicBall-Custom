using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sanicball.Extra {
	public static class DiscordUtils {

		public static void RemoveActivityIcons(){
			//Execute the Discord Activity Update
			GameObject discordGO = GameObject.Find("DiscordController");
			if(discordGO != null && discordGO.GetComponent<DiscordController>()){
				DiscordController discord = (DiscordController)discordGO.GetComponent(typeof(DiscordController));
				var sceneIndex = SceneManager.GetActiveScene().buildIndex;
				var stage = "";
				if(sceneIndex == 1){
					stage = "Main Menu";
				}else if(sceneIndex == 2){
					stage = "Lobby";
				}else if(sceneIndex == 3){
					stage = "Green Hill Zone";
				}else if(sceneIndex == 4){
					stage = "Flame Core";
				}else if(sceneIndex == 5){
					stage = "Dusty Desert";
				}else if(sceneIndex == 6){
					stage = "Rainbow Road";
				}else if(sceneIndex == 7){
					stage = "Ice Mountain";
				}else if(sceneIndex == 8){
					stage = "Minecraft";
				}else if(sceneIndex == 9){
					stage = "Testing Track";
				}else if(sceneIndex == 10){
					stage = "Luigi's Circuit";
				}else if(sceneIndex == 11){
					stage = "Waluigi's Pinball";
				}else if(sceneIndex == 12){
					stage = "Waluigi's Stadium";
				}else if(sceneIndex == 13){
					stage = "Volcano Valley";
				}else if(sceneIndex == 14){
					stage = "Peach's Gardens";
				}else if(sceneIndex == 15){
					stage = "Volcano Circuit";
				}else if(sceneIndex == 16){
					stage = "Toad Circuit";
				}else if(sceneIndex == 17){
					stage = "Bowser's Castle";
				}
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
				discord.activityManager.UpdateActivity(activity, (result) => {
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
			GameObject discordGO = GameObject.Find("DiscordController");
			if(discordGO != null && discordGO.GetComponent<DiscordController>()){
				DiscordController discord = (DiscordController)discordGO.GetComponent(typeof(DiscordController));
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
				discord.activityManager.UpdateActivity(activity, (result) => {
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
			GameObject discordGO = GameObject.Find("DiscordController");
			if(discordGO != null && discordGO.GetComponent<DiscordController>()){
				DiscordController discord = (DiscordController)discordGO.GetComponent(typeof(DiscordController));
				var sceneIndex = SceneManager.GetActiveScene().buildIndex;
				var stage = "";
				if(sceneIndex == 1){
					stage = "Main Menu";
				}else if(sceneIndex == 2){
					stage = "Lobby";
				}else if(sceneIndex == 3){
					stage = "Green Hill Zone";
				}else if(sceneIndex == 4){
					stage = "Flame Core";
				}else if(sceneIndex == 5){
					stage = "Dusty Desert";
				}else if(sceneIndex == 6){
					stage = "Rainbow Road";
				}else if(sceneIndex == 7){
					stage = "Ice Mountain";
				}else if(sceneIndex == 8){
					stage = "Minecraft";
				}else if(sceneIndex == 9){
					stage = "Testing Track";
				}else if(sceneIndex == 10){
					stage = "Luigi Circuit";
				}else if(sceneIndex == 11){
					stage = "Waluigi Pinball";
				}else if(sceneIndex == 12){
					stage = "Waluigi's Stadium";
				}else if(sceneIndex == 13){
					stage = "Volcano Valley";
				}else if(sceneIndex == 14){
					stage = "Peach's Gardens";
				}else if(sceneIndex == 15){
					stage = "Volcano Circuit";
				}else if(sceneIndex == 16){
					stage = "Toad Circuit";
				}else if(sceneIndex == 17){
					stage = "Bowser's Castle";
				}
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