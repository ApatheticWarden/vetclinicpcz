using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicConsole
{
    internal class Record
    {
        public DateTime Date { get; set; }
        public Client ClientPerson { get; set; }
        public Animal ClientAnimal { get; set; }

        public Record(DateTime dt, Client cli, Animal clianim) {
            Date = dt;
            ClientPerson = cli;
            ClientAnimal = clianim;
        }
    }
}
