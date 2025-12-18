using System.Data.Common;
using System.IO;
using System.Text;
using Microsoft.Data.Sqlite;
using VetClinicConsole;

namespace VetClinic{
    public class AlmostOpusMagnum: Window{
        public static void Main(string[] args) {
            Database _db = new Database("./Resources/Database/vets.db");
            var app = new AlmostOpusMagnum(); 
            app.Run(_db);
        }
        public override void Run(Database _db) {
            Console.Clear();
            const string logo = "            _     ___ _ _       _                                                 \r\n /\\   /\\___| |_  / __\\ (_)_ __ (_) ___    /\\/\\   __ _ _ __   __ _  __ _  ___ _ __ \r\n \\ \\ / / _ \\ __|/ /  | | | '_ \\| |/ __|  /    \\ / _` | '_ \\ / _` |/ _` |/ _ \\ '__|\r\n  \\ V /  __/ |_/ /___| | | | | | | (__  / /\\/\\ \\ (_| | | | | (_| | (_| |  __/ |   \r\n   \\_/ \\___|\\__\\____/|_|_|_| |_|_|\\___| \\/    \\/\\__,_|_| |_|\\__,_|\\__, |\\___|_|   \r\n                                                                  |___/           ";
            Console.WriteLine(logo);
            string helpString = "1) Employee Manager\n2) Record Manager";
            Console.WriteLine(helpString);
            string answ = Console.ReadLine();
            if (answ.Contains("1"))
            {
                EmployeeManager employeeManager = new EmployeeManager();
                employeeManager.Run(_db);
            } else if (answ.Contains("2")) { 
                RecordsManager recMan = new RecordsManager();
                recMan.Run(_db);
            }
            else
            {
                Console.WriteLine("Bruh...");
            }
        }
    }
}