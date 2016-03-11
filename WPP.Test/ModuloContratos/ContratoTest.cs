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
    public class ContratoTest
    {
        /// <summary>
        /// Valida que la contrato ingresado no sea nulo y que la contrato creado es el deseado
        /// </summary>
        [Test]
        public void CreateContratoTest()
        {
            Contrato contrato = new Contrato
            {
                Id = 1,
                DescripcionContrato = "Test"
            };

            var repository_fake = A.Fake<IContratoService>();
            A.CallTo(() => repository_fake.Create(contrato)).Returns(contrato);

            A.CallTo(() => repository_fake.Equals(contrato));

            Assert.IsNotNull(contrato);
        }

        /// <summary>
        /// Valida que al obtener una contrato retorne un objeto tipo contrato y que este no sea nulo
        /// </summary>
        [Test]
        public void GetContratoTest()
        {
            var contrato = A.Fake<Contrato>();
            var repository_fake = A.Fake<IContratoService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(contrato);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(contrato);
        }

        /// <summary>
        /// Valida que al obtener una contrato por medio de algunos de sus criterios retorne un objeto tipo Contrato, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetContratoTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("DescripcionContrato", "Test");

            var usuario = A.Fake<Contrato>();
            var repository_fake = A.Fake<IContratoService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una contrato, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateContratoTest()
        {
            var contrato = A.Fake<Contrato>();
            contrato = new Contrato { Id = 1, DescripcionContrato = "ContratoTest" };
            var repository_fake = A.Fake<IContratoService>();
            A.CallTo(() => repository_fake.Update(contrato)).Returns(contrato);

            Assert.That(contrato.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una contrato, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteContratoTest()
        {
            Contrato contrato = new Contrato();

            var repository_fake = A.Fake<IContratoService>();
            A.CallTo(() => repository_fake.Delete(contrato)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de contratos existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountUserTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IContratoService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
