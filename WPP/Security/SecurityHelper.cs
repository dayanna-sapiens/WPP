using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPP.Entities.Objects.Generales;
using WPP.Service.ModuloContratos;

namespace WPP.Security
{
    public class SecurityHelper
    {
        private IUsuarioService usuarioService;

        public SecurityHelper(IUsuarioService service)
        {
            this.usuarioService = service;
        }

        public Usuario CurrentUser(String email, String password)
        {
            IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
            criteriaUser.Add("Email", email);
            criteriaUser.Add("Password", password);

            Usuario usuario = usuarioService.Get(criteriaUser);

            return usuario;
        }

        public Usuario CurrentUser(String username)
        {
            IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
            criteriaUser.Add("Email", username);

            Usuario usuario = usuarioService.Get(criteriaUser);

            return usuario;
        }
    }
}