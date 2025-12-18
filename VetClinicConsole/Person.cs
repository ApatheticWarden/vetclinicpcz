using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicConsole
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Dictionary<string, string>? Contacts { get; set; }

        public Person(string name, string surname, Dictionary<string, string>? con) {
            Name = name;
            Surname = surname;
            Contacts = con;
        }

        public override string ToString()
        {
            return
                $"Name: {Name} | Surname: {Surname}\n" +
                $"Contacts:\n" +
                string.Join("\n", Contacts.Select(c => $"{c.Key}: {c.Value}"));
        }
    }
}
