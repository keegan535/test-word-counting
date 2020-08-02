using System;
using System.Collections.Generic;
using System.Text;

namespace WordCountingApp.Models
{
    class RankedWordCountItem
    {
        public int Rank { get; set; }

        public string Word { get; set; }

        public int Count { get; set; }

        public RankedWordCountItem(int rank, string word, int count)
        {
            Rank = rank;
            Word = word;
            Count = count;
        }

    }
}
