using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ScoreBoard
{
    public class ScorePanel : MonoBehaviour
    {
        [SerializeField] ScoreManager _scoreManager;
        [SerializeField] TextMeshProUGUI _highScoreBanner;
        [SerializeField] TextMeshProUGUI _newScore;
        [SerializeField] TextMeshProUGUI _newCoinAmount;
        [SerializeField] TMP_InputField _inputField;
        [SerializeField] Button _saveBtn;
        [SerializeField] IGMUI _igmUI;

        string _name;
        int _score;


        private void OnEnable()
        {
            _score = _scoreManager.Score;
            _newScore.text = _score.ToString();
            _newCoinAmount.text = _scoreManager.CoinOwned.ToString();
            _highScoreBanner.gameObject.SetActive(false);

            if (!_igmUI.IsSaveBtnDisabled)
                UnlockSaveAbility();
            CheckNewHighScore();
        }

        

        public void SaveScoreEntry()
        {
            ScoreBoardEntryData entry = new ScoreBoardEntryData().AddData(_name, _score);
            string jsonString = PlayerPrefs.GetString("SavedScore");
            ScoreBoardSaveData savedScore;
            savedScore = JsonUtility.FromJson<ScoreBoardSaveData>(jsonString);
            if(savedScore == null)
            {
                savedScore = new ScoreBoardSaveData();
            }
            savedScore.entries.Add(entry);
            string jsonToSave = JsonUtility.ToJson(savedScore);
            PlayerPrefs.SetString("SavedScore", jsonToSave);
            LockSaveAbility();
        }

        void CheckNewHighScore()
        {
            string jsonString = PlayerPrefs.GetString("SavedScore");
            ScoreBoardSaveData savedScore = JsonUtility.FromJson<ScoreBoardSaveData>(jsonString);
            if (savedScore != null)
            {
                for(var i = 0; i<savedScore.entries.Count; i++)
                {
                    if(_score <= savedScore.entries[i].entryScore)
                    {
                        return;
                    }
                }

            }
            _highScoreBanner.gameObject.SetActive(true);
        }

        public void GetInputString(string strInput)
        {
            _name = strInput;
        }

        void LockSaveAbility()
        {
            _inputField.interactable = false;
            _saveBtn.interactable = false;
            _igmUI.LockSaveAbilitySignal();
        }

        void UnlockSaveAbility()
        {
            _inputField.interactable = true;
            _saveBtn.interactable = true;
            _inputField.text = "";
        }
    }
}

