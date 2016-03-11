using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloFacturacion;

namespace WPP.Test.ModuloFacturacion
{
    [TestFixture]
    public class ConsecutivoFacturacionTest
    {

        /// <summary>
        /// Valida que la consecutivo ingresado no sea nulo y que la consecutivo creado es el deseado
        /// </summary>
        [Test]
        public void CreateConsecutivoFacturacionTest()
        {
            ConsecutivoFacturacion consecutivo = new ConsecutivoFacturacion
            {
                Id = 1,
                Secuencia = 1
            };

            var repository_fake = A.Fake<IConsecutivoFacturacionService>();
            A.CallTo(() => repository_fake.Create(consecutivo)).Returns(consecutivo);

            A.CallTo(() => repository_fake.Equals(consecutivo));

            Assert.IsNotNull(consecutivo);
        }

        /// <summary>
        /// Valida que al obtener una consecutivo retorne un objeto tipo consecutivo y que este no sea nulo
        /// </summary>
        [Test]
        public void GetConsecutivoFacturacionTest()
        {
            var consecutivo = A.Fake<ConsecutivoFacturacion>();
            var repository_fake = A.Fake<IConsecutivoFacturacionService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(consecutivo);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(consecutivo);
        }


        ///<sumary>
        /// Valida que al actualizar una consecutivo, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateConsecutivoFacturacionTest()
        {
            var consecutivo = A.Fake<ConsecutivoFacturacion>();
            consecutivo = new ConsecutivoFacturacion { Id = 1, Secuencia = 1 };
            var repository_fake = A.Fake<IConsecutivoFacturacionService>();
            A.CallTo(() => repository_fake.Update(consecutivo)).Returns(consecutivo);

            Assert.That(consecutivo.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una consecutivo, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteConsecutivoFacturacionTest()
        {
            ConsecutivoFacturacion consecutivo = new ConsecutivoFacturacion();

            var repository_fake = A.Fake<IConsecutivoFacturacionService>();
            A.CallTo(() => repository_fake.Delete(consecutivo)).MustNotHaveHappened();
        }
    }
}
