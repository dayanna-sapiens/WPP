using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Model.ModuloContratos;

namespace WPP.Mapper.ModuloContratos
{
    public class CategoriaProductoMapper
    {
        // Convierte Entidad en Modelo
        public CategoriaProductoModel GetCategoriaProductoModel(CategoriaProducto categoriaProducto)
        {
            AutoMapper.Mapper.CreateMap<CategoriaProducto, CategoriaProductoModel>();
            CategoriaProductoModel categoriaProductoModelo = AutoMapper.Mapper.Map<CategoriaProductoModel>(categoriaProducto);
            return categoriaProductoModelo;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<CategoriaProductoModel> GetListaCategoriaProductoModel(IList<CategoriaProducto> ListaCategoriaProducto)
        {
            AutoMapper.Mapper.CreateMap<CategoriaProducto, CategoriaProductoModel>();
            IList<CategoriaProductoModel> listaCategoriaProductoModel = AutoMapper.Mapper.Map<IList<CategoriaProductoModel>>(ListaCategoriaProducto);
            return listaCategoriaProductoModel;
        }

        // Convierte Modelo en Entidad
        public CategoriaProducto GetCategoriaProducto(CategoriaProductoModel categoriaProductoModelo, CategoriaProducto categoriaProducto)
        {
            AutoMapper.Mapper.CreateMap<CategoriaProductoModel, CategoriaProducto>();
            AutoMapper.Mapper.Map<CategoriaProductoModel, CategoriaProducto>(categoriaProductoModelo, categoriaProducto);
            return categoriaProducto;
        }
    }
}
