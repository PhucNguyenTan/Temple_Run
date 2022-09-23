using System;
using System.Collections.Generic;
using System.Linq;

namespace ScoreBoard
{
    [Serializable]
    public class ScoreBoardSaveData
    {
        public List<ScoreBoardEntryData> entries = new List<ScoreBoardEntryData>();

        public void SortEntries_Score()
        {
            entries.Sort((x, y) => y.entryScore.CompareTo(x.entryScore));
            //entries.Sort((x, y) => x.entryName.CompareTo(y.entryName));

        }
    }

}

