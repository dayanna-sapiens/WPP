using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using WPP.Entities.Objects.Generales;

namespace WPP.Security
{
    public interface IWPPMembershipProvider 
    {

        bool ChangePassword(string username, string oldPassword, string newPassword);

        bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer);
        
        MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status);
    
        bool DeleteUser(string username, bool deleteAllRelatedData);

        MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);

        MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);

        MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords);

        int GetNumberOfUsersOnline();

        string GetPassword(string username, string answer);

        MembershipUser GetUser(string username, bool userIsOnline);

        MembershipUser GetUser(object providerUserKey, bool userIsOnline);

        string GetUserNameByEmail(string email);
                        
        string ResetPassword(string username, string answer);
        
        bool UnlockUser(string userName);
        
        void UpdateUser(MembershipUser user);

        bool ValidateUser(string username, string password);
    }
}
