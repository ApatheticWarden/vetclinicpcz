using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicConsole
{
    internal class RecordsManager: Window
    {
        public override void Run()
        {
            Console.Clear();
            string logo = "   __                        _     \r\n  /__\\ ___  ___ ___  _ __ __| |___ \r\n / \\/// _ \\/ __/ _ \\| '__/ _` / __|\r\n/ _  \\  __/ (_| (_) | | | (_| \\__ \\\r\n\\/ \\_/\\___|\\___\\___/|_|  \\__,_|___/\r\n                                   \r\n                                   \r\n                                   \r\n                                   \r\n                                   \r\n                                   \r\n                                   ";
            string helpString = "1) Show <Lines> <Pages> | Show n records by m pages";

            Console.WriteLine(logo, "\n", helpString);
            var vetsdb = new Database("./Resources/Database/vets.db");
            while (true)
            {
                var answ = Console.ReadLine();
                if (vetsdb._connection != null)
                {
                    var parts = answ.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0)
                    {
                        Console.WriteLine("Invalid command, try again! Help for Tips");
                        continue;
                    }

                }
            }
        }
    }
}
