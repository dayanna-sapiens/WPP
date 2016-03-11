using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WPP.Entities.Objects.Generales;
using WPP.Helpers;
using WPP.Service.ModuloContratos;
using WPP.Service.Generales;

namespace WPP.Security
{
    public class WPPMembershipProvider : IWPPMembershipProvider
    {
        private IUsuarioService usuarioService;


        public WPPMembershipProvider(IUsuarioService service)
        {
            try
            {
                this.usuarioService = service;
            }
            catch (Exception ex)
            {

            }
        }

        public string ApplicationName
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

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Valida que el nombre de usuario y contraseña sean correctos
        /// </summary>
        /// <param name="username">nombre de usuario</param>
        /// <param name="password">contraseña</param>
        /// <returns>true si es válido o false si no lo es</returns>
        public bool ValidateUser(string username, string password)
        {
            IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
            criteriaUser.Add("Email", username);
            //criteriaUser.Add("Password", password);
            criteriaUser.Add("IsDeleted", false);

            Usuario usuario = usuarioService.Get(criteriaUser);

            if (usuario != null)
            {
                if (usuario.PasswordActivo)
                {
                    return WPPHelper.VerifyHash(password, usuario.Password);
                }
                else
                {
                    return false;
                }
            }
            else
            { return false; }           
        }
    }
}