using System.Linq;
using System.Collections.Generic;
using Sanicball.Data;
using SanicballCore;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    // State 1, 2, 3 and 4 are named like that to be generic to any condition
    enum RecordConditionState {
        NotSet,
        State1,
        State2,
        State3,
        State4
    }
    public class RecordsPanel : MonoBehaviour
    {
        public Text stageNameField;
        public Text individualCharactersField;
        public Text hadPowerupsField;

        public Transform tierRecords;
        public Transform characterRecords;

        public RecordTypeControl lapRecord;
        public RecordTypeControl hyperspeedLapRecord;

		public RecordTypeControl recordTypeControlPrefab;
		public RectTransform sectionHeaderPrefab;
		public RectTransform recordTypeContainer; 

		private List<RecordTypeControl> recordTypesByTier = new List<RecordTypeControl> ();
        private List<RecordTypeControl> recordTypesByCharacter = new List<RecordTypeControl>();


        private int selectedStage = 0;
        private bool individualCharacters = false;
        private RecordConditionState powerupCondition = RecordConditionState.NotSet; // 1 = No; 2 = Yes

        public void IndividualCharactersToggle() {
            individualCharacters = !individualCharacters;
            UpdateIndividualCharacters();
        }

        public void IncrementPowerupsCondition() {
            powerupCondition = (RecordConditionState)Mathf.Min((int)powerupCondition + 1, (int)RecordConditionState.State2);
            UpdateConditions();
        }

        public void DecrementPowerupsCondition() {
            powerupCondition = (RecordConditionState)Mathf.Max((int)powerupCondition - 1, (int)RecordConditionState.NotSet);
            UpdateConditions();
        }

        public void IncrementStage()
        {
            selectedStage++;
            if (selectedStage >= ActiveData.Stages.Length)
            {
                selectedStage = 0;
            }
            UpdateStageName();
        }

        public void DecrementStage()
        {
            selectedStage--;
            if (selectedStage < 0)
            {
                selectedStage = ActiveData.Stages.Length - 1;
            }
            UpdateStageName();
        }

        private void Start()
        {
			foreach (CharacterTier tier in System.Enum.GetValues (typeof(CharacterTier)).Cast<CharacterTier>())
			{
				RectTransform tierHeader = Instantiate(sectionHeaderPrefab);
				tierHeader.GetComponentInChildren<Text>().text = tier.ToString () + " balls"; //yes
				tierHeader.SetParent (tierRecords, false);

                //RectTransform characterHeader = Instantiate(sectionHeaderPrefab);
                //characterHeader.GetComponentInChildren<Text>().text = tier.ToString() + " balls"; //yes
                //characterHeader.SetParent(characterRecords, false);

                RecordTypeControl ctrl = Instantiate (recordTypeControlPrefab);
				ctrl.transform.SetParent (tierRecords, false);
				ctrl.titleField.text = "Lap record";
				recordTypesByTier.Add(ctrl);
            }

            foreach (Sanicball.Data.CharacterInfo charinfo in ActiveData.Characters) {
                RecordTypeControl charCtrl = Instantiate(recordTypeControlPrefab);
                charCtrl.transform.SetParent(characterRecords, false);
                charCtrl.titleField.text = charinfo.name + " lap record";
                recordTypesByCharacter.Add(charCtrl);
            }

            UpdateFields();
            UpdateStageName();
            UpdateIndividualCharacters();
            UpdateConditions();
        }

        private void UpdateFields()
        {
            var stageindex = ActiveData.Stages[selectedStage].id;
            var records = ActiveData.RaceRecords.Where(a => a.Stage == stageindex && a.GameVersion == GameVersion.AS_FLOAT && a.WasTesting == GameVersion.IS_TESTING).OrderBy(a => a.Time);

            if (!individualCharacters) {
                tierRecords.gameObject.SetActive(true);
                characterRecords.gameObject.SetActive(false);
                for (int i = 0; i < recordTypesByTier.Count(); i++) {
                    var ctrl = recordTypesByTier[i];
                    var bestLapRecord = records.Where(a => a.Tier == (CharacterTier)i).Where(FilterRecords).FirstOrDefault();
                    ctrl.SetRecord(bestLapRecord);
                }
            } else {
                tierRecords.gameObject.SetActive(false);
                characterRecords.gameObject.SetActive(true);
                for (int i = 0; i < recordTypesByCharacter.Count(); i++) {
                    var ctrl = recordTypesByCharacter[i];
                    var bestLapRecord = records.Where(a => a.Character == i).Where(FilterRecords).FirstOrDefault();
                    ctrl.gameObject.SetActive(bestLapRecord != null);
                    ctrl.SetRecord(bestLapRecord);
                }
            }

            //            var bestLapRecord = records.Where(a => a.Type == RecordType.Lap).FirstOrDefault();
            //            lapRecord.SetRecord(bestLapRecord);
            //
            //            var bestHyperspeedLapRecord = records.Where(a => a.Type == RecordType.HyperspeedLap).FirstOrDefault();
            //            hyperspeedLapRecord.SetRecord(bestHyperspeedLapRecord);
        }

        private bool FilterRecords(RaceRecord record) {
            bool shouldShow = true;
            if (powerupCondition != RecordConditionState.NotSet) shouldShow = shouldShow && ((powerupCondition == RecordConditionState.State2 && record.HadPowerups) || (powerupCondition == RecordConditionState.State1 && !record.HadPowerups && record.Date > RecordTypeControl.conditionsAddedDate));
            return shouldShow;
        }

        private void UpdateStageName()
        {
            stageNameField.text = ActiveData.Stages[selectedStage].name;
            UpdateFields();
        }

        private void UpdateIndividualCharacters() {
            individualCharactersField.text = individualCharacters ? "Yes" : "No";
            UpdateFields();
        }
        private void UpdateConditions() {
            hadPowerupsField.text = powerupCondition == RecordConditionState.NotSet ? "Not Set" : powerupCondition == RecordConditionState.State2 ? "Yes" : "No";
            UpdateFields();
        }
    }
}