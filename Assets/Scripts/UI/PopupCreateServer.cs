using Sanicball.Logic;
using Sanicball.Data;
using UnityEngine;
using UnityEngine.UI;
using SanicballCore.Server;
using System;
using System.Threading;
using System.Collections;

namespace Sanicball.UI
{
    public class PopupCreateServer : MonoBehaviour
    {
        [SerializeField]
        private InputField maxPlayersInput;
        [SerializeField]
        private InputField nameInput;
        [SerializeField]
        private InputField portInput;
        [SerializeField]
        private Toggle isPublicInput;
        [SerializeField]
        private Text portOutput;
        [SerializeField]
        private UI.Popup connectingPopupPrefab = null;
        [SerializeField]
        private GameObject serverStarterPrefab = null;
        [SerializeField]
        private GameObject serverConnectorPrefab = null;
        [SerializeField]
        private UI.PopupHandler popupHandler = null;
        
        public void Start(){
            popupHandler = FindObjectOfType<UI.PopupHandler>();
        }

        public void Create(){
            StartCoroutine(PopupParser());
        }

        public IEnumerator PopupParser()
        {
            int maxPlayers;
            int.TryParse(maxPlayersInput.text, out maxPlayers);
            int port;
            int.TryParse(portInput.text, out port);
            portOutput.text = "";
            if(port < 1024) {
                portOutput.text = "Port must be at least 1024.";
                yield break;
            }else if(port > 49151) {
                portOutput.text = "Port must be at most 49151.";
                yield break;
            }
            
            var serverStarter = Instantiate(serverStarterPrefab);
            DontDestroyOnLoad(serverStarter);
            serverStarter.GetComponent<LocalServerStarter>().InitServer(port, maxPlayers, nameInput.text, isPublicInput.isOn, ActiveData.GameSettings.serverListURL);

            if(popupHandler != null){
                popupHandler.OpenSecondaryPopup(connectingPopupPrefab);
                FindObjectOfType<UI.PopupConnecting>().ShowMessage("   Creating Server...", true);
            }

            var serverConnector = Instantiate(serverConnectorPrefab);
            serverConnector.GetComponent<LocalServerConnect>().Connect("127.0.0.1", port);
        }
    }
}

