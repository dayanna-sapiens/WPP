using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;

namespace WPP.Service.ModuloNomina
{
    public interface ICostoRutaRecoleccionService
    {
        CostoRutaRecoleccion Get(long id);
        CostoRutaRecoleccion Get(IDictionary<string, object> criterias);
        CostoRutaRecoleccion Create(CostoRutaRecoleccion entity);
        CostoRutaRecoleccion Update(CostoRutaRecoleccion entity);
        IList<CostoRutaRecoleccion> ListAll();
        IList<CostoRutaRecoleccion> GetAll(IDictionary<string, object> criterias);
        void Delete(CostoRutaRecoleccion entity);

    }
}
