
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Mappings;
using WPP.Entities.Mappings.Generales;
using WPP.Entities.Mappings.ModuloContratos;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Service.BaseServiceClasses
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        public ISession Session { get; set; }

        /// <summary>
        /// Realiza la conexión con la base de datos a traves de fluent hibernate, 
        /// el mapeo de las tablas en caso de ser necesario y construye la Session con la cual se va a trabajar
        /// </summary>
        static UnitOfWork()
        {
            try
            {               

                var dbConfig = OracleDataClientConfiguration.Oracle10
              .ConnectionString(c => c.FromConnectionStringWithKey("db"))
              .Driver<NHibernate.Driver.OracleClientDriver>()
              .DefaultSchema("WPP")
              .ShowSql();
                
                _sessionFactory = Fluently.Configure()
                  .Database(dbConfig)
                  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UsuarioMapping>())
                  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CompaniaMapping>())
                  //.ExposeConfiguration(cfg => new SchemaExport(cfg.SetProperty("hbm2ddl.auto", "create-drop"))
                 //.Create(false, true))
                 .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                  .BuildSessionFactory();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void CreateObjects()
        {

             Catalogo catgrupo1 = new Catalogo();
             catgrupo1.Nombre = "G 1";
             catgrupo1.Tipo = "Grupo";
             catgrupo1.Version = 0;
             catgrupo1.ModifiedBy = "sdf";
             catgrupo1.IsDeleted = false;
             catgrupo1.DateLastModified = DateTime.Now;
             catgrupo1.CreatedBy = "asdf";
             catgrupo1.CreateDate = DateTime.Now;

             //Session.Save(catgrupo1);

             Catalogo catgrupo2 = new Catalogo();
             catgrupo2.Nombre = "G 2";
             catgrupo2.Tipo = "Grupo";
             catgrupo2.Version = 0;
             catgrupo2.ModifiedBy = "sdf";
             catgrupo2.IsDeleted = false;
             catgrupo2.DateLastModified = DateTime.Now;
             catgrupo2.CreatedBy = "asdf";
             catgrupo2.CreateDate = DateTime.Now;

            // Session.Save(catgrupo2);


             Catalogo catgrupo3 = new Catalogo();
             catgrupo3.Nombre = "C 1";
             catgrupo3.Tipo = "TipoCompania";
             catgrupo3.Version = 0;
             catgrupo3.ModifiedBy = "sdf";
             catgrupo3.IsDeleted = false;
             catgrupo3.DateLastModified = DateTime.Now;
             catgrupo3.CreatedBy = "asdf";
             catgrupo3.CreateDate = DateTime.Now;

            // Session.Save(catgrupo3);

             Catalogo catgrupo4 = new Catalogo();
             catgrupo4.Nombre = "C 2";
             catgrupo4.Tipo = "TipoCompania";
             catgrupo4.Version = 0;
             catgrupo4.ModifiedBy = "sdf";
             catgrupo4.IsDeleted = false;
             catgrupo4.DateLastModified = DateTime.Now;
             catgrupo4.CreatedBy = "asdf";
             catgrupo4.CreateDate = DateTime.Now;

            // Session.Save(catgrupo4);



             Cliente cliente = null;

            for (int i = 0; i < 2; i++)
            {
                cliente = new Cliente();
                cliente.Email = DateTime.Now.Millisecond + "ddd@dd.com";
                cliente.CreateDate = DateTime.Now;
                cliente.CreatedBy = DateTime.Now.Millisecond + "ddd@dd.com";
                cliente.DateLastModified = DateTime.Now;
                cliente.Direccion = DateTime.Now.Millisecond + "dddasdfddsdf sdfcom";
                cliente.Fax = "888888888";
                cliente.FechaDesactivacion = DateTime.Now;
                cliente.Grupo = catgrupo1;
                cliente.Nombre = DateTime.Now.Millisecond + "nom";
                cliente.NombreComercial = DateTime.Now.Millisecond + "nom";
                cliente.NombreCorto = DateTime.Now.Millisecond + "nom";
                cliente.Numero = 123;
                cliente.Telefono1 = DateTime.Now.Millisecond + "555nom";
                cliente.Tipo = catgrupo4;

                //Session.Save(cliente);
			}
            //Session.Flush(); 



        }


        private static void BuildSchema(Configuration config)
        {
            //Creates database structure
            new SchemaExport(config)
               .Create(false, true);
        }


        public UnitOfWork()
        {
            try
            {
                Session = _sessionFactory.OpenSession();
            }
            catch (Exception ex)
            {
                string d = ex.ToString();
                throw;
            }
           
           // CreateObjects();
        }

        public void BeginTransaction()
        {
            if (!Session.IsOpen)
            {
                Session = _sessionFactory.OpenSession();
            }
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                Session.Close();
            }
        }
    }
}
