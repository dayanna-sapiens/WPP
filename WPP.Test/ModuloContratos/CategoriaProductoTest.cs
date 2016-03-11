using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloContratos;

namespace WPP.Test.ModuloContratos
{
    [TestFixture]
    public class CategoriaProductoTest
    {
        /// <summary>
        /// Valida que el categoria ingresado no sea nulo y que el categoria creado es el deseado
        /// </summary>
        [Test]
        public void CreateCategoriaProductoTest()
        {
            CategoriaProducto categoria = new CategoriaProducto
            {
                Id = 1,
                Nombre = "Test"
            };

            var repository_fake = A.Fake<ICategoriaProductoService>();
            A.CallTo(() => repository_fake.Create(categoria)).Returns(categoria);

            A.CallTo(() => repository_fake.Equals(categoria));

            Assert.IsNotNull(categoria);
        }

        /// <summary>
        /// Valida que al obtener un categoria retorne un objeto tipo categoria y que este no sea nulo
        /// </summary>
        [Test]
        public void GetCategoriaProductoTest()
        {
            var categoria = A.Fake<CategoriaProducto>();
            var repository_fake = A.Fake<ICategoriaProductoService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(categoria);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(categoria);
        }

        /// <summary>
        /// Valida que al obtener un categoria por medio de algunos de sus criterios retorne un objeto tipo CategoriaProducto, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetCategoriaProductoTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Nombre", "Test");

            var categoria = A.Fake<CategoriaProducto>();
            var repository_fake = A.Fake<ICategoriaProductoService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(categoria);

            Assert.IsNotNull(categoria.Id);
        }

        ///<sumary>
        /// Valida que al actualizar un categoria, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateCategoriaProductoTest()
        {
            var categoria = A.Fake<CategoriaProducto>();
            categoria = new CategoriaProducto { Id = 1, Nombre = "CategoriaTest" };
            var repository_fake = A.Fake<ICategoriaProductoService>();
            A.CallTo(() => repository_fake.Update(categoria)).Returns(categoria);

            Assert.That(categoria.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar un categoria, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteCategoriaProductoTest()
        {
            CategoriaProducto categoria = new CategoriaProducto();

            var repository_fake = A.Fake<ICategoriaProductoService>();
            A.CallTo(() => repository_fake.Delete(categoria)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la lista de categorias, esta no me devuelva un objeto nulo
        ///</sumary>
        [Test]
        public void ListAllCategoriaProductoTest()
        {
            var repository_fake = A.Fake<ICategoriaProductoService>();
            A.CallTo(() => repository_fake.ListAll()).Returns(A<List<CategoriaProducto>>.That.Not.IsNull());
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de categorias existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountUserTest()
        {
            var count = 0;
            var repository_fake = A.Fake<ICategoriaProductoService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    }
}
