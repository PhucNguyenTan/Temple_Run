
using UnityEngine;
using TMPro;


namespace ScoreBoard
{
    public class ScoreBoardEntryUI : MonoBehaviour
    {
        TextMeshProUGUI _name;
        TextMeshProUGUI _score;
        TextMeshProUGUI _rank;

        private void Awake()
        {
            _rank = transform.Find("Rank").gameObject.transform.GetComponent<TextMeshProUGUI>();
            _name = transform.Find("Name").gameObject.transform.GetComponent<TextMeshProUGUI>();
            _score = transform.Find("Score").gameObject.transform.GetComponent<TextMeshProUGUI>();
        }

        public void Initialize(string rank, ScoreBoardEntryData entryData)
        {
            _rank.text = rank;
            _name.text = entryData.entryName;
            _score.text = entryData.entryScore.ToString();
        }
    }
}
