using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;

namespace WPP.Service.Generales
{
    public interface IRecoveryPasswordService
    {
        RecoveryPassword Get(string token);
        IList<RecoveryPassword> GetFromUser(Usuario usuario);
        RecoveryPassword Create(RecoveryPassword entity);
        RecoveryPassword Update(RecoveryPassword entity);
        void Delete(RecoveryPassword entity);
     
       
       
    }
}
