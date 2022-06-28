using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SanicballCore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Sanicball.Data
{
    public class ActiveData : MonoBehaviour, ISerializationCallbackReceiver
    {
        #region Fields

        public List<RaceRecord> raceRecords = new List<RaceRecord>();

        //Pseudo-singleton pattern - this field accesses the current instance.
        private static ActiveData instance;

        //This data is saved to a json file
        private GameSettings gameSettings = new GameSettings();

        private KeybindCollection keybinds = new KeybindCollection();
        private MatchSettings matchSettings = MatchSettings.CreateDefault();

        //This data is set from the editor and remains constant
        [Header("Static data")]
        [SerializeField]
        private StageInfo[] stages;

        [SerializeField]
        private CharacterInfo[] characters;

        [SerializeField]
        private GameJoltInfo gameJoltInfo;

        [SerializeField]
        private GameObject christmasHat;
		[SerializeField]
        private GameObject halloweenHat;
		[SerializeField]
        private GameObject waluigiHat;
        [SerializeField]
        private Material eSportsTrail;
        [SerializeField]
        private GameObject eSportsHat;
        [SerializeField]
        private AudioClip eSportsMusic;
        [SerializeField]
        private ESportMode eSportsPrefab;

        [SerializeField]
        private CharacterDependantPlaylists characterSpecificMusic;

        //[SerializeField]
        //private Song[] ugandaMusic;
        //[SerializeField]
        //private Song[] shrekMusic;
        //[SerializeField]
        //private Song[] kirbyMusic;
        //[SerializeField]
        //private Song[] windowsMusic;
        //[SerializeField]
        //private Song[] khumkhumMusic;
        //[SerializeField]
        //private Song[] mattMusic;
        #endregion Fields

        public static CharacterInfo[] characterDataInEditor;

        #region Properties

        public static GameSettings GameSettings { get { return instance.gameSettings; } }
        public static KeybindCollection Keybinds { get { return instance.keybinds; } }
        public static MatchSettings MatchSettings { get { return instance.matchSettings; } set { instance.matchSettings = value; } }
        public static List<RaceRecord> RaceRecords { get { return instance.raceRecords; } }

        public static StageInfo[] Stages { get { return instance.stages; } }
        public static CharacterInfo[] Characters { get { return instance.characters; } }
        public static GameJoltInfo GameJoltInfo { get { return instance.gameJoltInfo; } }
        public static GameObject ChristmasHat { get { return instance.christmasHat; } }
        public static GameObject HalloweenHat { get { return instance.halloweenHat; } }
        public static GameObject WaluigiHat { get { return instance.waluigiHat; } }
        public static Material ESportsTrail {get{return instance.eSportsTrail;}}
        public static GameObject ESportsHat {get{return instance.eSportsHat;}}
        public static AudioClip ESportsMusic {get{return instance.eSportsMusic;}}
        public static ESportMode ESportsPrefab {get{return instance.eSportsPrefab;}}
        //public static Song[] UgandaMusic {get{return instance.ugandaMusic;}}
        //public static Song[] ShrekMusic {get{return instance.shrekMusic;}}
        //public static Song[] KirbyMusic {get{return instance.kirbyMusic;} }
        //public static Song[] WahndewsMusic { get { return instance.windowsMusic; } }
        //public static Song[] KhumKhumMusic { get { return instance.khumkhumMusic; } },
        //public static Song[] MattMusic { get { return instance.mattMusic; } }

        public static List<Song> songs = new List<Song>();
        public static CharacterDependantPlaylists CharacterSpecificMusic { get { return instance.characterSpecificMusic; } }

        public static bool ESportsFullyReady {
            get {
                bool possible = false;
                if (GameSettings.eSportsReady)
                {
                    Sanicball.Logic.MatchManager m = FindObjectOfType<Sanicball.Logic.MatchManager>();
                    if (m)
                    {
                        var players = m.Players;
                        foreach (var p in players) {
                            if (p.CtrlType != SanicballCore.ControlType.None) {
                                if (p.CharacterId == 13) 
                                {
                                    possible = true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                return possible;
            }
        }

        #endregion Properties

        #region Unity functions

        //Make sure there is never more than one GameData object
        [Obsolete]
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            string path = Path.Join(Application.dataPath, "Music");
            print(path); //here
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (string filename in Directory.GetFiles(path))
            {
                print(filename); //here
                if (!filename.EndsWith(".meta"))
                    StartCoroutine(AddAudioToPlaylist(filename));
            }


            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => {
                if (scene.buildIndex == 1) { // Menu scene
                    if (DateTime.Now.Month >= 6 && DateTime.Now.Month <= 7) {
                        SceneManager.LoadScene("Menu_Sonic1");
                    }
                }
            };
        }

        [Obsolete]
        private IEnumerator AddAudioToPlaylist(string filename)
        {
            WWW request = GetAudioFromFile(filename);
            yield return request;

            AudioClip audioClip = request.GetAudioClip();
            audioClip.name = "song";

            Song song = new Song();
            song.name = "song";
            song.clip = audioClip;

            songs.Add(song);
        }

        [Obsolete]
        private WWW GetAudioFromFile(string filepath)
        {
            WWW request = new WWW(filepath);
            return request;
        }

       

        private void OnEnable()
        {
            LoadAll();
            gameJoltInfo.Init();
        }

        private void OnApplicationQuit()
        {
            SaveAll();
        }

        #endregion Unity functions

        #region Saving and loading

        public void LoadAll()
        {
            Load("GameSettings.json", ref gameSettings);
            Load("GameKeybinds.json", ref keybinds);
            Load("MatchSettings.json", ref matchSettings);
            Load("Records.json", ref raceRecords);
        }

        public void SaveAll()
        {
            Save("GameSettings.json", gameSettings);
            Save("GameKeybinds.json", keybinds);
            Save("MatchSettings.json", matchSettings);
            Save("Records.json", raceRecords);
        }

        private void Load<T>(string filename, ref T output)
        {
            string fullPath = Application.persistentDataPath + "/" + filename;
            if (File.Exists(fullPath))
            {
                //Load file contents
                string dataString;
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    dataString = sr.ReadToEnd();
                }
                //Deserialize from JSON into a data object
                try
                {
                    var dataObj = JsonConvert.DeserializeObject<T>(dataString);
                    //Make sure an object was created, this would't end well with a null value
                    if (dataObj != null)
                    {
                        output = dataObj;
                        Debug.Log(filename + " loaded successfully.");
                    }
                    else
                    {
                        Debug.LogError("Failed to load " + filename + ": file is empty.");
                    }
                }
                catch (JsonException ex)
                {
                    Debug.LogError("Failed to parse " + filename + "! JSON converter info: " + ex.Message);
                }
            }
            else
            {
                Debug.Log(filename + " has not been loaded - file not found.");
            }
        }

        private void Save(string filename, object objToSave)
        {
            var data = JsonConvert.SerializeObject(objToSave);
            using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + filename))
            {
                sw.Write(data);
            }
            Debug.Log(filename + " saved successfully.");
        }

        #endregion Saving and loading

        public void OnBeforeSerialize() {
            OnAfterDeserialize();
        }
        public void OnAfterDeserialize() {
            characterDataInEditor = characters;
        }
    }
}
