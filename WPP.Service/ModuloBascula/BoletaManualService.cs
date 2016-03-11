using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloBoletaManual
{
    public class BoletaManualService : IBoletaManualService
    {
         private IRepository<BoletaManual> repository;

        public BoletaManualService(IRepository<BoletaManual> _repository)
        {
            repository = _repository;
        }

        public BoletaManual Get(long id)
        {
            return repository.Get(id);
        }

        public BoletaManual Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public BoletaManual Create(BoletaManual entity)
        {
            repository.Add(entity);
            return entity;
        }

        public BoletaManual Update(BoletaManual entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(BoletaManual entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(BoletaManual item)
        {
            return repository.Contains(item);
        }

        public bool Contains(BoletaManual item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<BoletaManual> ListAll()
        {
            return repository.GetAll();
        }

        public IList<BoletaManual> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<BoletaManual> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<BoletaManual>();
        }


        public IPagedList<BoletaManual> PagingSearch(String boleta, String otr, String equipo, String estado, long compania, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);
            query = query.Where(u => u.Compania.Id == compania);

            if (!String.IsNullOrEmpty(boleta))
                query = query.Where(u => u.NumeroBoleta == boleta);

            if (!String.IsNullOrEmpty(estado))
                query = query.Where(u => u.Estado.ToUpper().Contains(estado.ToUpper()));

            if (!String.IsNullOrEmpty(otr))
                query = query.Where(u => u.OTR.Consecutivo == Convert.ToInt64(otr));

            if (!String.IsNullOrEmpty(equipo))
                query = query.Where(u => u.OTR.Equipo.Placa == equipo);

            
            switch (sortOrder)
            {
                case "BoletaAsc":
                    return query.OrderBy(u => u.NumeroBoleta).ToPagedList(page, itemsPerPage);
                case "BoletaDesc":
                    return query.OrderByDescending(u => u.NumeroBoleta).ToPagedList(page, itemsPerPage);
                case "OTRAsc":
                    return query.OrderBy(u => u.OTR.Consecutivo).ToPagedList(page, itemsPerPage);
                case "OTRDesc":
                    return query.OrderByDescending(u => u.OTR.Consecutivo).ToPagedList(page, itemsPerPage);
                case "EstadoAsc":
                    return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "EstadoDesc":
                    return query.OrderByDescending(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "EquipoAsc":
                    return query.OrderBy(u => u.OTR.Equipo.Placa).ToPagedList(page, itemsPerPage);
                case "EquipoDesc":
                    return query.OrderByDescending(u => u.OTR.Equipo.Placa).ToPagedList(page, itemsPerPage);                
                default:
                    return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);

            }

        }

    }
}
