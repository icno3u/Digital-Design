namespace wordFinder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inPath = "Test.txt";
            string outPath = "Answer.txt";
            WordFinder wordFinder = new WordFinder(inPath);
            wordFinder.WriteWordsInFile(outPath);
        }
    }
}
