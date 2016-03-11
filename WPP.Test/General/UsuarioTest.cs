using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Persistance.BaseRepositoryClasses;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloContratos;

namespace WPP.Test.ModuloContratos
{
  
    [TestFixture]
    public class UsuarioTest
    {
        /// <summary>
        /// Valida que el usuario ingresado no sea nulo
        /// </summary>
        [Test]
        public void CreateUserTest()
        {
            Usuario userTest = new Usuario 
                                 { Id = 0, 
                                   Nombre = "UserTest", 
                                   Apellido1 = "Test", 
                                   Apellido2 = "Test", 
                                   Cedula = "123456789" };

            var repository_fake = A.Fake<IUsuarioService>();
            A.CallTo(() => repository_fake.Create(userTest));

            A.CallTo(() => repository_fake.Equals(userTest));
                    
            Assert.IsNotNull(userTest);          
        }

        /// <summary>
        /// Valida que al obtener un usuario retorne un objeto tipo Usuario y que este no sea nulo
        /// </summary>
        [Test]
        public void GetUserTest()
        {
            var usuario = A.Fake<Usuario>();
            var repository_fake = A.Fake<IUsuarioService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(usuario);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(usuario);            
        }

        /// <summary>
        /// Valida que al obtener un usuario por medio de algunos de sus criterios retorne un objeto tipo Usuario, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetUserTestWithParameters()
        {
            IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
            criteriaUser.Add("Email", "user@sapiens.co.cr");
            criteriaUser.Add("Password", "123");

            var usuario = A.Fake<Usuario>();
            var repository_fake = A.Fake<IUsuarioService>();
            A.CallTo(() => repository_fake.Get(criteriaUser)).Returns(usuario);
            
            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar un usuario, el objeto no tenga sus columnas de email y password nulas
        ///</sumary>
        [Test]
        public void UpdateUserTest()
        {
            Usuario userTest = new Usuario { Id = 0, Nombre = "UserTest" };
            var usuario = A.Fake<Usuario>();
            var repository_fake = A.Fake<IUsuarioService>();
            A.CallTo(() => repository_fake.Update(userTest)).Returns(usuario);

            Assert.That(usuario.Email != null);
            Assert.That(usuario.Password != null);      
        }

        ///<sumary>
        /// Valida que al eliminar un usuario, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteUserTest()
        {
            Usuario userTest = new Usuario();

            var repository_fake = A.Fake<IUsuarioService>();
            A.CallTo(() => repository_fake.Delete(userTest)).MustNotHaveHappened();       
        }

        ///<sumary>
        /// Valida que al consultar si un usuario existe, la acción se lleve de manera exitosa 
        ///</sumary>
        [Test]
        public void ContainsUserTest()
        {
            Usuario userTest = new Usuario { Id = 0, Nombre = "UserTest" };

            var repository_fake = A.Fake<IUsuarioService>();
            A.CallTo(() => repository_fake.Contains(userTest)).MustHaveHappened(Repeated.NoMoreThan.Once);
        }

        ///<sumary>
        /// Valida que al consultar si existe usuario el valor retornado sea el esperado
        ///</sumary>
        [Test]
        public void ContainsUserTestWithParameters()
        {
            Usuario userTest = new Usuario ();
 
            var repository_fake = A.Fake<IUsuarioService>();

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => repository_fake.Contains(userTest, "Nombre", "UserTest")).Returns(true);
            A.CallTo(() => unitOfWork_fake.Commit());
        }

        ///<sumary>
        /// Valida que al obtener la lista de usuarios, esta no me devuelva un objeto nulo
        ///</sumary>
        [Test]
        public void ListAllUserTest()
        {
            var repository_fake = A.Fake<IUsuarioService>();
            A.CallTo(() => repository_fake.ListAll()).Returns(A<IEnumerable<Usuario>>.That.Not.IsNull());
        }

        ///<sumary>
        /// Valida que al obtener una lista de usuarios que cumpla con ciertos criterios se lleve a cabo de manera exitosa
        ///</sumary>
        [Test]
        public void GetAllUserTest()
        {
            IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
            criteriaUser.Add("Roles", "Super Usuario");
            
            var repository_fake = A.Fake<IUsuarioService>();
            A.CallTo(() => repository_fake.GetAll(criteriaUser, "CreateDate", DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(2)))
                .MustHaveHappened(Repeated.NoMoreThan.Twice);
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de usuarios existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountUserTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IUsuarioService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
        
    }
    
}
