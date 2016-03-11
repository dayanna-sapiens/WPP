using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.Generales
{
    public interface IRegionService
    {
        Region Get(long id);
        Region Get(IDictionary<string, object> criterias);
        IList<Region> GetAll(long nodoPadre);

    }
}
