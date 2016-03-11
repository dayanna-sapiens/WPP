using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WPP.Entities.Objects.Generales;
using WPP.Helpers;
using WPP.Service.ModuloContratos;

namespace WPP.Security
{
    public class WPPRolesProvider : RoleProvider
    {
         private IUsuarioService usuarioService;
        
        public WPPRolesProvider()
        {
            usuarioService = DependencyResolver.Current.GetService<IUsuarioService>();
        }

        
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene los roles del usuario 
        /// </summary>
        /// <param name="username">nombre de usuario</param>
        /// <returns>los roles asignados al usuario</returns>
        public override string[] GetRolesForUser(string username)
        {
            IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
            criteriaUser.Add("Email", username);
            Usuario usuario = usuarioService.Get(criteriaUser);

            if (usuario != null)
                return usuario.Roles.Split(',');
            else
            {
                String[] empty = new String[1];
                empty[0] = "";
                return empty;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}