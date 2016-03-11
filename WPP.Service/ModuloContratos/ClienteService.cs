using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloContratos
{
    public class ClienteService : IClienteService
    {
        private IRepository<Cliente> repository;

        public ClienteService(IRepository<Cliente> _repository)
        {
            repository = _repository;
        }

        public Cliente Get(long id)
        {
            return repository.Get(id);
        }

        public Cliente Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Cliente Create(Cliente entity)
        {
            repository.Add(entity);
            return entity;
        }

        public Cliente Update(Cliente entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(Cliente entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(Cliente item)
        {
            return repository.Contains(item);
        }

        public bool Contains(Cliente item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<Cliente> ListAll()
        {
            return repository.GetAll();
        }

        public IList<Cliente> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<Cliente> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Cliente>();
        }

        public IPagedList<Cliente> PagingSearch(String nombre, String nombreCorto, String grupo, String tipo, String numero, int page, int itemsPerPage, String sortOrder)
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

            if (!String.IsNullOrEmpty(numero))
                query = query.Where(u => u.Numero.ToString().Contains( numero));

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
                case "NumeroAsc":
                    return query.OrderBy(u => u.Numero).ToPagedList(page, itemsPerPage);
                case "NumeroDesc":
                    return query.OrderByDescending(u => u.Numero).ToPagedList(page, itemsPerPage);
                default:
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);

            }

        }


        public IList<Cliente> ClienteSearch(String cliente)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(cliente))
            {
                long num = 0;
                if (long.TryParse(cliente, out num))
                {
                    query = query.Where(u => u.Nombre.ToUpper().Contains(cliente.ToUpper()) || u.NombreCorto.ToUpper().Contains(cliente.ToUpper()) || u.Numero == num);
                }
                else
                {
                    query = query.Where(u => u.Nombre.ToUpper().Contains(cliente.ToUpper()) || u.NombreCorto.ToUpper().Contains(cliente.ToUpper()));
                }
            }
            return query.ToList<Cliente>();
        }

    }
}
