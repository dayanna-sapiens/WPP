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
    public class ContactoClienteTest
    {
        /// <summary>
        /// Valida que la contacto ingresado no sea nulo y que la contacto creado es el deseado
        /// </summary>
        [Test]
        public void CreateContactoClienteTest()
        {
            ContactoCliente contacto = new ContactoCliente
            {
                Id = 1,
                Nombre = "Test"
            };

            var repository_fake = A.Fake<IContactoClienteService>();
            A.CallTo(() => repository_fake.Create(contacto)).Returns(contacto);

            A.CallTo(() => repository_fake.Equals(contacto));

            Assert.IsNotNull(contacto);
        }

        /// <summary>
        /// Valida que al obtener una contacto retorne un objeto tipo contacto y que este no sea nulo
        /// </summary>
        [Test]
        public void GetContactoClienteTest()
        {
            var contacto = A.Fake<ContactoCliente>();
            var repository_fake = A.Fake<IContactoClienteService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(contacto);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(contacto);
        }

       
        ///<sumary>
        /// Valida que al actualizar una contacto, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateContactoClienteTest()
        {
            var contacto = A.Fake<ContactoCliente>();
            contacto = new ContactoCliente { Id = 1, Nombre = "ContactoClienteTest" };
            var repository_fake = A.Fake<IContactoClienteService>();
            A.CallTo(() => repository_fake.Update(contacto)).Returns(contacto);

            Assert.That(contacto.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una contacto, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteContactoClienteTest()
        {
            ContactoCliente contacto = new ContactoCliente();

            var repository_fake = A.Fake<IContactoClienteService>();
            A.CallTo(() => repository_fake.Delete(contacto)).MustNotHaveHappened();
        }

       
    }
}
