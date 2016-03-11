using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Persistance.BaseRepositoryClasses
{
    public class NHibernateBase
    {
        protected readonly ISessionFactory sessionFactory;

        protected virtual ISession Session
        {
            get
            {
                ISession session = null;
                if (!NHibernate.Context.CurrentSessionContext.HasBind(sessionFactory))
                {
                    session = sessionFactory.OpenSession();
                    NHibernate.Context.CurrentSessionContext.Bind(session);
                }
                return sessionFactory.GetCurrentSession();
            }
        }

        public NHibernateBase(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        protected virtual TResult Transact<TResult>(Func<TResult> func)
        {
            if (!Session.Transaction.IsActive)
            {
                //Wrap in Transaction
                TResult result;
                using (var tx = Session.BeginTransaction())
                {
                    result = func.Invoke();
                    tx.Commit();

                }
                return result;
            }
            //Dont Wrap
            return func.Invoke();
        }

        protected virtual void Transact(Action action)
        {
            Transact<bool>(() =>
            {
                action.Invoke();
                return false;
            });
        }
    }
}
