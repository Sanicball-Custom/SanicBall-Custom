using System;
using Sanicball.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class RecordTypeControl : MonoBehaviour
    {
		public Text titleField;
        public Text timeField;
        public Text rawTimeField;
        public Text characterField;
        public Text dateField;
        public Color timeColor = new Color(50 / 255f, 50 / 255f, 50 / 255f);
        public Color noRecordColor = new Color(0.5f,0.5f,0.5f);

        public void SetRecord(RaceRecord r)
        {
            if (r != null)
            {
                var timespan = TimeSpan.FromSeconds(r.Time);
                timeField.color = timeColor;
                timeField.text = string.Format("{0:00}:{1:00}.{2:000}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                characterField.text = "Set with " + ActiveData.Characters[r.Character].name;
                dateField.text = r.Date.ToString();
                if(r.RawTime != 0){
                    var rawTimespan = TimeSpan.FromSeconds(r.RawTime);
                    rawTimeField.text = string.Format("{0:00}:{1:00}.{2:000}", rawTimespan.Minutes, rawTimespan.Seconds, rawTimespan.Milliseconds);
                }else{
                    rawTimeField.text = "No Raw Time Found.";
                    rawTimeField.color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
            else
            {
                timeField.text = "No records found";
                timeField.color = noRecordColor;
                rawTimeField.text = "";
                characterField.text = "";
                dateField.text = "";
            }
        }
    }
}