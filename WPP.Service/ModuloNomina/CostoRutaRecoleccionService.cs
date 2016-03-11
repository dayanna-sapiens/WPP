using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloNomina
{
    public class CostoRutaRecoleccionService : ICostoRutaRecoleccionService
    {
         private IRepository<CostoRutaRecoleccion> repository;

         public CostoRutaRecoleccionService(IRepository<CostoRutaRecoleccion> repository)
        {
            this.repository = repository;
        }

         public CostoRutaRecoleccion Get(long id)
        {
            return repository.Get(id);
        }

         public CostoRutaRecoleccion Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

         public CostoRutaRecoleccion Create(CostoRutaRecoleccion entity)
        {
            repository.Add(entity);
            return entity;
        }
         public CostoRutaRecoleccion Update(CostoRutaRecoleccion entity)
        {
            repository.Update(entity);
            return entity;
        }
         public void Delete(CostoRutaRecoleccion entity)
        {
            repository.Remove(entity);
        }

         public IList<CostoRutaRecoleccion> ListAll()
        {
            return repository.GetAll();
        }

         public IList<CostoRutaRecoleccion> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

    }
}
