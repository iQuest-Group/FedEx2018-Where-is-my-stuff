using System;

namespace iQuest.FedEx2018.Wims
{
    static class Program
    {
        private static readonly string helpString = "Type 's' to save data, 'f' for finish inventory, 'x' for exit";

        static void Main(string[] args)
        {
            Console.WriteLine();

            using (RfidReader rfidReader = new RfidReader())
            {
                rfidReader.StartReading();
                while (true)
                {
                    ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                    if (consoleKeyInfo.KeyChar == 's')
                    {
                        rfidReader.Save();
                    }
                    else if (consoleKeyInfo.KeyChar == 'f')
                    {
                        rfidReader.FinishInventory();
                    }
                    else if (consoleKeyInfo.KeyChar == 'x')
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
