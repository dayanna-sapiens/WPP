using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Model;


namespace WPP.Mapper
{
    public class UsuarioMapper
    {
        // Convierte Entidad en Modelo
        public UsuarioModel GetUsuarioModel(Usuario usuario)
        {
            AutoMapper.Mapper.CreateMap<Usuario, UsuarioModel>()
            .ForMember(dest => dest.NombreCompleto, opts => opts.MapFrom(src => src.Nombre + " " + src.Apellido1 + " " + src.Apellido2));
            UsuarioModel usuarioModelo = AutoMapper.Mapper.Map<UsuarioModel>(usuario);
            return usuarioModelo;
        }

        // Convierte Modelo en Entidad
        public Usuario GetUsuario(UsuarioModel usuarioModelo, Usuario usuario)
        {
            AutoMapper.Mapper.CreateMap<UsuarioModel, Usuario>()
                .ForMember(dest => dest.Companias, opts => opts.Ignore());
            AutoMapper.Mapper.Map<UsuarioModel, Usuario>(usuarioModelo, usuario);
            return usuario;
        }

        // Convierte la Lista de Entidad en Lista de Modelo
        public IList<UsuarioModel> GetListaUsuarioModel(IList<Usuario> ListaUsuario)
        {
            AutoMapper.Mapper.CreateMap<Usuario, UsuarioModel>()
            .ForMember(dest => dest.NombreCompleto, opts => opts.MapFrom(src => src.Nombre + " " + src.Apellido1 + " " + src.Apellido2));
            IList<UsuarioModel> listaUsuarioModel = AutoMapper.Mapper.Map<IList<UsuarioModel>>(ListaUsuario);
            return listaUsuarioModel;
        }
    }
}
