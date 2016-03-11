using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloFacturacion;

namespace WPP.Service.ModuloFacturacion
{
    public interface IFacturacionService
    {
        Facturacion Get(long id);
        Facturacion Get(IDictionary<string, object> criterias);
        Facturacion Create(Facturacion entity);
        Facturacion Update(Facturacion entity);
        void Delete(Facturacion entity);
        bool Contains(Facturacion item);
        bool Contains(Facturacion item, string property, object value);
        IList<Facturacion> ListAll();
        IList<Facturacion> GetAll(IDictionary<string, object> criterias);
        int Count();

        IPagedList<Facturacion> PagingSearch(String nombre, long compania, String activo, int page, int itemsPerPage, String sortOrder);
        IList<Facturacion> PrefacturaSearch(long cliente, long compania);
        IList<Facturacion> DocumentosSearch(string desde, string hasta, string numeroCliente, string cliente, string numeroContrato, string contrato, long compania);

        IList<Facturacion> FacturacionSearch(string cliente, string numCliente, string contrato, string numContrato, long compania);

        IList<Contrato> FiltrarContratosXEmpleadoFacturados(string desde, string hasta, string cliente, string ejecutivo, string estado, string compania);
        
    }
}
