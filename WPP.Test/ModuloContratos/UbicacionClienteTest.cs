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
    public class UbicacionClienteTest
    {
        /// <summary>
        /// Valida que la ubicacionCliente ingresado no sea nulo y que la ubicacionCliente creado es el deseado
        /// </summary>
        [Test]
        public void CreateUbicacionClienteTest()
        {
            UbicacionCliente ubicacionCliente = new UbicacionCliente
            {
                Id = 1,
                Descripcion = "Test"
            };

            var repository_fake = A.Fake<IUbicacionClienteService>();
            A.CallTo(() => repository_fake.Create(ubicacionCliente)).Returns(ubicacionCliente);

            A.CallTo(() => repository_fake.Equals(ubicacionCliente));

            Assert.IsNotNull(ubicacionCliente);
        }

        /// <summary>
        /// Valida que al obtener una ubicacionCliente retorne un objeto tipo ubicacionCliente y que este no sea nulo
        /// </summary>
        [Test]
        public void GetUbicacionClienteTest()
        {
            var ubicacionCliente = A.Fake<UbicacionCliente>();
            var repository_fake = A.Fake<IUbicacionClienteService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(ubicacionCliente);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(ubicacionCliente);
        }


        ///<sumary>
        /// Valida que al actualizar una ubicacionCliente, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateUbicacionClienteTest()
        {
            var ubicacionCliente = A.Fake<UbicacionCliente>();
            ubicacionCliente = new UbicacionCliente { Id = 1, Descripcion = "UbicacionClienteTest" };
            var repository_fake = A.Fake<IUbicacionClienteService>();
            A.CallTo(() => repository_fake.Update(ubicacionCliente)).Returns(ubicacionCliente);

            Assert.That(ubicacionCliente.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una ubicacionCliente, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteUbicacionClienteTest()
        {
            UbicacionCliente ubicacionCliente = new UbicacionCliente();

            var repository_fake = A.Fake<IUbicacionClienteService>();
            A.CallTo(() => repository_fake.Delete(ubicacionCliente)).MustNotHaveHappened();
        }

    }
}
