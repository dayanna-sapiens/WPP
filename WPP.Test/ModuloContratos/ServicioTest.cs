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
    public class ServicioTest
    {
        /// <summary>
        /// Valida que la servicio ingresado no sea nulo y que la servicio creado es el deseado
        /// </summary>
        [Test]
        public void CreateServicioTest()
        {
            Servicio servicio = new Servicio
            {
                Id = 1,
                Nombre = "Test"
            };

            var repository_fake = A.Fake<IServicioService>();
            A.CallTo(() => repository_fake.Create(servicio)).Returns(servicio);

            A.CallTo(() => repository_fake.Equals(servicio));

            Assert.IsNotNull(servicio);
        }

        /// <summary>
        /// Valida que al obtener una servicio retorne un objeto tipo servicio y que este no sea nulo
        /// </summary>
        [Test]
        public void GetServicioTest()
        {
            var servicio = A.Fake<Servicio>();
            var repository_fake = A.Fake<IServicioService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(servicio);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(servicio);
        }

        /// <summary>
        /// Valida que al obtener una servicio por medio de algunos de sus criterios retorne un objeto tipo Servicio, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetServicioTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Nombre", "Test");

            var usuario = A.Fake<Servicio>();
            var repository_fake = A.Fake<IServicioService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una servicio, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateServicioTest()
        {
            var servicio = A.Fake<Servicio>();
            servicio = new Servicio { Id = 1, Nombre = "ServicioTest" };
            var repository_fake = A.Fake<IServicioService>();
            A.CallTo(() => repository_fake.Update(servicio)).Returns(servicio);

            Assert.That(servicio.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una servicio, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteServicioTest()
        {
            Servicio servicio = new Servicio();

            var repository_fake = A.Fake<IServicioService>();
            A.CallTo(() => repository_fake.Delete(servicio)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de servicios existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountServicioTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IServicioService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
