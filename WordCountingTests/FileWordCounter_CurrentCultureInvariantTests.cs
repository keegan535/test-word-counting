using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordCounting.Tests
{
    public class FileWordCounter_CurrentCultureInvariantTests
    {
        string _testFilePath;
        Dictionary<string, int> _testFileWordCounts = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
        List<List<string>> _expectedWordsPerLine = new List<List<string>>();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _testFilePath = $"c:\\Temp\\WordCounterTestFile_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}.txt";

            List<string> lines = new List<string>()
            {
                "This is my test file which contains some text.",
                "this checks if it's a word or a number(1234) or just punctuation('\"\\/!?-).",
                "Seventy-five is one word. don't is one.",
                "\"Emmerich\", \"Emmerich's\", \"Emmerichs\" are different."
            };

            _expectedWordsPerLine = new List<List<string>>();
            _expectedWordsPerLine.Add(new List<string>() { "This", "is", "my", "test", "File", "which", "contains", "some", "text" });
            _expectedWordsPerLine.Add(new List<string>() { "this", "checks", "if", "it's", "a", "word", "or", "a", "number", "or", "just", "punctuation" });
            _expectedWordsPerLine.Add(new List<string>() { "Seventy-five", "is", "one word", "don't", "is", "one" });
            _expectedWordsPerLine.Add(new List<string>() { "Emmerich", "Emmerich's", "Emmerichs", "are", "different" });

            _testFileWordCounts = new Dictionary<string, int>();
            foreach (List<string> words in _expectedWordsPerLine)
            {
                foreach (string word in words)
                {
                    if (!_testFileWordCounts.TryAdd(word, 1))
                    {
                        _testFileWordCounts[word]++;
                    }
                }
            }

            File.WriteAllLines(_testFilePath, lines);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            File.Delete(_testFilePath);
        }

        [Test]
        public void CountWords_ValidFilePath_ReturnsCorrectWordCount()
        {
            Assert.AreEqual(_testFileWordCounts, FileWordCounter.CountWords(_testFilePath, StringComparer.CurrentCultureIgnoreCase));
        }

        [Test]
        public void CountWords_InvalidFilePath_ReturnsEmptyDictionary()
        {
            Assert.AreEqual(new Dictionary<string, int>(), FileWordCounter.CountWords("", StringComparer.CurrentCultureIgnoreCase));
        }

        [Test]
        public void WordsFromString_ValidStringLine_ReturnsCorrectWordList()
        {
            string line = "This is my test file which contains some text." +
                "this checks if it's a word or a number(1234) or just punctuation('\"\\/!?-)." +
                "Seventy-five is one word. don't is one." +
                "\"Emmerich\", \"Emmerich's\", \"Emmerichs\" are different.";

            List<string> expectedWords = new List<string>()
            {
                "This", "is", "my", "test", "File", "which", "contains", "some", "text",
                "this", "checks", "if", "it's", "a", "word", "or", "a", "number", "or", "just", "punctuation",
                "Seventy-five", "is", "one word", "don't", "is", "one",
                "Emmerich", "Emmerich's", "Emmerichs", "are", "different"
            };

            List<string> actualWords = FileWordCounter.WordsFromString(line).ToList();

            Assert.AreEqual(expectedWords, actualWords);
        }

        [Test]
        public void WordsFromString_Null_ReturnsEmptyWordList()
        {
            Assert.AreEqual(new List<string>(), FileWordCounter.WordsFromString(null));
        }

        [Test]
        public void WordsFromString_EmptyString_ReturnsEmptyWordList()
        {
            Assert.AreEqual(new List<string>(), FileWordCounter.WordsFromString(string.Empty));
        }

        [Test]
        public void WordsFromString_StringWithNoValidWords_ReturnsEmptyWordList()
        {
            Assert.AreEqual(new List<string>(), FileWordCounter.WordsFromString("123456 --- !'\\\"/?><"));
        }

    }
}