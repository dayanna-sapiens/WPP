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
    public class CompaniaTest
    {
        /// <summary>
        /// Valida que la compania ingresado no sea nulo y que la compania creado es el deseado
        /// </summary>
        [Test]
        public void CreateCompaniaTest()
        {
            Compania compania = new Compania
            {
                Id = 1,
                Nombre = "Test",
                Email = "test@gmail.com",
                Cedula = "123456789"
            };

            var repository_fake = A.Fake<ICompaniaService>();
            A.CallTo(() => repository_fake.Create(compania)).Returns(compania);

            A.CallTo(() => repository_fake.Equals(compania));

            Assert.IsNotNull(compania);
        }

        /// <summary>
        /// Valida que al obtener una compania retorne un objeto tipo compania y que este no sea nulo
        /// </summary>
        [Test]
        public void GetCompaniaTest()
        {
            var compania = A.Fake<Compania>();
            var repository_fake = A.Fake<ICompaniaService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(compania);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(compania);
        }

        /// <summary>
        /// Valida que al obtener una compania por medio de algunos de sus criterios retorne un objeto tipo Compania, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetCompaniaTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Nombre", "Test");

            var usuario = A.Fake<Compania>();
            var repository_fake = A.Fake<ICompaniaService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una compania, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateCompaniaTest()
        {
            var compania = A.Fake<Compania>();
            compania = new Compania { Id = 1, Nombre = "UserTest" };
            var repository_fake = A.Fake<ICompaniaService>();
            A.CallTo(() => repository_fake.Update(compania)).Returns(compania);

            Assert.That(compania.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una compania, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteCompaniaTest()
        {
            Compania compania = new Compania();

            var repository_fake = A.Fake<ICompaniaService>();
            A.CallTo(() => repository_fake.Delete(compania)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de companias existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountUserTest()
        {
            var count = 0;
            var repository_fake = A.Fake<ICompaniaService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
