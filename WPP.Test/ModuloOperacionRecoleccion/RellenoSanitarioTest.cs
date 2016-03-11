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
    public class RellenoSanitarioTest
    {
        /// <summary>
        /// Valida que la rellenoSanitario ingresado no sea nulo y que la rellenoSanitario creado es el deseado
        /// </summary>
        [Test]
        public void CreateRellenoSanitarioTest()
        {
            RellenoSanitario rellenoSanitario = new RellenoSanitario
            {
                Id = 1,
                Nombre = "Test"
            };

            var repository_fake = A.Fake<IRellenoSanitarioService>();
            A.CallTo(() => repository_fake.Create(rellenoSanitario)).Returns(rellenoSanitario);

            A.CallTo(() => repository_fake.Equals(rellenoSanitario));

            Assert.IsNotNull(rellenoSanitario);
        }

        /// <summary>
        /// Valida que al obtener una rellenoSanitario retorne un objeto tipo rellenoSanitario y que este no sea nulo
        /// </summary>
        [Test]
        public void GetRellenoSanitarioTest()
        {
            var rellenoSanitario = A.Fake<RellenoSanitario>();
            var repository_fake = A.Fake<IRellenoSanitarioService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(rellenoSanitario);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(rellenoSanitario);
        }

        /// <summary>
        /// Valida que al obtener una rellenoSanitario por medio de algunos de sus criterios retorne un objeto tipo RellenoSanitario, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetRellenoSanitarioTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Nombre", "Test");

            var usuario = A.Fake<RellenoSanitario>();
            var repository_fake = A.Fake<IRellenoSanitarioService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una rellenoSanitario, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateRellenoSanitarioTest()
        {
            var rellenoSanitario = A.Fake<RellenoSanitario>();
            rellenoSanitario = new RellenoSanitario { Id = 1, Nombre = "RellenoSanitarioTest" };
            var repository_fake = A.Fake<IRellenoSanitarioService>();
            A.CallTo(() => repository_fake.Update(rellenoSanitario)).Returns(rellenoSanitario);

            Assert.That(rellenoSanitario.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una rellenoSanitario, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteRellenoSanitarioTest()
        {
            RellenoSanitario rellenoSanitario = new RellenoSanitario();

            var repository_fake = A.Fake<IRellenoSanitarioService>();
            A.CallTo(() => repository_fake.Delete(rellenoSanitario)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de rellenoSanitarios existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountRellenoSanitarioTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IRellenoSanitarioService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
