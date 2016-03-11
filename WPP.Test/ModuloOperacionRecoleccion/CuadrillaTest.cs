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
    public class CuadrillaTest
    {
        /// <summary>
        /// Valida que la cuadrilla ingresado no sea nulo y que la cuadrilla creado es el deseado
        /// </summary>
        [Test]
        public void CreateCuadrillaTest()
        {
            Cuadrilla cuadrilla = new Cuadrilla
            {
                Id = 1,
                Descripcion = "Test"
            };

            var repository_fake = A.Fake<ICuadrillaService>();
            A.CallTo(() => repository_fake.Create(cuadrilla)).Returns(cuadrilla);

            A.CallTo(() => repository_fake.Equals(cuadrilla));

            Assert.IsNotNull(cuadrilla);
        }

        /// <summary>
        /// Valida que al obtener una cuadrilla retorne un objeto tipo cuadrilla y que este no sea nulo
        /// </summary>
        [Test]
        public void GetCuadrillaTest()
        {
            var cuadrilla = A.Fake<Cuadrilla>();
            var repository_fake = A.Fake<ICuadrillaService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(cuadrilla);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(cuadrilla);
        }

        /// <summary>
        /// Valida que al obtener una cuadrilla por medio de algunos de sus criterios retorne un objeto tipo Cuadrilla, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetCuadrillaTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Descripcion", "Test");

            var usuario = A.Fake<Cuadrilla>();
            var repository_fake = A.Fake<ICuadrillaService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una cuadrilla, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateCuadrillaTest()
        {
            var cuadrilla = A.Fake<Cuadrilla>();
            cuadrilla = new Cuadrilla { Id = 1, Descripcion = "CuadrillaTest" };
            var repository_fake = A.Fake<ICuadrillaService>();
            A.CallTo(() => repository_fake.Update(cuadrilla)).Returns(cuadrilla);

            Assert.That(cuadrilla.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una cuadrilla, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteCuadrillaTest()
        {
            Cuadrilla cuadrilla = new Cuadrilla();

            var repository_fake = A.Fake<ICuadrillaService>();
            A.CallTo(() => repository_fake.Delete(cuadrilla)).MustNotHaveHappened();
        }

       
    }
}
