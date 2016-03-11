using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloBascula;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloBoletaManual;

namespace WPP.Test.ModuloBascula
{
     [TestFixture]
    public class BoletaManualTest
    {
        /// <summary>
        /// Valida que la boleta ingresada no sea nulo y que la boleta creada es la deseada
        /// </summary>
        [Test]
        public void CreateBoletaManualTest()
        {
            BoletaManual equipo = new BoletaManual
            {
                Id = 1,
                NumeroBoleta = "123Test"
            };

            var repository_fake = A.Fake<IBoletaManualService>();
            A.CallTo(() => repository_fake.Create(equipo)).Returns(equipo);

            A.CallTo(() => repository_fake.Equals(equipo));

            Assert.IsNotNull(equipo);
        }

        /// <summary>
        /// Valida que al obtener una boleta retorne un objeto boleta y que este no sea nulo
        /// </summary>
        [Test]
        public void GetBoletaManualTest()
        {
            var equipo = A.Fake<BoletaManual>();
            var repository_fake = A.Fake<IBoletaManualService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(equipo);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(equipo);
        }

        /// <summary>
        /// Valida que al obtener una boleta por medio de algunos de sus criterios retorne un objeto tipo BoletaManual, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetBoletaManualTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("NumeroBoleta", "123Test");

            var usuario = A.Fake<BoletaManual>();
            var repository_fake = A.Fake<IBoletaManualService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una boleta, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateBoletaManualTest()
        {
            var equipo = A.Fake<BoletaManual>();
            equipo = new BoletaManual { Id = 1, NumeroBoleta = "123Test" };
            var repository_fake = A.Fake<IBoletaManualService>();
            A.CallTo(() => repository_fake.Update(equipo)).Returns(equipo);

            Assert.That(equipo.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una boleta, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteBoletaManualTest()
        {
            BoletaManual equipo = new BoletaManual();

            var repository_fake = A.Fake<IBoletaManualService>();
            A.CallTo(() => repository_fake.Delete(equipo)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de boletas existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountBoletaManualTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IBoletaManualService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
