using Microsoft.Azure.Documents;
using System;
using System.Threading.Tasks;

namespace iQuest.Fedex2018.Winms.TagsFilesProcessor
{
    class Program
    {
        private static readonly string helpString = "Type x for exit";

        static void Main(string[] args)
        {
            RecognizedTagsFilesProcessor recognizedTagsFilesProcessor =
                new RecognizedTagsFilesProcessor();

            Task.WaitAll(recognizedTagsFilesProcessor.Start());

            while (true)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                if (consoleKeyInfo.KeyChar == 'x')
                {
                    break;
                }
                else
                {
                    Console.WriteLine(helpString);
                }
            }

        }
    }
}
