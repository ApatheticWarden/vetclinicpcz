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
            while (true) {
                var recordsdb = new Database("");
            }
        }
    }
}
