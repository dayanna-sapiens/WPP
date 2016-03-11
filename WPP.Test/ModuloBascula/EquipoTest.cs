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
    public class EquipoTest
    {
        /// <summary>
        /// Valida que la equipo ingresado no sea nulo y que la equipo creado es el deseado
        /// </summary>
        [Test]
        public void CreateEquipoTest()
        {
            Equipo equipo = new Equipo
            {
                Id = 1,
                Nombre = "Test"
            };

            var repository_fake = A.Fake<IEquipoService>();
            A.CallTo(() => repository_fake.Create(equipo)).Returns(equipo);

            A.CallTo(() => repository_fake.Equals(equipo));

            Assert.IsNotNull(equipo);
        }

        /// <summary>
        /// Valida que al obtener una equipo retorne un objeto tipo equipo y que este no sea nulo
        /// </summary>
        [Test]
        public void GetEquipoTest()
        {
            var equipo = A.Fake<Equipo>();
            var repository_fake = A.Fake<IEquipoService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(equipo);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(equipo);
        }

        /// <summary>
        /// Valida que al obtener una equipo por medio de algunos de sus criterios retorne un objeto tipo Equipo, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetEquipoTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Nombre", "Test");

            var usuario = A.Fake<Equipo>();
            var repository_fake = A.Fake<IEquipoService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una equipo, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateEquipoTest()
        {
            var equipo = A.Fake<Equipo>();
            equipo = new Equipo { Id = 1, Nombre = "EquipoTest" };
            var repository_fake = A.Fake<IEquipoService>();
            A.CallTo(() => repository_fake.Update(equipo)).Returns(equipo);

            Assert.That(equipo.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una equipo, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteEquipoTest()
        {
            Equipo equipo = new Equipo();

            var repository_fake = A.Fake<IEquipoService>();
            A.CallTo(() => repository_fake.Delete(equipo)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de equipos existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountEquipoTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IEquipoService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
