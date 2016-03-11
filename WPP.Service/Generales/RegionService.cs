using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects;
using WPP.Entities.Objects.Generales;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.Generales
{
    public class RegionService: IRegionService
    {
         private IRepository<Region> repository;
         public RegionService(IRepository<Region> _repository)
        {
            repository = _repository;
        }

         public Region Get(long id)
        {
            return repository.Get(id);
        }

        public Region Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }



        public IList<Region> GetAll(long nodoPadre)
        {
            IDictionary<string, object> criterias = new Dictionary<string, object>();
            criterias.Add("NodoPadre", nodoPadre);
            return repository.GetAll(criterias);


        }

     
    }
}
