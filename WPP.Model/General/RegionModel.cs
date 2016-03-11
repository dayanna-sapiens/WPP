using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.General
{
    public class RegionModel
    {
        public long Id { get; set; }

        public String Nombre { get; set; }

        public long NodoPadre { get; set; }
    }
}
