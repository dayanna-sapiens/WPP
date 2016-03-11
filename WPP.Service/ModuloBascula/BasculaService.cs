using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloBascula
{
    public class BasculaService : IBasculaService
    {
         private IRepository<Bascula> repository;

        public BasculaService(IRepository<Bascula> _repository)
        {
            repository = _repository;
        }

        public Bascula Get(long id)
        {
            return repository.Get(id);
        }

        public Bascula Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Bascula Create(Bascula entity)
        {
            repository.Add(entity);
            return entity;
        }

        public Bascula Update(Bascula entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(Bascula entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(Bascula item)
        {
            return repository.Contains(item);
        }

        public bool Contains(Bascula item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IEnumerable<Bascula> ListAll()
        {
            return repository.GetAll();
        }

        public IList<Bascula> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<Bascula> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Bascula>();
        }


        public IPagedList<Bascula> PagingSearch(String boleta, String cliente, String equipo, String estado, long compania, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);
            query = query.Where(u => u.Compania.Id == compania);

            if (!String.IsNullOrEmpty(boleta))
                query = query.Where(u => u.Boleta == Convert.ToInt64(boleta));

            if (!String.IsNullOrEmpty(estado))
                query = query.Where(u => u.Estado.ToUpper().Contains(estado.ToUpper()));

            if (!String.IsNullOrEmpty(cliente))
                query = query.Where(u => u.Contrato.Cliente.Nombre == cliente);

            if (!String.IsNullOrEmpty(equipo))
                query = query.Where(u => u.Equipo.Nombre == equipo);

            
            switch (sortOrder)
            {
                case "BoletaAsc":
                    return query.OrderBy(u => u.Boleta).ToPagedList(page, itemsPerPage);
                case "BoletaDesc":
                    return query.OrderByDescending(u => u.Boleta).ToPagedList(page, itemsPerPage);
                case "ClienteAsc":
                    return query.OrderBy(u => u.Contrato.Cliente.Nombre).ToPagedList(page, itemsPerPage);
                case "ClienteDesc":
                    return query.OrderByDescending(u => u.Contrato.Cliente.Nombre).ToPagedList(page, itemsPerPage);
                case "EstadoAsc":
                    return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "EstadoDesc":
                    return query.OrderByDescending(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "EquipoAsc":
                    return query.OrderBy(u => u.Equipo.Nombre).ToPagedList(page, itemsPerPage);
                case "EquipoDesc":
                    return query.OrderByDescending(u => u.Equipo.Nombre).ToPagedList(page, itemsPerPage);                
                default:
                    return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);

            }

        }

        public IList<Bascula> BasculaSearch(long contrato, DateTime hasta, DateTime desde, long compania)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);
            query = query.Where(u => u.Compania.Id == compania);
            query = query.Where(u => u.Estado != "Anulada");

            if (contrato != 0 && contrato != null)
            {
                query = query.Where(u => u.Contrato.Id == contrato);
            }

            if (hasta != null && desde != null)
            {
                query = query.Where(u => u.Fecha >= desde && u.Fecha <= hasta);
            }

            return query.ToList<Bascula>();
        }
        
    }
}
