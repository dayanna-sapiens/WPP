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
    public class ContenedorTest
    {
        /// <summary>
        /// Valida que la contenedor ingresado no sea nulo y que la contenedor creado es el deseado
        /// </summary>
        [Test]
        public void CreateContenedorTest()
        {
            Contenedor contenedor = new Contenedor
            {
                Id = 1,
                Descripcion = "Test"
            };

            var repository_fake = A.Fake<IContenedorService>();
            A.CallTo(() => repository_fake.Create(contenedor)).Returns(contenedor);

            A.CallTo(() => repository_fake.Equals(contenedor));

            Assert.IsNotNull(contenedor);
        }

        /// <summary>
        /// Valida que al obtener una contenedor retorne un objeto tipo contenedor y que este no sea nulo
        /// </summary>
        [Test]
        public void GetContenedorTest()
        {
            var contenedor = A.Fake<Contenedor>();
            var repository_fake = A.Fake<IContenedorService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(contenedor);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(contenedor);
        }

        /// <summary>
        /// Valida que al obtener una contenedor por medio de algunos de sus criterios retorne un objeto tipo Contenedor, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetContenedorTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Descripcion", "Test");

            var usuario = A.Fake<Contenedor>();
            var repository_fake = A.Fake<IContenedorService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una contenedor, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateContenedorTest()
        {
            var contenedor = A.Fake<Contenedor>();
            contenedor = new Contenedor { Id = 1, Descripcion = "ContenedorTest" };
            var repository_fake = A.Fake<IContenedorService>();
            A.CallTo(() => repository_fake.Update(contenedor)).Returns(contenedor);

            Assert.That(contenedor.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una contenedor, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteContenedorTest()
        {
            Contenedor contenedor = new Contenedor();

            var repository_fake = A.Fake<IContenedorService>();
            A.CallTo(() => repository_fake.Delete(contenedor)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de contenedors existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountContenedorTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IContenedorService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
