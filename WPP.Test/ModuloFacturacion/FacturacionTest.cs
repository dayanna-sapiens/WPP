using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloFacturacion;

namespace WPP.Test.ModuloFacturacion
{
    [TestFixture]
    public class FacturacionTest
    {
        /// <summary>
        /// Valida que el punto ingresado no sea nulo y que el punto creado es el deseado
        /// </summary>
        [Test]
        public void CreateFacturacionTest()
        {
            Facturacion punto = new Facturacion
            {
                Id = 1,
                Descripcion = "Test"
            };

            var repository_fake = A.Fake<IFacturacionService>();
            A.CallTo(() => repository_fake.Create(punto)).Returns(punto);

            A.CallTo(() => repository_fake.Equals(punto));

            Assert.IsNotNull(punto);
        }

        /// <summary>
        /// Valida que al obtener un punto retorne un objeto tipo punto y que este no sea nulo
        /// </summary>
        [Test]
        public void GetFacturacionTest()
        {
            var punto = A.Fake<Facturacion>();
            var repository_fake = A.Fake<IFacturacionService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(punto);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(punto);
        }

        /// <summary>
        /// Valida que al obtener un punto por medio de algunos de sus criterios retorne un objeto tipo Facturacion, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetFacturacionTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Descripcion", "Test");

            var punto = A.Fake<Facturacion>();
            var repository_fake = A.Fake<IFacturacionService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(punto);

            Assert.IsNotNull(punto.Id);
        }

        ///<sumary>
        /// Valida que al actualizar un punto, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateFacturacionTest()
        {
            var punto = A.Fake<Facturacion>();
            punto = new Facturacion { Id = 1, Descripcion = "Test" };
            var repository_fake = A.Fake<IFacturacionService>();
            A.CallTo(() => repository_fake.Update(punto)).Returns(punto);

            Assert.That(punto.Id, Is.EqualTo(1));
        }


        ///<sumary>
        /// Valida que al obtener la lista de puntos, esta no me devuelva un objeto nulo
        ///</sumary>
        [Test]
        public void ListAllFacturacionTest()
        {
            var repository_fake = A.Fake<IFacturacionService>();
            A.CallTo(() => repository_fake.ListAll()).Returns(A<List<Facturacion>>.That.Not.IsNull());
        }

    }
}
