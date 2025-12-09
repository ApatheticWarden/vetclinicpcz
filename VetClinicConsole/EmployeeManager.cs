using System;
using System.Collections.Generic;
using System.Linq;
using VetClinic;

namespace VetClinicConsole
{
    internal class EmployeeManager : Window
    {
        private Database _db;
        private List<Worker> _workers = new List<Worker>();

        // Commands dictionary
        private Dictionary<string, Action<string[]>> _commands;

        public EmployeeManager()
        {
            _commands = new Dictionary<string, Action<string[]>>(StringComparer.OrdinalIgnoreCase)
            {
                { "employees", CmdEmployees }, { "1", CmdEmployees },
                { "extend", CmdExtend },       { "2", CmdExtend },
                { "help", CmdHelp },           { "3", CmdHelp },
                { "find", CmdFind },           { "4", CmdFind },
                { "edit", CmdEdit },           { "5", CmdEdit },
                { "del", CmdEdit },           { "6", CmdEdit },
                { "add", CmdEdit },           { "7", CmdEdit },
                { "clear", CmdClear },
                { "exit", CmdExit }, { "*", CmdExit }, { "0", CmdExit }
            };
        }

        public override void Run()
        {
            Console.Clear();
            PrintLogo();
            PrintHelp();

            _db = new Database("./Resources/Database/vets.db");

            if (_db._connection == null)
            {
                Console.WriteLine("⚠ ERROR: Database failed to load!");
                return;
            }

            while (true)
            {
                Console.Write("\n> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Empty command. Type 'help'.");
                    continue;
                }

                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0];
                string[] args = parts.Skip(1).ToArray();

                if (_commands.TryGetValue(command, out var handler))
                {
                    handler.Invoke(args);
                }
                else
                {
                    Console.WriteLine("Unknown command. Type 'help'.");
                }
            }
        }

        // ────────────────────────────────────────────────────────────
        // COMMAND IMPLEMENTATIONS
        // ────────────────────────────────────────────────────────────

        private void CmdEmployees(string[] args)
        {
            uint lines = 25;
            int page = 1;

            if (args.Length > 0) uint.TryParse(args[0], out lines);
            if (args.Length > 1) int.TryParse(args[1], out page);

            try
            {
                _workers = _db.LoadEmployees(_db._connection, lines, page);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading employees: " + ex.Message);
                return;
            }

            if (_workers.Count == 0)
            {
                Console.WriteLine("No employees found.");
                return;
            }

            foreach (var w in _workers)
                Console.WriteLine(w);

            Console.WriteLine($"Count: {lines} | Page: {page}");
        }

        private void CmdExtend(string[] args)
        {
            if (_workers.Count == 0)
            {
                Console.WriteLine("Load employees first with 'employees' command.");
                return;
            }

            if (args.Length == 0 || !uint.TryParse(args[0], out uint id))
            {
                Console.WriteLine("Syntax: extend <ID>");
                return;
            }

            var worker = _workers.FirstOrDefault(w => w.GetID() == id);

            Console.WriteLine(worker != null
                ? "\n" + worker.OutputData()
                : "Worker not found in loaded list.");
        }

        private void CmdHelp(string[] args) => PrintHelp();

        private void CmdFind(string[] args)
        {
            if (args.Length == 1 && uint.TryParse(args[0], out uint id))
            {
                var w = _db.FindByID(id);
                Console.WriteLine(w?.OutputData() ?? "Worker not found.");
            }
            else if (args.Length == 2)
            {
                var w = _db.FindByName(args[0], args[1]);
                Console.WriteLine(w?.OutputData() ?? "Worker not found.");
            }
            else
            {
                Console.WriteLine("Usage:\n find <ID>\n find <Name> <Surname>");
            }
        }

        private void CmdEdit(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: edit <ID>");
                return;
            }

            if (!uint.TryParse(args[0], out uint id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            _db.EditWorker(id);
        }

        private void CmdDelete(string[] args) {
            if (args.Length < 2){
                Console.WriteLine("Usage: delete <ID> <TableName>");
                return;
            }
            if (!uint.TryParse(args[0], out uint id)){
                Console.WriteLine("Invalid ID.");
                return;
            }
            string table = args[1];
            _db.DeleteRecord(id, table);
        }

        private void CmdClear(string[] args)
        {
            Console.Clear();
            PrintLogo();
            PrintHelp();
        }

        private void CmdExit(string[] args)
        {
            var app = new AlmostOpusMagnum();
            app.Run();
        }

        // ────────────────────────────────────────────────────────────
        // UTILITY METHODS
        // ────────────────────────────────────────────────────────────

        private void PrintLogo()
        {
            Console.WriteLine(
@"   __                _                                                                  
  /__\ __ ___  _ __ | | ___  _   _  ___  ___    /\/\   __ _ _ __   __ _  __ _  ___ _ __ 
 /_\| '_ ` _ \| '_ \| |/ _ \| | | |/ _ \/ _ \  /    \ / _` | '_ \ / _` |/ _` |/ _ \ '__|
//__| | | | | | |_) | | (_) | |_| |  __/  __/ / /\/\ \ (_| | | | | (_| | (_| |  __/ |   
\__/|_| |_| |_| .__/|_|\___/ \__, |\___|\___| \/    \/\__,_|_| |_|\__,_|\__, |\___|_|   
              |_|            |___/                                      |___/            ");
        }

        private void PrintHelp()
        {
            Console.WriteLine(@"
COMMANDS:
1) employees <lines> <page>      - Shows N employees at page K
2) extend <ID>                    - Shows extended info from loaded employees
3) help                           - Shows this help menu
4) find <ID> | <Name> <Surname>   - Find employee by ID or full name
5) edit <ID>                      - Edit employee data
clear                             - Clear the screen
exit / 0 / *                      - Exit to main menu
");
        }
    }
}
