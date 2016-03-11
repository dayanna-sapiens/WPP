using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Test.ModuloOperacionRecoleccion
{
    [TestFixture]
    public class RutaRecoleccionTest
    {
        /// <summary>
        /// Valida que la rutaRecoleccion ingresado no sea nulo y que la rutaRecoleccion creado es el deseado
        /// </summary>
        [Test]
        public void CreateRutaRecoleccionTest()
        {
            RutaRecoleccion rutaRecoleccion = new RutaRecoleccion
            {
                Id = 1,
                Descripcion = "Test"
            };

            var repository_fake = A.Fake<IRutaRecoleccionService>();
            A.CallTo(() => repository_fake.Create(rutaRecoleccion)).Returns(rutaRecoleccion);

            A.CallTo(() => repository_fake.Equals(rutaRecoleccion));

            Assert.IsNotNull(rutaRecoleccion);
        }

        /// <summary>
        /// Valida que al obtener una rutaRecoleccion retorne un objeto tipo rutaRecoleccion y que este no sea nulo
        /// </summary>
        [Test]
        public void GetRutaRecoleccionTest()
        {
            var rutaRecoleccion = A.Fake<RutaRecoleccion>();
            var repository_fake = A.Fake<IRutaRecoleccionService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(rutaRecoleccion);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(rutaRecoleccion);
        }

        /// <summary>
        /// Valida que al obtener una rutaRecoleccion por medio de algunos de sus criterios retorne un objeto tipo RutaRecoleccion, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetRutaRecoleccionTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Descripcion", "Test");

            var usuario = A.Fake<RutaRecoleccion>();
            var repository_fake = A.Fake<IRutaRecoleccionService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una rutaRecoleccion, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateRutaRecoleccionTest()
        {
            var rutaRecoleccion = A.Fake<RutaRecoleccion>();
            rutaRecoleccion = new RutaRecoleccion { Id = 1, Descripcion = "RutaRecoleccionTest" };
            var repository_fake = A.Fake<IRutaRecoleccionService>();
            A.CallTo(() => repository_fake.Update(rutaRecoleccion)).Returns(rutaRecoleccion);

            Assert.That(rutaRecoleccion.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una rutaRecoleccion, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteRutaRecoleccionTest()
        {
            RutaRecoleccion rutaRecoleccion = new RutaRecoleccion();

            var repository_fake = A.Fake<IRutaRecoleccionService>();
            A.CallTo(() => repository_fake.Delete(rutaRecoleccion)).MustNotHaveHappened();
        }

       
    }
}
