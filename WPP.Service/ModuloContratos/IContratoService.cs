using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.ModuloContratos
{
    public interface IContratoService
    {
        Contrato Get(long id);
        Contrato Get(IDictionary<string, object> criterias);
        Contrato Create(Contrato entity);
        Contrato Update(Contrato entity);
        void Delete(Contrato entity);

        IList<Contrato> ListAll();
        IList<Contrato> GetAll(IDictionary<string, object> criterias);
        IList<Contrato> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();

        IPagedList<Contrato> PagingSearch(String clienteNumero, String clienteDescripcion, String estado, String contrato, String contratoDescripcion, String puntoFacturacion, int page, int itemsPerPage, String sortOrder, long compania);

        IList<Contrato> Filtrar(string busqueda, string Compania, string tipo);

        
    }
}
