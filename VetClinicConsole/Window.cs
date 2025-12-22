using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicConsole
{
    public abstract class Window
    {
        public abstract Window Run(Database _db);
    }
}
