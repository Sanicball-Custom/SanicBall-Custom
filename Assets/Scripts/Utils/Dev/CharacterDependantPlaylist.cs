using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball;
using Sanicball.Data;

[Serializable]
public class CharacterDependantPlaylist {
    [HideInInspector]
    public string name;
    public int characterId;
    public Song[] playlist;

    public void RefreshValues() {
        Sanicball.Data.CharacterInfo character = null;

        try {
            character = ActiveData.characterDataInEditor[characterId];
        } catch (IndexOutOfRangeException) { }
          catch (ArgumentOutOfRangeException) { }
          catch (NullReferenceException) { name = "Character ID #" + characterId; return; }

        if (character != null) name = character.name + " Playlist";
        else name = "UNKNOWN CHARACTER ID";
    }
}

[Serializable]
public class CharacterDependantPlaylists : ISerializationCallbackReceiver {
    [SerializeField]
    private CharacterDependantPlaylist[] list;
    public void RefreshListValues() {
        foreach (CharacterDependantPlaylist cdp in list) {
            cdp.RefreshValues();
        }
    }
    public CharacterDependantPlaylist this[int i] {
        get { return list.First(p => p.characterId == i); }
        set { list[list.ToList().FindIndex(p => p.characterId == i)] = value; }
    }

    public void OnBeforeSerialize() {
        RefreshListValues();
    }
    public void OnAfterDeserialize() {
        RefreshListValues();
    }
}
