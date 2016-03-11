using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.ModuloContratos
{
    public interface IProductoContratoService
    {
        ProductoContrato Get(long id);
        ProductoContrato Get(IDictionary<string, object> criterias);
        ProductoContrato Create(ProductoContrato entity);
        ProductoContrato Update(ProductoContrato entity);
        void Delete(ProductoContrato entity);
        bool Contains(ProductoContrato item);
        bool Contains(ProductoContrato item, string property, object value);
        IList<ProductoContrato> ListAll();
        IList<ProductoContrato> GetAll(IDictionary<string, object> criterias);
        IList<ProductoContrato> GetAll(IDictionary<string, object> criterias, string property, DateTime startDate, DateTime endDate);
        int Count();

    }
}
