using System.Data.Common;
using System.IO;
using System.Text;
using Microsoft.Data.Sqlite;
using VetClinicConsole;

namespace VetClinic{
    public class AlmostOpusMagnum: Window{

        public static void Main(string[] args) {
            Database db;
            try {
                db = new Database("./Resources/Database/vets.db");
            } catch (Exception ex) {
                Console.WriteLine("Failed to initialize database: " + ex.Message);
                return;
            }
            var nav = new AppNavigator(db);
            nav.Start();
        }

        public override Window Run(Database _db) {
            Console.Clear();
            const string logo = "            _     ___ _ _       _                                                 \r\n /\\   /\\___| |_  / __\\ (_)_ __ (_) ___    /\\/\\   __ _ _ __   __ _  __ _  ___ _ __ \r\n \\ \\ / / _ \\ __|/ /  | | | '_ \\| |/ __|  /    \\ / _` | '_ \\ / _` |/ _` |/ _ \\ '__|\r\n  \\ V /  __/ |_/ /___| | | | | | | (__  / /\\/\\ \\ (_| | | | | (_| | (_| |  __/ |   \r\n   \\_/ \\___|\\__\\____/|_|_|_| |_|_|\\___| \\/    \\/\\__,_|_| |_|\\__,_|\\__, |\\___|_|   \r\n                                                                  |___/           ";
            Console.WriteLine(logo);
            const string help = @"
1) Employees      | Manage employee records
2) Records        | Manage veterinary records
0) Exit           | Exit the application
";
            Console.WriteLine(help);
            string choice = Console.ReadLine()?.Trim();
            switch (choice) {
                case "1":
                case "employees":
                    return new EmployeeManager();
                case "2":
                case "records":
                    return new RecordsManager();
                case "0":
                case "exit":
                    return null;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    return this;
            }
        }
    }
}