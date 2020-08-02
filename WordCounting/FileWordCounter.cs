using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            Dictionary<string, int> wordCounts = new Dictionary<string, int>(wordComparer);

            //read all lines in parrallel
            //get words from line
            //foreach word in line
            //try add word to dictionary
            //if not then increment count at [word]

            return wordCounts;
        }

        /// <summary>
        /// Gets the words from a given string using the REGEX:
        /// null
        /// </summary>
        /// <param name="line"></param>
        /// <returns>a list of the found words</returns>
        public static IEnumerable<string> WordsFromString(string line)
        {
            Regex wordMatchingRegex = null;
            return wordMatchingRegex.Matches(line).Select(x => x.Value);
        }

    }
}
