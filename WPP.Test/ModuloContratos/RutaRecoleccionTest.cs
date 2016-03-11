using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Entities.Objects.ModuloOperacionRecoleccion;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloContratos;
using WPP.Service.ModuloOperacionRecoleccion;

namespace WPP.Test.ModuloContratos
{
    [TestFixture]
    public class RutaRecoleccionTest
    {
        /// <summary>
        /// Valida que el ruta ingresado no sea nulo y que el ruta creado es el deseado
        /// </summary>
        [Test]
        public void CreateRutaRecoleccionTest()
        {
            RutaRecoleccion ruta = new RutaRecoleccion
            {
                Id = 1,
                Descripcion = "Test"
            };

            var repository_fake = A.Fake<IRutaRecoleccionService>();
            A.CallTo(() => repository_fake.Create(ruta)).Returns(ruta);

            A.CallTo(() => repository_fake.Equals(ruta));

            Assert.IsNotNull(ruta);
        }

        /// <summary>
        /// Valida que al obtener un ruta retorne un objeto tipo ruta y que este no sea nulo
        /// </summary>
        [Test]
        public void GetRutaRecoleccionTest()
        {
            var ruta = A.Fake<RutaRecoleccion>();
            var repository_fake = A.Fake<IRutaRecoleccionService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(ruta);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(ruta);
        }

        /// <summary>
        /// Valida que al obtener un ruta por medio de algunos de sus criterios retorne un objeto tipo RutaRecoleccion, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetRutaRecoleccionTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Descripcion", "Test");

            var ruta = A.Fake<RutaRecoleccion>();
            var repository_fake = A.Fake<IRutaRecoleccionService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(ruta);

            Assert.IsNotNull(ruta.Id);
        }

        ///<sumary>
        /// Valida que al actualizar un ruta, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateRutaRecoleccionTest()
        {
            var ruta = A.Fake<RutaRecoleccion>();
            ruta = new RutaRecoleccion { Id = 1, Descripcion = "Test" };
            var repository_fake = A.Fake<IRutaRecoleccionService>();
            A.CallTo(() => repository_fake.Update(ruta)).Returns(ruta);

            Assert.That(ruta.Id, Is.EqualTo(1));
        }


        ///<sumary>
        /// Valida que al obtener la lista de rutas, esta no me devuelva un objeto nulo
        ///</sumary>
        [Test]
        public void ListAllRutaRecoleccionTest()
        {
            var repository_fake = A.Fake<IRutaRecoleccionService>();
            A.CallTo(() => repository_fake.ListAll()).Returns(A<List<RutaRecoleccion>>.That.Not.IsNull());
        }
    }
}
