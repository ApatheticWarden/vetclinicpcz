using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicConsole
{
    internal class Database
    {
        public readonly SqliteConnection _connection;
        public uint lines = 25;
        public int page = 0;

        public Database(string path) {
            _connection = new SqliteConnection(("Data Source=" + path));
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
                CloseDB();
                
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
                            string hire_date = Convert.ToDateTime(reader["Hire_date"]).ToString("dd/MM/yyyy");
                            string occupation = reader["Occupation"].ToString();
                            decimal salary = Convert.ToDecimal(reader["Salary"]);
                            w = new Worker(id, name, surname,null, hire_date, occupation, salary);
                        }
                    }
                }
                CloseDB();
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
                CloseDB();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return w;
        }
    }
}
