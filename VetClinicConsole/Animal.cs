using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VetClinicConsole
{
    internal abstract class Animal
    {
        public uint ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public int Age { get; set; }
        public decimal? Weight { get; set; }
        public required Owner OwnerClient { get; set; }
        public string? Diagnosis { get; set; } = string.Empty;
        public string? Treatment { get; set; } = string.Empty;
        public DateTime? LastVisit { get; set; }
        public virtual string OutputData()
        {
            return $"Animal ID: {ID}\n" +
                   $"Name: {Name}\n" +
                   $"Species: {Species}\n" +
                   $"Age: {Age}\n" +
                   $"Weight: {Weight} kg\n" +
                   $"Owner: {OwnerClient.Name} {OwnerClient.Surname}\n" +
                   $"Diagnosis: {Diagnosis}\n" +
                   $"Treatment: {Treatment}\n" +
                   $"Last visit: {LastVisit:dd/MM/yyyy}";
        }
        public override string ToString()
        {
            return OutputData();
        }
    }
}