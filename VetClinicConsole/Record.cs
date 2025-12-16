using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VetClinicConsole
{
    internal class Record
    {
        public uint Id { get; set; }
        public uint ClientId { get; set; }
        public uint ClientAnimalId { get; set; }
        public string Date { get; set; }
        public string Reason { get; set; }

        public Record(uint id, uint _clid, uint _anm_id, string date, string rsn) {
            Id = id;
            ClientId = _clid;
            ClientAnimalId = _anm_id;
            Date = date;
            Reason = rsn;
        }
        public uint GetID() {
            return Id;
        }
        public string OutputData() {
            return ($"ID: {Id}\nClient ID: {ClientId}\nAnimal ID: {ClientAnimalId}\nDate: {Date}\nReason: {Reason}");
        }
        public override string ToString() {
            return $"Record ID: {Id} | Date: {Date} | Reason: {Reason}";
        }
    }
}
