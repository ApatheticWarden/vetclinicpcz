using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicConsole
{
    internal class RecordsManager: Window
    {
        private Database _db;
        private Dictionary<string, Action<string[]>> _commands;
        private List<Record> _records= new List<Record>();
        public RecordsManager() {
            _commands = new Dictionary<string, Action<string[]>>(StringComparer.OrdinalIgnoreCase)
            {
                { "1", CmdRecords } , { "Records", CmdRecords },
                { "2", CmdExtend }, { "Extend", CmdExtend }
            };
        }

        private void CmdRecords(string[] args) {
            uint lines = 25;
            int page = 0;

            if (args.Length > 0) uint.TryParse(args[0], out lines);
            if (args.Length > 1) int.TryParse(args[1], out page);

            try {
                _records = _db.LoadRecords(_db._connection, lines, page);
            } catch (Exception ex) {
                Console.WriteLine("Error loading records: " + ex.Message);
                return;
            }

            if (_records.Count == 0) {
                Console.WriteLine("No records found.");
                return;
            }

            foreach (var r in _records)
                Console.WriteLine(r);

            Console.WriteLine($"Count: {lines} | Page: {page}");
        }
        private void CmdExtend(string[] args) {
            if (_records.Count == 0) {
                Console.WriteLine("Load records first with 'records' command.");
                return;
            }

            if (args.Length == 0 || !uint.TryParse(args[0], out uint id)) {
                Console.WriteLine("Syntax: extend <ID>");
                return;
            }

            var record = _records.FirstOrDefault(w => w.GetID() == id);

            Console.WriteLine(record != null
                ? "\n" + record.OutputData()
                : "Record not found in loaded list.");
        }
        public override void Run()
        {
            Console.Clear();

            _db = new Database("./Resources/Database/vets.db");

            string logo = "   __                        _     \r\n  /__\\ ___  ___ ___  _ __ __| |___ \r\n / \\/// _ \\/ __/ _ \\| '__/ _` / __|\r\n/ _  \\  __/ (_| (_) | | | (_| \\__ \\\r\n\\/ \\_/\\___|\\___\\___/|_|  \\__,_|___/\r\n                                   \r\n                                   \r\n                                   \r\n                                   \r\n                                   \r\n                                   \r\n                                   ";
            string helpString = "1) Show <Lines> <Pages> | Show n records by m pages";

            Console.WriteLine(logo, "\n", helpString);
            var vetsdb = new Database("./Resources/Database/vets.db");
            while (true) {
                Console.Write("\n> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) {
                    Console.WriteLine("Empty command. Type 'help'.");
                    continue;
                }

                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0];
                string[] args = parts.Skip(1).ToArray();

                if (_commands.TryGetValue(command, out var handler)) {
                    handler.Invoke(args);
                } else {
                    Console.WriteLine("Unknown command. Type 'help'.");
                }
            }
        }
    }
}
