using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloNomina;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloNomina
{
    public class PlanillaService : IPlanillaService
    {
        private IRepository<Planilla> repository;

        public PlanillaService(IRepository<Planilla> _repository)
        {
            repository = _repository;
        }

        public Planilla Get(long id)
        {
            return repository.Get(id);
        }

        public Planilla Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Planilla Create(Planilla entity)
        {
            repository.Add(entity);
            return entity;
        }

        public Planilla Update(Planilla entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(Planilla entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(Planilla item)
        {
            return repository.Contains(item);
        }

        public bool Contains(Planilla item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<Planilla> ListAll()
        {
            return repository.GetAll();
        }

        public IList<Planilla> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<Planilla> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Planilla>();
        }

        //public IPagedList<Planilla> PagingSearch(long consecutivo, string fechaInicio, string fechaFin, String estado, long compania, int page, int itemsPerPage, String sortOrder)
        //{
        //    //var query = repository.GetQuery();

        //    //query = query.Where(u => u.IsDeleted == false);

        //    //query = query.Where(u => u.Compania.Id == compania);
                     
        //    //if (!String.IsNullOrEmpty(estado))
        //    //    query = query.Where(u => u.Estado.ToString().Contains(estado));

        //    //if (consecutivo != 0 && consecutivo != null)
        //    //    query = query.Where(u => u.Consecutivo == consecutivo);

        //    //if (!String.IsNullOrEmpty(fechaInicio) || !String.IsNullOrEmpty(fechaFin))
        //    //{
        //    //    DateTime Inicio = Convert.ToDateTime(fechaInicio);
        //    //    DateTime Fin = Convert.ToDateTime(fechaFin);

        //    //    query = query.Where(u => u.FechaInicio.AddDays(-1) <= Inicio && u.FechaFin.AddDays(1) >= Fin );
        //    //}

        //    //switch (sortOrder)
        //    //{
        //    //    case "ConsecutivoAsc":
        //    //        return query.OrderBy(u => u.Consecutivo).ToPagedList(page, itemsPerPage);
        //    //    case "ConsecutivoDesc":
        //    //        return query.OrderByDescending(u => u.Consecutivo).ToPagedList(page, itemsPerPage);
        //    //    case "EstadoAsc":
        //    //        return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);
        //    //    case "EstadoDesc":
        //    //        return query.OrderByDescending(u => u.Estado).ToPagedList(page, itemsPerPage);
        //    //    case "FechaInicioAsc":
        //    //        return query.OrderBy(u => u.FechaInicio).ToPagedList(page, itemsPerPage);
        //    //    case "FechaInicioDesc":
        //    //        return query.OrderByDescending(u => u.FechaInicio).ToPagedList(page, itemsPerPage);
        //    //    case "FechaFinAsc":
        //    //        return query.OrderBy(u => u.FechaFin).ToPagedList(page, itemsPerPage);
        //    //    case "FechaFinDesc":
        //    //        return query.OrderByDescending(u => u.FechaFin).ToPagedList(page, itemsPerPage);
        //    //    default:
        //    //        return query.OrderBy(u => u.Consecutivo).ToPagedList(page, itemsPerPage);

        //    //}
        //    return null;
        //}
   }
}
