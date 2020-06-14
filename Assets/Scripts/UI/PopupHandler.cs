using UnityEngine;

namespace Sanicball.UI
{
    public class PopupHandler : MonoBehaviour
    {
        public CanvasGroup groupDisabledOnPopup;
        public Transform targetParent;
        private Popup activePopup;
        private Popup secondaryPopup;

        public void OpenPopup(Popup popupPrefab)
        {
            if (activePopup != null)
            {
                //Closing old popup
                activePopup.Close();
                if(secondaryPopup) secondaryPopup.Close();
                groupDisabledOnPopup.interactable = true;
            }
            //Opening new popup
            activePopup = Instantiate(popupPrefab);
            activePopup.transform.SetParent(targetParent, false);
            activePopup.onClose += () =>
            {
                groupDisabledOnPopup.interactable = true;
            };
            groupDisabledOnPopup.interactable = false;
        }

        public void OpenSecondaryPopup(Popup popupPrefab)
        {
            if (secondaryPopup != null)
            {
                //Closing old popup
                secondaryPopup.Close();
                groupDisabledOnPopup.interactable = true;
            }
            //Opening new popup
            secondaryPopup = Instantiate(popupPrefab);
            secondaryPopup.transform.SetParent(targetParent, false);
            secondaryPopup.onClose += () =>
            {
                groupDisabledOnPopup.interactable = true;
            };
            groupDisabledOnPopup.interactable = false;
        }

        public void CloseActivePopup()
        {
            activePopup.Close();
            activePopup = null;
            groupDisabledOnPopup.interactable = true;
        }

        public void CloseSecondaryPopup()
        {
            secondaryPopup.Close();
            secondaryPopup = null;
            groupDisabledOnPopup.interactable = true;
        }

        private void Start()
        {
        }

        private void Update()
        {
        }
    }
}