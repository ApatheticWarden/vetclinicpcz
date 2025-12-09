using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VetClinicConsole
{
    internal class Database
    {
        public readonly SqliteConnection _connection;
        public uint lines = 25;
        public int page = 0;

        public Database(string path) {
            _connection = new SqliteConnection(("Data Source=" + path));
            _connection.Open();
        }
        void OpenDB()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }
        void CloseDB()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
            }
        }
        public List<Worker> LoadEmployees(SqliteConnection con, uint lines = 25, int page = 0)
        {
            List<Worker> buffer = new List<Worker>();
            try
            {
                OpenDB();
                
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $"SELECT *  FROM EMPLOYEES LIMIT {lines} OFFSET {lines * page}";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            uint id = Convert.ToUInt32(reader["ID"]);
                            string name = reader["Name"].ToString();
                            string surname = reader["Last_name"].ToString();
                            string hire_date = reader["Hire_date"].ToString();
                            string occupation = reader["Occupation"].ToString();
                            decimal salary = Convert.ToDecimal(reader["Salary"]);
                            var w = new Worker(id, name, surname,null, hire_date, occupation, salary);
                            buffer.Add(w);
                        }
                    }
                }
                //CloseDB();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return buffer;
        }
        public List<Record> LoadRecords(SqliteConnection con,uint lines = 25, int page = 0)
        {
            List<Record> buffer = new List<Record>();
            try
            {
                OpenDB();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $"SELECT *  FROM RECORDS LIMIT {lines} OFFSET {lines * page}";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Add some data here
                            //var w = new Record(DateTime dt,Client cli, Animal clianm);
                            //buffer.Add(w);
                        }
                    }
                }
                CloseDB();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return buffer;
        }
        public Worker FindByID(uint idToFind)
        {
            Worker? w = null;
            try
            {
                OpenDB();
                using (SqliteCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM EMPLOYEES WHERE ID = @id";
                    command.Parameters.AddWithValue("@id", idToFind);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            uint id = Convert.ToUInt32(reader["ID"]);
                            string name = reader["Name"].ToString();
                            string surname = reader["Last_name"].ToString();

                            // Parse hire date with multiple possible formats
                            string hireDateStr = reader["Hire_date"].ToString();
                            DateTime hireDate;
                            string[] formats = { "dd.MM.yyyy", "yyyy-MM-dd", "dd/MM/yyyy" };
                            if (!DateTime.TryParseExact(hireDateStr, formats, CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None, out hireDate))
                            {
                                // fallback if parsing fails
                                hireDate = DateTime.MinValue;
                                Console.WriteLine($"Warning: Cannot parse hire date for worker {name} {surname}");
                            }

                            string hire_date = hireDate.ToString("dd/MM/yyyy"); // format to European

                            string occupation = reader["Occupation"].ToString();
                            decimal salary = Convert.ToDecimal(reader["Salary"]);

                            w = new Worker(id, name, surname, null, hire_date, occupation, salary);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return w;
        }
        public Worker FindByName(string Name, string Surname)
        {
            Worker? w = null;
            try
            {
                OpenDB();
                using (SqliteCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM EMPLOYEES WHERE NAME = @name AND LAST_NAME = @surname";
                    command.Parameters.AddWithValue("@name", Name);
                    command.Parameters.AddWithValue("@surname", Surname);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            uint id = Convert.ToUInt32(reader["ID"]);
                            string name = reader["Name"].ToString();
                            string surname = reader["Last_name"].ToString();
                            string hire_date = Convert.ToDateTime(reader["Hire_date"]).ToString("dd/MM/yyyy");
                            string occupation = reader["Occupation"].ToString();
                            decimal salary = Convert.ToDecimal(reader["Salary"]);
                            w = new Worker(id, name, surname,null, hire_date, occupation, salary);
                        }
                    }
                }
                //CloseDB();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return w;
        }

        /// <summary>
        /// Allows editing of an employee's data by ID. 
        /// User can choose which fields to change.
        /// </summary>
        public void EditWorker(uint ID)
        {
            if (_connection == null)
            {
                Console.WriteLine("Connection to database is null!");
                return;
            }

            OpenDB();
            var worker = FindByID(ID);
            if (worker == null)
            {
                Console.WriteLine("Worker not found.");
                return;
            }

            string newName = worker.Name;
            string newLastName = worker.Surname;
            string newHireDate = worker.HireDate;
            string newOccupation = worker.Occupation;
            decimal newSalary = worker.Salary;

            while (true)
            {
                Console.WriteLine("Current Worker Data:");
                Console.WriteLine($"1) Name: {newName}");
                Console.WriteLine($"2) Last Name: {newLastName}");
                Console.WriteLine($"3) Hire Date: {newHireDate}");
                Console.WriteLine($"4) Occupation: {newOccupation}");
                Console.WriteLine($"5) Salary: {newSalary}");
                Console.WriteLine("0) Save and exit");
                Console.WriteLine("----------------------------------------");
                Console.Write("Enter number of field to change: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        newName = AskForString("Name", newName);
                        break;
                    case "2":
                        newLastName = AskForString("Last Name", newLastName);
                        break;
                    case "3":
                        newHireDate = AskForString("Hire Date", newHireDate);
                        break;
                    case "4":
                        newOccupation = AskForString("Occupation", newOccupation);
                        break;
                    case "5":
                        newSalary = AskForDecimal("Salary", newSalary);
                        break;
                    case "0":
                        UpdateWorkerInDB(ID, newName, newLastName, newHireDate, newOccupation, newSalary);
                        return;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Asking input from user
        /// Made for replacing everlasting if else
        /// </summary>
        /// <returns> Value if not null or current if it is</returns>
        private string AskForInput(string prompt, string currentValue) {
            Console.Write($"{prompt} (leave empty to keep '{currentValue}'): ");
            string input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? currentValue : input;
        }

        /// <summary>
        /// Prompts the user to input a string for a field, allowing to keep the current value if left empty.
        /// </summary>
        /// <returns>The new value entered by the user, or the current value if input is empty.</returns>
        private string AskForString(string field, string currentValue)
        {
            Console.Write($"New {field} (leave empty to keep current): ");
            string input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? currentValue : input;
        }

        /// <summary>
        /// Prompts the user to input a decimal number for a field, allowing to keep the current value if left empty.
        /// </summary>
        /// <returns>The new decimal value entered by the user, or the current value if input is empty or invalid.</returns>
        private decimal AskForDecimal(string field, decimal currentValue)
        {
            Console.Write($"New {field} (leave empty to keep current): ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return currentValue;

            if (decimal.TryParse(input, out decimal result))
                return result;

            Console.WriteLine("Invalid input, keeping current value.");
            return currentValue;
        }

        /// <summary>
        /// Updates the worker's data in the database with new values.
        /// </summary>
        private void UpdateWorkerInDB(uint ID, string name, string lastName, string hireDate, string occupation, decimal salary)
        {
            string query = @"
        UPDATE EMPLOYEES
        SET Name = @Name,
            Last_name = @LastName,
            Hire_Date = @HireDate,
            Occupation = @Occupation,
            Salary = @Salary
        WHERE ID = @ID;";

            using var command = _connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@LastName", lastName);
            command.Parameters.AddWithValue("@HireDate", hireDate);
            command.Parameters.AddWithValue("@Occupation", occupation);
            command.Parameters.AddWithValue("@Salary", salary);

            int rows = command.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Worker updated successfully!" : "Update failed.");
        }

        /// <summary>
        /// Deletes a record from the specified table based on its ID.
        /// Basic user dont have access to it, so only dev has
        /// </summary>
        public void DeleteRecord(uint ID, string tableName)
        {
            string query = $"DELETE FROM {tableName} WHERE ID = @ID;";

            using var command = _connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@ID", ID);

            int rows = command.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Record deleted successfully!" : "Delete failed.");
        }

        /// <summary>
        /// Adds different record based on users input
        /// Table - Employees
        /// </summary>
        public void AddWorker() {
            if (_connection == null) {
                Console.WriteLine("Connection to database is null!");
                return;
            }

            string name, lastName, occupation;
            DateTime hireDate; // = Convert.ToDateTime()
            decimal salary;
            name = AskForInput("Name", "Empty");
            lastName = AskForInput("Surname", "Empty");
            hireDate = Convert.ToDateTime(AskForInput("Hire Date | YYYY-MM-DD", DateTime.MinValue.ToString()));
            occupation = AskForInput("Occupation", "Empty");
            salary = AskForDecimal("Salary", 0.0m);

            using var command = _connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO EMPLOYEES (Name, Last_name, Hire_Date, Occupation, Salary) 
                VALUES (@Name, @LastName, @HireDate, @Occupation, @Salary)";

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@LastName", lastName);
            command.Parameters.AddWithValue("@HireDate", hireDate.ToString("yyyy-MM-dd")); // store as ISO
            command.Parameters.AddWithValue("@Occupation", occupation);
            command.Parameters.AddWithValue("@Salary", salary);

            int rows = command.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Worker added successfully!" : "Failed to add worker.");
        }
    }
}
