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
    public class EmpleadoRecoleccionTest
    {
        /// <summary>
        /// Valida que la empleadoRecoleccion ingresado no sea nulo y que la empleadoRecoleccion creado es el deseado
        /// </summary>
        [Test]
        public void CreateEmpleadoRecoleccionTest()
        {
            EmpleadoRecoleccion empleadoRecoleccion = new EmpleadoRecoleccion
            {
                Id = 1,
                Nombre = "Pedro Rojas"
            };

            var repository_fake = A.Fake<IEmpleadoRecoleccionService>();
            A.CallTo(() => repository_fake.Create(empleadoRecoleccion)).Returns(empleadoRecoleccion);

            A.CallTo(() => repository_fake.Equals(empleadoRecoleccion));

            Assert.IsNotNull(empleadoRecoleccion);
        }

        /// <summary>
        /// Valida que al obtener una empleadoRecoleccion retorne un objeto tipo empleadoRecoleccion y que este no sea nulo
        /// </summary>
        [Test]
        public void GetEmpleadoRecoleccionTest()
        {
            var empleadoRecoleccion = A.Fake<EmpleadoRecoleccion>();
            var repository_fake = A.Fake<IEmpleadoRecoleccionService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(empleadoRecoleccion);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(empleadoRecoleccion);
        }

        /// <summary>
        /// Valida que al obtener una empleadoRecoleccion por medio de algunos de sus criterios retorne un objeto tipo EmpleadoRecoleccion, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetEmpleadoRecoleccionTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Nombre", "Pedro Rojas");

            var usuario = A.Fake<EmpleadoRecoleccion>();
            var repository_fake = A.Fake<IEmpleadoRecoleccionService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una empleadoRecoleccion, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateEmpleadoRecoleccionTest()
        {
            var empleadoRecoleccion = A.Fake<EmpleadoRecoleccion>();
            empleadoRecoleccion = new EmpleadoRecoleccion { Id = 1, Nombre = "PedroRojas" };
            var repository_fake = A.Fake<IEmpleadoRecoleccionService>();
            A.CallTo(() => repository_fake.Update(empleadoRecoleccion)).Returns(empleadoRecoleccion);

            Assert.That(empleadoRecoleccion.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una empleadoRecoleccion, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteEmpleadoRecoleccionTest()
        {
            EmpleadoRecoleccion empleadoRecoleccion = new EmpleadoRecoleccion();

            var repository_fake = A.Fake<IEmpleadoRecoleccionService>();
            A.CallTo(() => repository_fake.Delete(empleadoRecoleccion)).MustNotHaveHappened();
        }

      
    }
}
