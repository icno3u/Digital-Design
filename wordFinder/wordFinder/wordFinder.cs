using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace wordFinder
{
    internal class WordFinder
    {
        public List<string> _text;
        private char[] charsToRemove;
        private Dictionary<string, int> _words;
        public WordFinder(string inPath) 
        {
            _text = readTextFromFile(inPath);
            charsToRemove = new char[] { '/', ',', '"', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', '(', ')', '!', '?','+', '…', '«', '»',':',';', '–' };
            _words = FindUniqueWords();
        }
        private List<string> readTextFromFile(string inPath) //считывание текста из файла
        {
            List<string> tempText = new List<string>();
            using(StreamReader reader = new StreamReader(inPath)) 
            {
                while(!reader.EndOfStream)
                {
                    tempText.Add(reader.ReadLine());
                }
            }
            return tempText;
        }
        private List<string> FindWords() // нахождение всех слов в тексте
        {
            for(int i = 0; i < _text.Count; i++)
            {
                foreach (var c in charsToRemove) //удаление 'мусорных' символов
                {
                    _text[i] = string.Join("", _text[i].ToCharArray().Where(a => !charsToRemove.Contains(a)).ToArray());
                }
                if (_text[i] != "" && _text[i][_text[i].Length - 1] == '-') //проверка на перенос и его замена на специальный знак ('+')
                {
                    _text[i] = _text[i].Remove(_text[i].Length - 1, 1);
                    _text[i] =  _text[i].Insert(_text[i].Length, "+");
                }
            }
            List<string> words = new List<string>();
            foreach(string line in _text)
            {
                words.AddRange(line.Split(' ').ToArray());
            }
            words = words.Where(p=>p!="" & p!="-").ToList(); //удаление пустых значений и оставшихся '-'
            for (int i = 0; i < words.Count; i++)
            {
                if (words[i][words[i].Length-1] == '+')
                {
                    words[i] = words[i].Remove(words[i].Length-1, 1);
                    words[i+1] = $"{words[i]}{words[i+1]}";
                    words.RemoveAt(i);
                }
            }
            words = words.Select(word=>word.ToLower()).ToList(); //всё в нижний регистр
            return words;
        }
        public Dictionary<string, int> FindUniqueWords() //производит пересчёт слов и их сортировку
        {
            Dictionary<string, int> tempWords = new Dictionary<string, int>();
            List<string> wordsForParse = FindWords();
            foreach(string word in wordsForParse) 
            {
                if (tempWords.ContainsKey(word) & word!="-") 
                {
                    tempWords[word]++;
                }
                else 
                {
                    tempWords.Add(word, 1);
                }
            }
            tempWords = tempWords.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); //сортировка по убыванию
            return tempWords;
        }
        public void WriteWordsInFile(string outPath) // запись в файл
        {
            using(StreamWriter writer = new StreamWriter(outPath)) 
            {
                foreach(KeyValuePair<string, int> entry in _words) 
                {
                    writer.WriteLine($"{entry.Key}  {entry.Value}");
                }
            }
        }
    }
}
