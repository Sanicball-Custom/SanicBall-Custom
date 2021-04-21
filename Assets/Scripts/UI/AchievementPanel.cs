using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPanel : MonoBehaviour {

    public GameObject achievementEntryPrefab;
    public Transform unlockedAchievements;
    public Transform lockedAchievements;

    void Start() {}

    void Update() {}
    public void RefreshAchievements() {
        Achievements.UpdateAchievements();
        foreach(Transform child in unlockedAchievements) {
            Destroy(child);
        }
        foreach (Transform child in lockedAchievements) {
            Destroy(child);
        }
        foreach (Achievement achievement in Achievements.achievements) {
            GameObject entry;
            if (Achievements.Unlocked(achievement.title)) entry = Instantiate(achievementEntryPrefab, unlockedAchievements);
            else entry = Instantiate(achievementEntryPrefab, lockedAchievements);
            entry.transform.GetChild(0).GetComponent<Text>().text = achievement.title;
            entry.transform.GetChild(1).GetComponent<Text>().text = achievement.description;
        }
    }
}
