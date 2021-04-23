using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Gameplay;
public enum TriggerType
{
    Default,
    BallDependant,
    Other
}
public class AchievementTrigger : MonoBehaviour
{
    public TriggerType triggerType;
    public string AchievementTitle;
    public string BallSpecialAchievementTitle;
    public int characterId;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /*void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Ball>() && triggerType == TriggerType.Default)
            Achievements.Unlock(AchievementTitle);
    }*/
}
