using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.UI;
using Sanicball.Logic;
using System.Net.Sockets;

public class LocalServerConnect : MonoBehaviour {
	
	[SerializeField]
	private PopupHandler popupHandler = null;
	
	public void Start(){
		popupHandler = FindObjectOfType<PopupHandler>();
	}

	public void Connect(string ip, int port){
		while(!Ping("127.0.0.1",port)){
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				popupHandler.CloseActivePopup();
				break;
			}
		}

		MatchStarter matchStarter = FindObjectOfType<MatchStarter>();
		matchStarter.JoinOnlineGame("127.0.0.1", port);
		Destroy(gameObject);
	}
	
	private bool Ping(string ip, int port){
		try{
			using(var client = new UdpClient()){
				client.Connect(ip,port);
				Debug.Log("[PopupCreateServer.cs -> Ping] "+ip+":"+port+" succeeded.");
				return true;
			}
		}catch(SocketException e){
			return false;
		}
	}
}
