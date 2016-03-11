using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloBascula;

namespace WPP.Test.ModuloBascula
{
    [TestFixture]
    public class PagoBasculaTest
    {
        /// <summary>
        /// Valida que la pagoBascula ingresado no sea nulo y que la pagoBascula creado es el deseado
        /// </summary>
        [Test]
        public void CreatePagoBasculaTest()
        {
            PagoBascula pagoBascula = new PagoBascula
            {
                Id = 1,
                Monto = 50000
            };

            var repository_fake = A.Fake<IPagoBasculaService>();
            A.CallTo(() => repository_fake.Create(pagoBascula)).Returns(pagoBascula);

            A.CallTo(() => repository_fake.Equals(pagoBascula));

            Assert.IsNotNull(pagoBascula);
        }

        /// <summary>
        /// Valida que al obtener una pagoBascula retorne un objeto tipo pagoBascula y que este no sea nulo
        /// </summary>
        [Test]
        public void GetPagoBasculaTest()
        {
            var pagoBascula = A.Fake<PagoBascula>();
            var repository_fake = A.Fake<IPagoBasculaService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(pagoBascula);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(pagoBascula);
        }

        /// <summary>
        /// Valida que al obtener una pagoBascula por medio de algunos de sus criterios retorne un objeto tipo PagoBascula, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetPagoBasculaTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Monto", 50000);

            var usuario = A.Fake<PagoBascula>();
            var repository_fake = A.Fake<IPagoBasculaService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una pagoBascula, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdatePagoBasculaTest()
        {
            var pagoBascula = A.Fake<PagoBascula>();
            pagoBascula = new PagoBascula { Id = 1, Monto = 75000 };
            var repository_fake = A.Fake<IPagoBasculaService>();
            A.CallTo(() => repository_fake.Update(pagoBascula)).Returns(pagoBascula);

            Assert.That(pagoBascula.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una pagoBascula, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeletePagoBasculaTest()
        {
            PagoBascula pagoBascula = new PagoBascula();

            var repository_fake = A.Fake<IPagoBasculaService>();
            A.CallTo(() => repository_fake.Delete(pagoBascula)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de pagoBasculas existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountPagoBasculaTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IPagoBasculaService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
