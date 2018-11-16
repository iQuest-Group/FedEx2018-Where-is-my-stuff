using System;

namespace iQuest.FedEx2018.Wims
{
    class Program
    {
        private static readonly string helpString = "Type 's' to save data, x for exit";

        static void Main(string[] args)
        {
            Console.WriteLine();

            using (RfidReader rfidReader = new RfidReader())
            {
                rfidReader.StartReading();
                while (true)
                {
                    ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                    if (consoleKeyInfo.KeyChar == 'x')
                    {
                        break;
                    }
                    else if (consoleKeyInfo.KeyChar == 's')
                    {
                        rfidReader.Save();
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
