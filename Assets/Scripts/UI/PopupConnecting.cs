using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class PopupConnecting : MonoBehaviour
    {
        [SerializeField]
        private Text titleField = null;
        [SerializeField]
        private Image spinner = null;

        public void ShowMessage(string text)
        {
            titleField.text = text;
            spinner.enabled = false;
        }

        public void ShowMessage(string text, bool spin)
        {
            titleField.text = text;
            spinner.enabled = spin;
        }

        public void Update(){
            spinner.GetComponent<RectTransform>().anchoredPosition = new Vector2(-titleField.preferredWidth+titleField.preferredWidth/2, spinner.GetComponent<RectTransform>().anchoredPosition.y);
        }
    }
}