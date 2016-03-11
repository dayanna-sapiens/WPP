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
    public class EquipoService : IEquipoService
    {
        private IRepository<Equipo> repository;

        public EquipoService(IRepository<Equipo> _repository)
        {
            repository = _repository;
        }

        public Equipo Get(long id)
        {
            return repository.Get(id);
        }

        public Equipo Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Equipo Create(Equipo entity)
        {
            repository.Add(entity);
            return entity;
        }
        public Equipo Update(Equipo entity)
        {
            repository.Update(entity);
            return entity;
        }
        public void Delete(Equipo entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }
        public bool Contains(Equipo item)
        {
            return repository.Contains(item);
        }
        public bool Contains(Equipo item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }
        public IList<Equipo> ListAll()
        {
            return repository.GetAll();
        }
        public IList<Equipo> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }
       
        public int Count()
        {
            return repository.Count<Equipo>();
        }

        public IPagedList<Equipo> PagingSearch(String nombre, String placa, String marca, 
                                                String tipo, int page, int itemsPerPage, String sortOrder, long companiaId)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);
            query = query.Where(u => u.Compania.Id == companiaId);

            if (!String.IsNullOrEmpty(nombre))
                query = query.Where(u => u.Nombre.ToUpper().Contains(nombre.ToUpper()));

            if (!String.IsNullOrEmpty(placa))
                query = query.Where(u => u.Placa.ToUpper().Contains(placa.ToUpper()));

            if (!String.IsNullOrEmpty(marca))
                query = query.Where(u => u.Marca.ToUpper().Contains(marca.ToUpper()));

            if (!String.IsNullOrEmpty(tipo) && tipo != "Todos")
                query = query.Where(u => u.Tipo == tipo);


            switch (sortOrder)
            {
                case "NombreAsc":
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "NombreDesc":
                    return query.OrderByDescending(u => u.Nombre).ToPagedList(page, itemsPerPage);
                case "PlacaAsc":
                    return query.OrderBy(u => u.Placa).ToPagedList(page, itemsPerPage);
                case "PlacaDesc":
                    return query.OrderByDescending(u => u.Placa).ToPagedList(page, itemsPerPage);
                case "MarcaAsc":
                    return query.OrderBy(u => u.Marca).ToPagedList(page, itemsPerPage);
                case "MarcaDesc":
                    return query.OrderByDescending(u => u.Marca).ToPagedList(page, itemsPerPage);
                default:
                    return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);

            }
        }

        public IList<Equipo> EquipoSearch(String equipo)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(equipo))
            {
                query = query.Where(u => u.Nombre.ToUpper().Contains(equipo.ToUpper()));
            }

            return query.ToList<Equipo>();
        }
    }
}
