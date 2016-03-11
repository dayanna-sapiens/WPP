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
    public class ContratoService : IContratoService
    {
        private IRepository<Contrato> repository;

        public ContratoService(IRepository<Contrato> _repository)
        {
            repository = _repository;
        }

        public Contrato Get(long id)
        {
            return repository.Get(id);
        }

        public Contrato Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Contrato Create(Contrato entity)
        {
            repository.Add(entity);
            return entity;
        }

        public Contrato Update(Contrato entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(Contrato entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(Contrato item)
        {
            return repository.Contains(item);
        }

        public bool Contains(Contrato item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<Contrato> ListAll()
        {
            return repository.GetAll();
        }

        public IList<Contrato> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<Contrato> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Contrato>();
        }

       
        public IPagedList<Contrato> PagingSearch(String clienteNumero, String clienteDescripcion, String estado, String contrato, String contratoDescripcion, String puntoFacturacion, int page, int itemsPerPage, String sortOrder, long compania)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false && u.Compania.Id == compania);

            if (!String.IsNullOrEmpty(clienteDescripcion))
                query = query.Where(u => u.Cliente.Nombre.ToUpper().Contains(clienteDescripcion.ToUpper()));

            if (!String.IsNullOrEmpty(estado))
                query = query.Where(u => u.Estado.ToUpper().Contains(estado.ToUpper()));

            if (!String.IsNullOrEmpty(clienteNumero))
                query = query.Where(u => u.Cliente.Numero == Convert.ToInt64(clienteNumero));

            if (!String.IsNullOrEmpty(contrato))
                query = query.Where(u => u.Numero == Convert.ToInt64(contrato));

            if (!String.IsNullOrEmpty(puntoFacturacion))
                query = query.Where(u => u.PuntoFacturacion.Nombre.ToUpper().Contains(puntoFacturacion.ToUpper()));

            if (!String.IsNullOrEmpty(contratoDescripcion))
                query = query.Where(u => u.DescripcionContrato.ToUpper().Contains(contratoDescripcion.ToUpper()));

            switch (sortOrder)
            {
                case "ClienteDescripcionAsc":
                    return query.OrderBy(u => u.Cliente.Nombre).ToPagedList(page, itemsPerPage);
                case "ClienteDescripcionDesc":
                    return query.OrderByDescending(u => u.Cliente.Nombre).ToPagedList(page, itemsPerPage);
                case "ClienteNumeroAsc":
                    return query.OrderBy(u => u.Cliente.Numero).ToPagedList(page, itemsPerPage);
                case "ClienteNumeroDesc":
                    return query.OrderByDescending(u => u.Cliente.Numero).ToPagedList(page, itemsPerPage);
                case "EstadoAsc":
                    return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "EstadoDesc":
                    return query.OrderByDescending(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "ContratoAsc":
                    return query.OrderBy(u => u.Numero).ToPagedList(page, itemsPerPage);
                case "ContratoDesc":
                    return query.OrderByDescending(u => u.Numero).ToPagedList(page, itemsPerPage);
                case "PuntoFacturacionAsc":
                    return query.OrderBy(u => u.PuntoFacturacion.Nombre).ToPagedList(page, itemsPerPage);
                case "PuntoFacturacionDesc":
                    return query.OrderByDescending(u => u.PuntoFacturacion.Nombre).ToPagedList(page, itemsPerPage);
                case "DescripcionContratoAsc":
                    return query.OrderBy(u => u.DescripcionContrato).ToPagedList(page, itemsPerPage);
                case "DescripcionContratoDesc":
                    return query.OrderByDescending(u => u.DescripcionContrato).ToPagedList(page, itemsPerPage);
                default:
                    return query.OrderBy(u => u.Cliente.Nombre).ToPagedList(page, itemsPerPage);

            }

        }

        public IList<Contrato> Filtrar(string busqueda, string Compania, string tipo)
        {
            var query = repository.GetQuery();
            query = query.Where(u => u.IsDeleted == false && u.Estado != "Inactivo");
                
            if (!String.IsNullOrEmpty(Compania))
            {
                query = query.Where(u => u.Compania.Id == Convert.ToInt64(Compania));
            }

            if (!String.IsNullOrEmpty(tipo))
            {
                query = query.Where(u => u.Cliente.Tipo.Nombre.ToUpper().Contains(tipo.ToUpper()));
            }

            if (!String.IsNullOrEmpty(busqueda))
            {
                int num;
                if (int.TryParse(busqueda, out num))
                {
                    query = query.Where(u => u.Cliente.Numero == long.Parse(busqueda) || u.Numero == long.Parse(busqueda) 
                        || u.Cliente.Nombre.ToUpper().Contains(busqueda.ToUpper()) || u.DescripcionContrato.ToUpper().Contains(busqueda.ToUpper())); 
                }
                else
                {
                    query = query.Where(u => u.Cliente.Nombre.ToUpper().Contains(busqueda.ToUpper()) || u.DescripcionContrato.ToUpper().Contains(busqueda.ToUpper()));
                }
                
            }
            else
            {
               return null;
            }

            return query.ToList<Contrato>();
        }


     


    }
}
