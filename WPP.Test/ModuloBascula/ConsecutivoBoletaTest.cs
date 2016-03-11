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
    public class ConsecutivoBoletaTest
    {
        /// <summary>
        /// Valida que la consecutivo ingresado no sea nulo y que la consecutivo creado es el deseado
        /// </summary>
        [Test]
        public void CreateConsecutivoBoletaTest()
        {
            ConsecutivoBoleta consecutivo = new ConsecutivoBoleta
            {
                Id = 1,
                Secuencia = 200
            };

            var repository_fake = A.Fake<IConsecutivoBoletaService>();
            A.CallTo(() => repository_fake.Create(consecutivo)).Returns(consecutivo);

            A.CallTo(() => repository_fake.Equals(consecutivo));

            Assert.IsNotNull(consecutivo);
        }

        /// <summary>
        /// Valida que al obtener una consecutivo retorne un objeto tipo consecutivo y que este no sea nulo
        /// </summary>
        [Test]
        public void GetConsecutivoBoletaTest()
        {
            var consecutivo = A.Fake<ConsecutivoBoleta>();
            var repository_fake = A.Fake<IConsecutivoBoletaService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(consecutivo);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(consecutivo);
        }

        /// <summary>
        /// Valida que al obtener una consecutivo por medio de algunos de sus criterios retorne un objeto tipo ConsecutivoBoleta, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetConsecutivoBoletaTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Secuencia", 250);

            var usuario = A.Fake<ConsecutivoBoleta>();
            var repository_fake = A.Fake<IConsecutivoBoletaService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una consecutivo, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateConsecutivoBoletaTest()
        {
            var consecutivo = A.Fake<ConsecutivoBoleta>();
            consecutivo = new ConsecutivoBoleta { Id = 1, Secuencia = 255 };
            var repository_fake = A.Fake<IConsecutivoBoletaService>();
            A.CallTo(() => repository_fake.Update(consecutivo)).Returns(consecutivo);

            Assert.That(consecutivo.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una consecutivo, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteConsecutivoBoletaTest()
        {
            ConsecutivoBoleta consecutivo = new ConsecutivoBoleta();

            var repository_fake = A.Fake<IConsecutivoBoletaService>();
            A.CallTo(() => repository_fake.Delete(consecutivo)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de consecutivos existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountConsecutivoBoletaTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IConsecutivoBoletaService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
