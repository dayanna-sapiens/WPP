using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.Generales
{
    public  interface IContactoCompaniaService 
    {
        ContactoCompania Get(long id);
        ContactoCompania Get(IDictionary<string, object> criterias);
        IList<ContactoCompania> GetAll(long nodoPadre);
    }
}
