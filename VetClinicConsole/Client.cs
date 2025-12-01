using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicConsole
{
    internal class Client: Person
    {
        private int _id;
        public int Id { get => _id; }
        public Client(int id, string nm, string snm, Dictionary<string, string>? con):base(nm,snm,con) {
            _id = id;
        }

        public override string ToString()
        {
            return $"ID: {Id}" + base.ToString();
        }
    }
}
