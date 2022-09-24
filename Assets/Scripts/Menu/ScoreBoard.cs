using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ScoreBoard
{
    public class ScoreBoard : MonoBehaviour
    {
        [SerializeField] int _maxEntry = 5;
        [SerializeField] ScoreBoardEntryUI _scoreBoardObj = null;

        private void OnEnable()
        {
            ScoreBoardSaveData testData = new ScoreBoardSaveData();
            testData.entries = new List<ScoreBoardEntryData> {
                new ScoreBoardEntryData().AddData("bcd", 123),
                new ScoreBoardEntryData().AddData("abc", 45613),
                new ScoreBoardEntryData().AddData("cdf", 43212),
                new ScoreBoardEntryData().AddData("egh", 01313),
                new ScoreBoardEntryData().AddData("423", 3564)
            };

            UpdateScoreboardUI(GetSavedData());
        }

        void UpdateScoreboardUI(ScoreBoardSaveData saveData)
        {
            RemoveItemOnUI();
            if (saveData == null) return;
            if (saveData.entries.Count < 1) return;
            saveData.SortEntries_Score();
            for (int i = 0; i < saveData.entries.Count; i++)
            {
                string rankstring;
                int ranknum = i + 1;
                switch (i)
                {
                    case 0:
                        rankstring = ranknum + "st"; break;
                    case 1:
                        rankstring = ranknum + "nd"; break;
                    case 2:
                        rankstring = ranknum + "rd"; break;
                    default:
                        rankstring = ranknum + "th"; break;

                }
                ScoreBoardEntryUI entry = Instantiate(_scoreBoardObj, transform);
                entry.Initialize(rankstring, saveData.entries[i]);
            }
        }

        void RemoveItemOnUI()
        {
            if(transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        ScoreBoardSaveData GetSavedData()
        {
            ScoreBoardSaveData entriesSaved;
            string jsonString = PlayerPrefs.GetString("SavedScore");
            entriesSaved = JsonUtility.FromJson<ScoreBoardSaveData>(jsonString);
            return entriesSaved;
        }

        public void DeleteSavedData()
        {
            PlayerPrefs.SetString("SavedScore", "");
            UpdateScoreboardUI(GetSavedData());
        }
    }
}
