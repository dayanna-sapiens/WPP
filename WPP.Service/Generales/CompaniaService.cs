using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class CompaniaService : ICompaniaService
    {
        private IRepository<Compania> repository;
        private IRepository<Cliente> clienteRepository;

        public CompaniaService(IRepository<Compania> _repository, IRepository<Cliente> _repositoryCliente)
        {
            repository = _repository;
            clienteRepository = _repositoryCliente;
        }

        public Compania Get(long id)
        {
            return repository.Get(id);
        }

        public Compania Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Compania Create(Compania entity)
        {
            repository.Add(entity);           
            return entity;
        }

        public Compania Update(Compania entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(Compania entity)
        {
            repository.Remove(entity);
        }

        public bool Contains(Compania item)
        {
            return repository.Contains(item);
        }

        public bool Contains(Compania item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<Compania> ListAll()
        {
            return repository.GetAll();
        }

        public IList<Compania> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<Compania> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Compania>();
        }

        public IPagedList<Compania> PagingSearch(String nombre, String nombreCorto, String grupo, String tipo, int page, int itemsPerPage, String sortOrder)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);
            
            if (!String.IsNullOrEmpty(nombre))
                query = query.Where(u => u.Nombre.ToUpper().Contains(nombre.ToUpper()));

            if (!String.IsNullOrEmpty(nombreCorto))
                query = query.Where(u => u.NombreCorto.ToUpper().Contains(nombreCorto.ToUpper()));

            if (!String.IsNullOrEmpty(grupo))
                query = query.Where(u => u.Grupo.Nombre.ToUpper().Contains(grupo.ToUpper()));

            if (!String.IsNullOrEmpty(tipo))
                query = query.Where(u => u.Tipo.Nombre.ToUpper().Contains(tipo.ToUpper()));

           switch (sortOrder)
            {
                case "NombreAsc":
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "NombreDesc":
                    return query.OrderByDescending(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "NombreCortoAsc":
                    return query.OrderBy(u => u.NombreCorto).ToPagedList(page, itemsPerPage);
                case "NombreCortoDesc":
                    return query.OrderByDescending(u => u.NombreCorto).ToPagedList(page, itemsPerPage);
                case "GrupoAsc":
                    return query.OrderBy(u => u.Grupo.Nombre).ToPagedList(page, itemsPerPage);
                case "GrupoDesc":
                    return query.OrderByDescending(u => u.Grupo.Nombre).ToPagedList(page, itemsPerPage);
                case "TipoAsc":
                    return query.OrderBy(u => u.Tipo.Nombre).ToPagedList(page, itemsPerPage);
                case "TipoDesc":
                    return query.OrderByDescending(u => u.Tipo.Nombre).ToPagedList(page, itemsPerPage);
                default:
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);

            }

        }



    }
}


