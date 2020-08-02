using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordCounting
{
    /// <summary>
    /// static class to provide methods for counting the words from filepaths.
    /// </summary>
    public static class FileWordCounter
    {
        /// <summary>
        /// Counts the number of each word in the given file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="wordComparer"></param>
        /// <returns>a dictionary of the results using the given wordComparer</returns>
        public static IDictionary<string, int> CountWords(string filePath, StringComparer wordComparer)
        {
            if (!File.Exists(filePath)) { throw new FileNotFoundException(); }

            Dictionary<string, int> wordCounts = new Dictionary<string, int>(wordComparer);

            Parallel.ForEach<string>(File.ReadAllLines(filePath), line =>
            {
                IEnumerable<string> words = WordsFromString(line).ToList();
                foreach (string word in words)
                {
                    lock (wordCounts)
                    {
                        if (!wordCounts.TryAdd(word, 1))
                        {
                            wordCounts[word]++;
                        }
                    }
                }
            });

            return wordCounts;
        }

        /// <summary>
        /// Gets the words from a given string using the REGEX:
        /// [a-zA-Z]+[-'][a-zA-Z]+|[a-zA-Z]+[']?|[a-zA-Z]+
        /// </summary>
        /// <param name="line"></param>
        /// <returns>a list of the found words</returns>
        public static IEnumerable<string> WordsFromString(string line)
        {
            Regex wordMatchingRegex = new Regex(@"[a-zA-Z]+[-'][a-zA-Z]+|[a-zA-Z]+[']?|[a-zA-Z]+");
            return wordMatchingRegex.Matches(line).Select(x => x.Value);
        }

    }
}
