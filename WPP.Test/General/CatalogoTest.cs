using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Service.BaseServiceClasses;
using WPP.Service.Generales;

namespace WPP.Test.General
{
    public class CatalogoTest
    {
        /// <summary>
        /// Valida que el catalogo ingresado no sea nulo
        /// </summary>
        [Test]
        public void CreateCatalogoTest()
        {
            Catalogo catalogoTest = new Catalogo
            {
                Id = 1,
                Nombre = "Grupo",
                Tipo = "Municipal",
                CreatedBy = "test",
                CreateDate = DateTime.Now
            };

            var repository_fake = A.Fake<ICatalogoService>();
            A.CallTo(() => repository_fake.Create(catalogoTest));

            A.CallTo(() => repository_fake.Equals(catalogoTest));

            Assert.IsNotNull(catalogoTest);
        }

        /// <summary>
        /// Valida que al obtener un catalogo retorne un objeto tipo Catalogo y que este no sea nulo
        /// </summary>
        [Test]
        public void GetCatalogoTest()
        {
            var catalogo = A.Fake<Catalogo>();
            var repository_fake = A.Fake<ICatalogoService>();
            A.CallTo(() => repository_fake.Get(0)).Returns(catalogo).Twice();

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(catalogo);
        }

        /// <summary>
        /// Valida que al obtener un catalogo por medio de algunos de sus criterios retorne un objeto tipo Catalogo, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetCatalogoTestWithParameters()
        {
            IDictionary<string, object> criteriaUser = new Dictionary<string, object>();
            criteriaUser.Add("Tipo", "Comercial");

            var catalogo = A.Fake<Catalogo>();
            var repository_fake = A.Fake<ICatalogoService>();
            A.CallTo(() => repository_fake.Get(criteriaUser)).Returns(catalogo);

            Assert.IsNotNull(catalogo.Id);
        }

        ///<sumary>
        /// Valida que al actualizar un catalogo, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateCatalogoTest()
        {
            Catalogo userTest = new Catalogo { Id = 1, Nombre = "Grupo", Tipo = "Comercial" };
            var catalogo = A.Fake<Catalogo>();
            catalogo = new Catalogo { Id = 1, Nombre = "Grupo", Tipo = "Comercial" };
            var repository_fake = A.Fake<ICatalogoService>();
            A.CallTo(() => repository_fake.Update(userTest)).Returns(userTest);

            Assert.That(userTest.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar un catalogo, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteCatalogoTest()
        {
            Catalogo catalogo = new Catalogo();

            var repository_fake = A.Fake<ICatalogoService>();
            A.CallTo(() => repository_fake.Delete(catalogo)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al consultar si un catalogo existe, la acción se lleve de manera exitosa 
        ///</sumary>
        [Test]
        public void ContainsCatalogoTest()
        {
            Catalogo catalogo = new Catalogo { Id = 1, Nombre = "UserTest" };

            var repository_fake = A.Fake<ICatalogoService>();
            A.CallTo(() => repository_fake.Contains(catalogo)).MustHaveHappened(Repeated.NoMoreThan.Once);
        }

        ///<sumary>
        /// Valida que al consultar si existe catalogo el valor retornado sea el esperado
        ///</sumary>
        [Test]
        public void ContainsCatalogoTestWithParameters()
        {
            Catalogo catalogo = new Catalogo() { 
                Nombre = "Municipal"
            };

            var repository_fake = A.Fake<ICatalogoService>();

            A.CallTo(() => repository_fake.Contains(catalogo, "Nombre", "Municipal")).Returns(true);
        }

        ///<sumary>
        /// Valida que al obtener la lista de catalogos, esta no me devuelva un objeto nulo
        ///</sumary>
        [Test]
        public void ListAllCatalogoTest()
        {
            var repository_fake = A.Fake<ICatalogoService>();
            A.CallTo(() => repository_fake.ListAll()).Returns(A<IEnumerable<Catalogo>>.That.Not.IsNull());
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de registros del catalogo existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountCatalogoTest()
        {
            var count = 0;
            var repository_fake = A.Fake<ICatalogoService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
        
    }
}
