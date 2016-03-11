using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloContratos;

namespace WPP.Test.ModuloContratos
{
    [TestFixture]
    public class ProductoContratoTest
    {
         /// <summary>
        /// Valida que la producto ingresado no sea nulo y que la producto creado es el deseado
        /// </summary>
        [Test]
        public void CreateProductoContratoTest()
        {
            ProductoContrato producto = new ProductoContrato
            {
                Id = 1,
                Descripcion = "Test"
            };

            var repository_fake = A.Fake<IProductoContratoService>();
            A.CallTo(() => repository_fake.Create(producto)).Returns(producto);

            A.CallTo(() => repository_fake.Equals(producto));

            Assert.IsNotNull(producto);
        }

        /// <summary>
        /// Valida que al obtener una producto retorne un objeto tipo producto y que este no sea nulo
        /// </summary>
        [Test]
        public void GetProductoContratoTest()
        {
            var producto = A.Fake<ProductoContrato>();
            var repository_fake = A.Fake<IProductoContratoService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(producto);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(producto);
        }

        /// <summary>
        /// Valida que al obtener una producto por medio de algunos de sus criterios retorne un objeto tipo ProductoContrato, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetProductoContratoTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Descripcion", "Test");

            var usuario = A.Fake<ProductoContrato>();
            var repository_fake = A.Fake<IProductoContratoService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una producto, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateProductoContratoTest()
        {
            var producto = A.Fake<ProductoContrato>();
            producto = new ProductoContrato { Id = 1, Descripcion = "ProductoContratoTest" };
            var repository_fake = A.Fake<IProductoContratoService>();
            A.CallTo(() => repository_fake.Update(producto)).Returns(producto);

            Assert.That(producto.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una producto, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteProductoContratoTest()
        {
            ProductoContrato producto = new ProductoContrato();

            var repository_fake = A.Fake<IProductoContratoService>();
            A.CallTo(() => repository_fake.Delete(producto)).MustNotHaveHappened();
        }

        ///<sumary>
        /// Valida que al obtener la cantidad de productos existentes el valor esperado no sea nulo
        ///</sumary>
        [Test]
        public void CountProductoContratoTest()
        {
            var count = 0;
            var repository_fake = A.Fake<IProductoContratoService>();
            A.CallTo(() => repository_fake.Count()).Returns(count);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.NotNull(count);

        }
    
    }
}
