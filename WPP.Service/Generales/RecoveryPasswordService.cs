using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Persistance.BaseRepositoryClasses;

namespace WPP.Service.Generales
{
    public class RecoveryPasswordService : IRecoveryPasswordService
    {
         private IRepository<RecoveryPassword> repository;
         public RecoveryPasswordService(IRepository<RecoveryPassword> _repository)
        {
            repository = _repository;
        }

         public RecoveryPassword Get(string token)
        {
            IDictionary<string, object> crit = new Dictionary<string, object>();
            crit.Add("Token", token);
            return repository.Get(crit);
        }
         public IList<RecoveryPassword> GetFromUser(Usuario usuario)
         {
             IDictionary<string, object> crit = new Dictionary<string, object>();
             crit.Add("Usuario", usuario);
             return repository.GetAll(crit);
         }


         public RecoveryPassword Create(RecoveryPassword entity)
        {
            repository.Add(entity);
            return entity;
        }

         public RecoveryPassword Update(RecoveryPassword entity)
        {
            repository.Update(entity);
            return entity;
        }

         public void Delete(RecoveryPassword entity)
        {
            repository.Remove(entity);
        }

    }
}
