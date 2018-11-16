using System;
using System.Threading.Tasks;

namespace iQuest.Fedex2018.Winms.TagsFilesProcessor
{
    static class Program
    {
        private static readonly string helpString = "Type x for exit";

        static void Main(string[] args)
        {
            Console.WriteLine("Tags file processor starting...");
            Console.WriteLine(helpString);
            Console.WriteLine();

            Console.WriteLine("Please provide the inventory name");
            string inventoryName = Console.ReadLine();
            Console.WriteLine();

            using (RecognizedTagsFilesProcessor recognizedTagsFilesProcessor =
                new RecognizedTagsFilesProcessor(inventoryName))
            {

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
}
