using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.ModuloOperacionRecoleccion
{
    public class OTRService : IOTRService
    {
         private IRepository<OTR> repository;

        public OTRService(IRepository<OTR> _repository)
        {
            repository = _repository;
        }

        public OTR Get(long id)
        {
            return repository.Get(id);
        }

        public OTR Get(IDictionary<string, object> criterias)
        {
            return repository.Get(criterias);
        }

        public OTR Create(OTR entity)
        {
            repository.Add(entity);
            return entity;
        }

        public OTR Update(OTR entity)
        {
            repository.Update(entity);
            return entity;
        }

        public void Delete(OTR entity)
        {
            entity.IsDeleted = true;
            repository.Update(entity);
        }

        public bool Contains(OTR item)
        {
            return repository.Contains(item);
        }

        public bool Contains(OTR item, string property, object value)
        {
            return repository.Contains(item, property, value);
        }

        public IList<OTR> ListAll()
        {
            return repository.GetAll();
        }

        public IList<OTR> GetAll(IDictionary<string, object> criterias)
        {
            return repository.GetAll(criterias);
        }

        public IList<OTR> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate)
        {
            return repository.GetAll(criterias, property, startDate, endDate);
        }

        public int Count()
        {
            return repository.Count<OTR>();
        }

        public IPagedList<OTR> PagingSearch(String otr, String equipo, String estado, String rutaRecoleccion, String tipo, int page, int itemsPerPage, String sortOrder, long compania)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false && u.Compania.Id == compania);

            if (!String.IsNullOrEmpty(otr))
                query = query.Where(u => u.Consecutivo == Convert.ToInt64(otr));

            if (!String.IsNullOrEmpty(estado))
                query = query.Where(u => u.Estado.ToUpper().Contains(estado.ToUpper()));

            if (!String.IsNullOrEmpty(rutaRecoleccion))
                query = query.Where(u => u.RutaRecoleccion.Descripcion.ToUpper().Contains(rutaRecoleccion.ToUpper()));

            if (!String.IsNullOrEmpty(tipo))
                query = query.Where(u => u.Tipo.ToUpper().Contains(tipo.ToUpper()));
                      
            if (!String.IsNullOrEmpty(equipo))
                query = query.Where(u => u.Equipo.Nombre.ToString().Contains( equipo));

            switch (sortOrder)
            {
                case "RutaRecoleccionAsc":
                    return query.OrderBy(u => u.RutaRecoleccion.Descripcion).ToPagedList(page, itemsPerPage);
                case "RutaRecoleccionDesc":
                    return query.OrderByDescending(u => u.RutaRecoleccion.Descripcion).ToPagedList(page, itemsPerPage);
                case "EstadoAsc":
                    return query.OrderBy(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "EstadoDesc":
                    return query.OrderByDescending(u => u.Estado).ToPagedList(page, itemsPerPage);
                case "OTRAsc":
                    return query.OrderBy(u => u.Consecutivo).ToPagedList(page, itemsPerPage);
                case "OTRDesc":
                    return query.OrderByDescending(u => u.Consecutivo).ToPagedList(page, itemsPerPage);
                case "EquipoAsc":
                    return query.OrderBy(u => u.Equipo.Nombre).ToPagedList(page, itemsPerPage);
                case "EquipoDesc":
                    return query.OrderByDescending(u => u.Equipo.Nombre).ToPagedList(page, itemsPerPage);
                case "TipoAsc":
                    return query.OrderBy(u => u.Tipo).ToPagedList(page, itemsPerPage);
                case "TipoDesc":
                    return query.OrderByDescending(u => u.Tipo).ToPagedList(page, itemsPerPage);
                default:
                    return query.OrderBy(u => u.Consecutivo).ToPagedList(page, itemsPerPage);

            }

        }


        public IList<OTR> OTRSearch(long contrato, DateTime hasta, DateTime desde, long compania)
        {
            var query = repository.GetQuery();

            query = query.Where(u => u.IsDeleted == false);
            query = query.Where(u => u.Compania.Id == compania);

            IList<OTR> lista = new List<OTR>();
           
            if (hasta != null && desde != null)
            {
                query = query.Where(u => u.Fecha >= desde && u.Fecha <= hasta);
            }

            if (contrato != 0 && contrato != null)
            {
                var OTRs = query.ToList<OTR>();
                foreach (var item in OTRs)
                {
                    //if (item.OTRMadre)
                    //{
                    //    if (item.ListaOTRHijas.Count > 0)
                    //    {
                    //        foreach (var hijas in item.ListaOTRHijas)
                    //        {
                    //            if (hijas.RutaRecoleccion != null)
                    //            {
                    //                foreach (var ruta in hijas.RutaRecoleccion.Rutas)
                    //                {
                    //                    if (ruta.Contrato.Id == contrato)
                    //                    {
                    //                        lista.Add(item);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                        var rutas = item.RutaRecoleccion.Rutas;
                        foreach (var ruta in rutas)
                        {
                            if (ruta.Contrato.Id == contrato)
                            {
                                lista.Add(item);
                            }
                        }
                    //}
                }
            }

            return lista;
        }
    }
}
