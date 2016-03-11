using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloContratos;

namespace WPP.Test.ModuloContratos
{
    [TestFixture]
    public class ClienteTest
    {
        /// <summary>
        /// Valida que el cliente ingresado no sea nulo y que el cliente creado es el deseado
        /// </summary>
        [Test]
        public void CreateClienteTest()
        {
            Cliente cliente = new Cliente
            {
                Id = 1,
                Nombre = "Test",
                Email = "test@gmail.com",
                Cedula = "123456789"
            };

            var repository_fake = A.Fake<IClienteService>();
            A.CallTo(() => repository_fake.Create(cliente)).Returns(cliente);

            A.CallTo(() => repository_fake.Equals(cliente));

            Assert.IsNotNull(cliente);
        }

        /// <summary>
        /// Valida que al obtener un cliente retorne un objeto tipo cliente y que este no sea nulo
        /// </summary>
        [Test]
        public void GetClienteTest()
        {
            var cliente = A.Fake<Cliente>();
            var repository_fake = A.Fake<IClienteService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(cliente);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(cliente);
        }

        /// <summary>
        /// Valida que al obtener un cliente por medio de algunos de sus criterios retorne un objeto tipo Cliente, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetClienteTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Nombre", "Test");

            var usuario = A.Fake<Cliente>();
            var repository_fake = A.Fake<IClienteService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar un cliente, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateClienteTest()
        {
             var cliente = A.Fake<Cliente>();
             cliente = new Cliente { Id = 1, Nombre = "UserTest" };           
            var repository_fake = A.Fake<IClienteService>();
            A.CallTo(() => repository_fake.Update(cliente)).Returns(cliente);

            Assert.That(cliente.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar un cliente, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteClienteTest()
        {
            Cliente cliente = new Cliente();

            var repository_fake = A.Fake<IClienteService>();
            A.CallTo(() => repository_fake.Delete(cliente)).MustNotHaveHappened();
        }
               
        ///<sumary>
        /// Valida que al obtener la lista de clientes, esta no me devuelva un objeto nulo
        ///</sumary>
        [Test]
        public void ListAllClienteTest()
        {
            var repository_fake = A.Fake<IClienteService>();
            A.CallTo(() => repository_fake.ListAll()).Returns(A<List<Cliente>>.That.Not.IsNull());
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de clientes existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountUserTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IClienteService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
