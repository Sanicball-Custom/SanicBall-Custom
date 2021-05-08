using SanicballCore;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField]
        private Text versionNameField = null;
        [SerializeField]
        private Text taglineField = null;

        private SlideCanvasGroup activePanel;

        public void SetActivePanel(SlideCanvasGroup panel)
        {
            if (activePanel == null)
            {
                //Opening new panel
                panel.Open();
                activePanel = panel;
            }
            else
            {
                //Changing panel
                if (activePanel != panel)
                {
                    CloseActivePanel(true);
                    panel.Open();
                    activePanel = panel;
                }
                else
                {
                    //Closing active panel
                    CloseActivePanel(true);
                }
            }
        }

        public void CloseActivePanel(bool closeSelf = false)
        {
            activePanel.Close(closeSelf);
        }

        private void Start()
        {
            versionNameField.text = GameVersion.AS_STRING;
            taglineField.text = GameVersion.TAGLINE;
        }

        private void Update()
        {
            if(activePanel != null && !activePanel.isOpen) activePanel = null;
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) && FindObjectsOfType<Popup>().Length <= 0)
            {
                if (activePanel != null)
                {
                    CloseActivePanel();
                }
                else
                {
                    Application.Quit();
                }
            }
        }
    }
}