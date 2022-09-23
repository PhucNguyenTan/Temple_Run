using System;

namespace ScoreBoard
{
    [Serializable]
    public struct ScoreBoardEntryData
    {


        public string entryName;
        public int entryScore;

        public ScoreBoardEntryData AddData(string name, int score)
        {
            entryName = name;
            entryScore = score;
            return this;
        }
    }
}
