using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloFacturacion
{
    public class FacturacionService : IFacturacionService
    {
         private IRepository<Facturacion> repository;

        public FacturacionService(IRepository<Facturacion> _repository)
        {
            repository = _repository;
        }

        public Facturacion Get(long id)
        {
            return repository.Get(id);
        }

        public Facturacion Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public Facturacion Create(Facturacion entity)
        {
            repository.Add(entity);
            return entity;
        }

        public Facturacion Update(Facturacion entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(Facturacion entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(Facturacion item)
        {
            return repository.Contains(item);
        }

        public bool Contains(Facturacion item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<Facturacion> ListAll()
        {
            return repository.GetAll();
        }

        public IList<Facturacion> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<Facturacion> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<Facturacion>();
        }

        public IPagedList<Facturacion> PagingSearch(String nombre, long companiaId, String activo, int page, int itemsPerPage, String sortOrder)
        {
            //var query = repository.GetQuery();

            //query = query.Where(u => u.IsDeleted == false);

            //query = query.Where(u => u.Compania.Id == companiaId);

            //if (activo == "Activo")
            //    query = query.Where(u => u.Activo == true);
            //else if(activo == "Inactivo")
            //    query = query.Where(u => u.Activo == false);
            
                

            //if (!String.IsNullOrEmpty(nombre))
            //    query = query.Where(u => u.Nombre.ToString().Contains(nombre));


            //switch (sortOrder)
            //{
            //    case "NombreAsc":
            //        return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);
            //    case "NombreDesc":
            //        return query.OrderByDescending(u => u.Nombre).ToPagedList(page, itemsPerPage);
            //     default:
            //        return query.OrderBy(u => u.Nombre).ToPagedList(page, itemsPerPage);

            //}
            return null;
        }

        public IList<Facturacion> PrefacturaSearch(long cliente, long compania)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);
            query = query.Where(u => u.Compania.Id == compania);
            query = query.Where(u => u.Estado == "Prefacturación");

            if (cliente != 0 )
            {
                query = query.Where(u => u.Contrato.Cliente.Id == cliente);
            }

            return query.ToList<Facturacion>();
        }

        public IList<Facturacion> DocumentosSearch(string desde, string hasta, string numeroCliente, string cliente, string numeroContrato, string contrato, long compania)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);
            query = query.Where(u => u.Compania.Id == compania);
            query = query.Where(u => u.Estado == "Facturación");

            if (!String.IsNullOrEmpty(desde))
            {
                query = query.Where(u => u.FechaDesde >= Convert.ToDateTime(desde));
            }

            if (!String.IsNullOrEmpty(hasta))
            {
                query = query.Where(u => u.FechaHasta <= Convert.ToDateTime(hasta));
            }

            if (!String.IsNullOrEmpty(numeroCliente))
            {
                query = query.Where(u => u.Contrato.Cliente.Id == Convert.ToInt64(numeroCliente));
            }

            if (!String.IsNullOrEmpty(cliente))
            {
                query = query.Where(u => u.Contrato.Cliente.Nombre.ToUpper().Contains(cliente.ToUpper()));
            }

            if (!String.IsNullOrEmpty(numeroContrato))
            {
                query = query.Where(u => u.Contrato.Id == Convert.ToInt64(numeroContrato));
            }

            if (!String.IsNullOrEmpty(contrato))
            {
                query = query.Where(u => u.Contrato.DescripcionContrato.ToUpper().Contains(contrato.ToUpper()));
            }

            return query.ToList<Facturacion>();
        }

        public IList<Facturacion> FacturacionSearch(string cliente, string numCliente, string contrato, string numContrato, long compania)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);
            query = query.Where(u => u.Compania.Id == compania);
            query = query.Where(u => u.Estado == "Prefacturación");

            if (!String.IsNullOrEmpty(numCliente))
            {
                query = query.Where(u => u.Contrato.Cliente.Id == Convert.ToInt64(numCliente));
            }

            if (!String.IsNullOrEmpty(cliente))
            {
                query = query.Where(u => u.Contrato.Cliente.Nombre.ToUpper().Contains(cliente.ToUpper()));
            }

            if (!String.IsNullOrEmpty(numContrato))
            {
                query = query.Where(u => u.Contrato.Id == Convert.ToInt64(numContrato));
            }

            if (!String.IsNullOrEmpty(contrato))
            {
                query = query.Where(u => u.Contrato.DescripcionContrato.ToUpper().Contains(contrato.ToUpper()));
            }

            return query.ToList<Facturacion>();
        }

        public IList<Contrato> FiltrarContratosXEmpleadoFacturados(string desde, string hasta, string cliente, string ejecutivo, string estado, string compania)
        {
            var query = repository.GetQuery();
            query = query.Where(u => u.IsDeleted == false);

            if (!String.IsNullOrEmpty(compania) && compania != "Todos")
            {
                query = query.Where(u => u.Compania.Id == Convert.ToInt64(compania));
            }

            var queryAgrupado = query.GroupBy(u => u.Contrato.Id).ToList();
            var queryContratos = queryAgrupado.Select(s => s.FirstOrDefault()).ToList();
            var ListaContratos = queryContratos.Select(s => s.Contrato).ToList();

            if (estado != "Todos")
            {
                ListaContratos = ListaContratos.Where(m => m.Estado == estado).ToList();
            }
            if (cliente != String.Empty)
            {
                ListaContratos = ListaContratos.Where(s => s.Cliente.Nombre.ToUpper().Contains(cliente.ToUpper())).ToList();
            }

            if (ejecutivo != "Todos")
            {
                ListaContratos = ListaContratos.Where(s => s.Ejecutivo.Id == Convert.ToInt64(ejecutivo)).ToList();
            }


            return ListaContratos;
        }
    }
}
