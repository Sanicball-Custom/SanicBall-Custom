using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Data;

public class RecordFixer : MonoBehaviour {
    void Start() {
        // Sync record tiers with character tiers as defined in ActiveData
        foreach(RaceRecord record in ActiveData.RaceRecords) {
            if(record.Tier != ActiveData.Characters[record.Character].tier) {
                record.SetTier(ActiveData.Characters[record.Character].tier);
            }
        }
    }
}
