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
    public class BasculaTest
    {
        /// <summary>
        /// Valida que la bascula ingresado no sea nulo y que la bascula creado es el deseado
        /// </summary>
        [Test]
        public void CreateBasculaTest()
        {
            Bascula bascula = new Bascula
            {
                Id = 1,
                Boleta=181
            };

            var repository_fake = A.Fake<IBasculaService>();
            A.CallTo(() => repository_fake.Create(bascula)).Returns(bascula);

            A.CallTo(() => repository_fake.Equals(bascula));

            Assert.IsNotNull(bascula);
        }

        /// <summary>
        /// Valida que al obtener una bascula retorne un objeto tipo bascula y que este no sea nulo
        /// </summary>
        [Test]
        public void GetBasculaTest()
        {
            var bascula = A.Fake<Bascula>();
            var repository_fake = A.Fake<IBasculaService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(bascula);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(bascula);
        }

        /// <summary>
        /// Valida que al obtener una bascula por medio de algunos de sus criterios retorne un objeto tipo Bascula, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetBasculaTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Boleta", 123);

            var usuario = A.Fake<Bascula>();
            var repository_fake = A.Fake<IBasculaService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una bascula, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateBasculaTest()
        {
            var bascula = A.Fake<Bascula>();
            bascula = new Bascula { Id = 1, Boleta = 456 };
            var repository_fake = A.Fake<IBasculaService>();
            A.CallTo(() => repository_fake.Update(bascula)).Returns(bascula);

            Assert.That(bascula.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una bascula, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteBasculaTest()
        {
            Bascula bascula = new Bascula();

            var repository_fake = A.Fake<IBasculaService>();
            A.CallTo(() => repository_fake.Delete(bascula)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de basculas existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountBasculaTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IBasculaService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
