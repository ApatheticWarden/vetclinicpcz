using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicConsole
{
    internal class Owner : Client
    {
        public List<Animal> Animals;
        public Owner(int id, string nm, string snm, Dictionary<string, string> c) : base(id, nm, snm, c)
        {
            Animals = new List<Animal>();
        }
        public void AddAnimal(Animal animal) {
            Animals.Add(animal);
        }
        public void RemoveAnimal(Animal animal)
        {
            Animals.Remove(animal);
        }
        public override string ToString()
        {
            return $"Owner info:\n {Name} {Surname}\n Contacts: {Contacts} \n Animals: {Animals}";
        }
    }
}
