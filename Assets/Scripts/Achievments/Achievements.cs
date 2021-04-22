using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sanicball.UI;

public static class Achievements //UI will be the singleton itslef
{
    public static string canvasName;
    
    public static Achievement[] achievements =
    {
        new Achievement("Now what?", "Wondered with Nemo what was next"),
        new Achievement("It's a trap!", "Fell into the trap"),
        new Achievement("THE PINK BOX", "No time to explain"),
        new Achievement("The real Sanic", "Found your realistic self"),
        new Achievement("The Dank Engine", "Too much dankness for a train"),
        new Achievement("Picnic time", "Found the original Dorito bag"),
        new Achievement("MISSING", "Last seen going fast near Green Hill Zone"),
        new Achievement("Yaranika?", "Bumped into a star"),
        new Achievement("Our Ogre Lord", "You found him"),
        new Achievement("I'm an ogre!", "Found the statue and disguised as him"),
        new Achievement("Reunion", "What are they doing in the middle fo the desert?"),
        new Achievement("Kakka carrot cake", "Chilling under t\the umbrella"),
        new Achievement("4chan", "Looks like you're going to the Shadow Realm, Jimbo"),
        new Achievement("Vineshroom", "Found the mushroom"),
        new Achievement("Hilarious Easter Egg", "Hilarous Easter Eggs"),
        new Achievement("On top of the mountain", "You climbed the tallest mountain")
    };
    
    public static bool Exists(string achievement)
    { 
        return Array.Exists(achievements, element => element.title == achievement);
    }
    
    public static bool Unlocked(string achievementName)
    {
        if (Exists(achievementName))
        {
            Achievement achievement = Array.Find(achievements, element => element.title == achievementName);
            return achievement.achieved;
        }
        else
        {
            Debug.LogError("Achievement: " + achievementName + " does not exist!");
            return false;
        }
    }
    
    public static void Unlock(string achievementName)
    {
        Debug.Log("Unlocking achievement " + achievementName);
        //Achievement achievement;
        if (Exists(achievementName))
        {
            Achievement achievement = Array.Find(achievements, element => element.title == achievementName);
            if (!achievement.achieved)
                ShowUnlockedAchievement(achievement);
            PlayerPrefs.SetInt(achievementName, 1);
            UpdateAchievements();
        }
        else
            Debug.LogError("Achievement: " + achievementName + " does not exist!");
    }

    public static void UpdateAchievements() // 0 means not achieved, 1 means achieved
    {
        foreach(Achievement achievement in achievements)
        {
            if (PlayerPrefs.HasKey(achievement.title))
                achievement.achieved = PlayerPrefs.GetInt(achievement.title) == 1;
            else
                PlayerPrefs.SetInt(achievement.title, 0);
        }
    }
    
    public static void ClearAchievements()
    {
        foreach(Achievement achievement in achievements)
        {
            Debug.Log("Clearing Achievement " + achievement.title);
            PlayerPrefs.SetInt(achievement.title, 0);
        }
        UpdateAchievements();
    }
    public static void ShowUnlockedAchievement(Achievement achievement)
    {
        Debug.Log("Just unlocked " + achievement.title + ": " + achievement.description);
        GameObject.FindObjectOfType<AchievementCanvas>().UnlockAchievement(achievement);
        //achievementUI = AssetD
    }
}