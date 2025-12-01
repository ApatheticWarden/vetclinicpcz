using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicConsole
{
    internal class Worker : Person
    {
        public uint ID { get; private set; }
        public string? HireDate { get; private set; }
        public string Occupation { get; private set; }
        public decimal Salary { get; private set; }

        public Worker(uint id, string name, string surname, Dictionary<string, string>? con, string? hireDate, string occupation, decimal salary):base(name,surname,con)
        {
            ID = id;
            HireDate = hireDate;
            Occupation = occupation;
            Salary = salary;
        }
        public uint GetID() { return ID; }
        public string OutputData()
        {
            return ($"ID: {ID}\nName: {Name} {Surname}\nHired: {HireDate}\nOccupation: {Occupation}\nSalary: {Salary}");
        }
        public override string ToString()
        {
            return ($"ID: {ID} Name: {Name} {Surname}");
        }
    }
}