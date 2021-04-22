using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class AchievementCanvas : MonoBehaviour
    {
        public CanvasGroup panel;
        public Text label;

        float showTimer;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (GameInput.CallAchievementPopup())
                UnlockAchievement(new Achievement("Achievement Test", "placeholder desciption"));

            if (showTimer > 0)
            {
                panel.alpha = Mathf.Lerp(panel.alpha, 1, Time.deltaTime * 20);
                showTimer -= Time.deltaTime;
            }
            else
            {
                panel.alpha = Mathf.Lerp(panel.alpha, 0, Time.deltaTime * 20);
            }

        }

        public void UnlockAchievement(Achievement achievement)
        {
            Debug.Log("Achievement Popup!");

            showTimer = 5;
            label.text = achievement.title;
        }
    }
}
