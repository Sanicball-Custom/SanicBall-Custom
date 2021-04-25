using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SanicballCore;
using SanicballCore.MatchMessages;
using UnityEngine;
using Sanicball.Data;

namespace Sanicball.Logic
{
    public class MatchPlayerEventArgs : EventArgs
    {
        public MatchPlayer Player { get; private set; }
        public bool IsLocal { get; private set; }

        public MatchPlayerEventArgs(MatchPlayer player, bool isLocal)
        {
            Player = player;
            IsLocal = isLocal;
        }
    }

    [Serializable]
    public class MatchPlayer
    {
        private Guid clientGuid;
        private ControlType ctrlType;
        private double latestMovementTimestamp = int.MinValue;

        public MatchPlayer(Guid clientGuid, ControlType ctrlType, int initialCharacterId)
        {
            this.clientGuid = clientGuid;
            this.ctrlType = ctrlType;
            CharacterId = initialCharacterId;
        }

        public Guid ClientGuid { get { return clientGuid; } }
        public ControlType CtrlType { get { return ctrlType; } }
        public int CharacterId { get; set; }
        public Gameplay.Ball BallObject { get; set; }
        public bool ReadyToRace { get; set; }

        public void ChangeMusic() {
            GameObject music = GameObject.Find("IngameMusic");
            if (music != null && ActiveData.GameSettings.characterMusic && ActiveData.GameSettings.music) {
                MusicPlayer player = (MusicPlayer)music.GetComponent<MusicPlayer>();
                var customPlaylist = ActiveData.CharacterSpecificMusic[CharacterId];
                if (customPlaylist != null) {
                    player.playlist = customPlaylist.playlist;
                    player.ShuffleSongs();
                    player.Next();
                }
            }
        }

        public void ProcessMovement(double timestamp, PlayerMovement movement)
        {
            if (timestamp > latestMovementTimestamp)
            {
                Rigidbody ballRb = BallObject.GetComponent<Rigidbody>();

                BallObject.transform.position = movement.Position;
                BallObject.transform.rotation = movement.Rotation;
                ballRb.velocity = movement.Velocity;
                ballRb.angularVelocity = movement.AngularVelocity;
                BallObject.DirectionVector = movement.DirectionVector;

                latestMovementTimestamp = timestamp;
            }
        }
		
		public static MatchPlayer GetByBall(Gameplay.Ball referenceBall) {
			object[] gameobjects = GameObject.FindSceneObjectsOfType(typeof(GameObject));
			foreach (object obj in gameobjects) {
				//Debug.Log("Testing if Object "+((GameObject) obj).name+" has MatchPlayer.");
				MatchPlayer player = ((GameObject) obj).GetComponent<MatchPlayer>();
				if(player != null) {
					//Debug.Log("Testing MatchPlayer with the referenceBall");
					if(player.BallObject == referenceBall){
						//Debug.Log("MatchPlayer matching referenceBall found. returning value");
						return player;
					}
				}
			}
			return null;
		}
    }
}