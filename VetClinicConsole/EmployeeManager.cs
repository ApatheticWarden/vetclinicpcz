using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinic;

namespace VetClinicConsole
{
    internal class EmployeeManager: Window
    {
        public override void Run()
        {
            Console.Clear();
            string logo = "   __                _                                                                  \r\n  /__\\ __ ___  _ __ | | ___  _   _  ___  ___    /\\/\\   __ _ _ __   __ _  __ _  ___ _ __ \r\n /_\\| '_ ` _ \\| '_ \\| |/ _ \\| | | |/ _ \\/ _ \\  /    \\ / _` | '_ \\ / _` |/ _` |/ _ \\ '__|\r\n//__| | | | | | |_) | | (_) | |_| |  __/  __/ / /\\/\\ \\ (_| | | | | (_| | (_| |  __/ |   \r\n\\__/|_| |_| |_| .__/|_|\\___/ \\__, |\\___|\\___| \\/    \\/\\__,_|_| |_|\\__,_|\\__, |\\___|_|   \r\n              |_|            |___/                                      |___/           ";
            string helpString = "\n1) Employees <lines> <page>| Shows n employees at k page (Standart 25 per page)\n" +
                        "2) Extend <ID> | Extends data of employee with provided ID\n" +
                        "3) Help | Shows this tip\n" +
                        "4) Find | Finds different worker by ID or Name with Surname" +
                        "\n*) Exit | Exit from programm";
            List<Worker> Workers = new List<Worker>();

            // Initialization
            Database vetsdb = new Database("./Resources/Database/vets.db");
            Console.WriteLine(logo);
            Console.WriteLine("Here are some commands:");
            Console.WriteLine(helpString);

            while (true)
            {
                string answ = Console.ReadLine();
                if (vetsdb._connection != null)
                {
                    var parts = answ.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0)
                    {
                        Console.WriteLine("Invalid command, try again! Help for Tips");
                        continue;
                    }
                    if (parts.Length > 0 && (parts[0].ToLower() == "employees" || parts[0].ToLower() == "1"))
                    {
                        if (parts.Length > 1) uint.TryParse(parts[1], out vetsdb.lines);
                        if (parts.Length > 2) int.TryParse(parts[2], out vetsdb.page);

                        try
                        {
                            Workers = vetsdb.LoadEmployees(vetsdb._connection, vetsdb.lines, vetsdb.page);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Err during loading workers list: List is null " + e);
                            break;
                        }
                        if (Workers.Count > 0)
                        {
                            foreach (Worker w in Workers)
                                Console.WriteLine(w.ToString());
                            Console.WriteLine($"Count: {vetsdb.lines} | Page: {vetsdb.page} ");
                        }
                        else
                        {
                            Console.WriteLine("ERR | Can`t load workers");
                        }
                    }
                    else if (parts.Length > 0 && (parts[0].ToLower() == "extend" || parts[0].ToLower() == "2"))
                    {
                        if (Workers.Count > 0 && parts.Length > 1)
                        {
                            uint id;
                            uint.TryParse(parts[1], out id);

                            foreach (Worker w in Workers)
                            {
                                if (w.GetID() == id)
                                {
                                    Console.WriteLine("\n" + w.OutputData() + "\n");
                                }
                            }
                        }
                        else if (Workers.Count == 0)
                        {
                            Console.WriteLine("Wait a sec, load some workers first! (help for Tips)");
                            continue;
                        }
                    }
                    else if (parts.Length == 1 && (parts[0].ToLower() == "help" || parts[0].ToLower() == "3"))
                    {
                        Console.WriteLine(helpString);
                    }
                    else if (parts.Length > 0 && (parts[0].ToLower() == "find" || parts[0].ToLower() == "4"))
                    {
                        if (parts.Length > 1 && uint.TryParse(parts[1], out uint id))
                        {
                            var worker = vetsdb.FindByID(id);
                            if (worker != null)
                                Console.WriteLine("\n" + worker.OutputData());
                            else
                                Console.WriteLine(" Worker not found!");
                        }
                        else if (parts.Length > 2)
                        {
                            var worker = vetsdb.FindByName(parts[1], parts[2]);
                            if (worker != null)
                                Console.WriteLine("\n" + worker.OutputData());
                            else
                                Console.WriteLine(" Worker not found!");
                        }
                        else
                        {
                            Console.WriteLine("Invalid syntax\n Find <ID>\n OR\n Find John Doe");
                        }
                    }
                    else if (parts[0].ToLower() == "clear") {
                        Console.Clear();
                        Console.WriteLine($"{logo}\n{helpString}");
                    }
                    else if (parts[0].ToLower() == "exit" || parts[0].ToLower() == "*" || parts[0].ToLower() == "0")
                    {
                        var app = new AlmostOpusMagnum();
                        app.Run();
                    }
                }
                else
                {
                    Console.WriteLine("Warning! Your database is not loaded correctly!");
                }
            }
        }
    }
}
