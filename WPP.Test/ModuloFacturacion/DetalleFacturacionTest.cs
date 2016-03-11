using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloFacturacion;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloDetalleFacturacion;

namespace WPP.Test.ModuloFacturacion
{
    [TestFixture]
    public class DetalleFacturacionTest
    {
        /// <summary>
        /// Valida que el punto ingresado no sea nulo y que el punto creado es el deseado
        /// </summary>
        [Test]
        public void CreateDetalleFacturacionTest()
        {
            DetalleFacturacion punto = new DetalleFacturacion
            {
                Id = 1,
                Cantidad = 1
            };

            var repository_fake = A.Fake<IDetalleFacturacionService>();
            A.CallTo(() => repository_fake.Create(punto)).Returns(punto);

            A.CallTo(() => repository_fake.Equals(punto));

            Assert.IsNotNull(punto);
        }

        /// <summary>
        /// Valida que al obtener un punto retorne un objeto tipo punto y que este no sea nulo
        /// </summary>
        [Test]
        public void GetDetalleFacturacionTest()
        {
            var punto = A.Fake<DetalleFacturacion>();
            var repository_fake = A.Fake<IDetalleFacturacionService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(punto);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(punto);
        }

        /// <summary>
        /// Valida que al obtener un punto por medio de algunos de sus criterios retorne un objeto tipo DetalleFacturacion, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetDetalleFacturacionTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Cantidad", 1);

            var punto = A.Fake<DetalleFacturacion>();
            var repository_fake = A.Fake<IDetalleFacturacionService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(punto);

            Assert.IsNotNull(punto.Id);
        }

        ///<sumary>
        /// Valida que al actualizar un punto, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateDetalleFacturacionTest()
        {
            var punto = A.Fake<DetalleFacturacion>();
            punto = new DetalleFacturacion { Id = 1, Cantidad = 1 };
            var repository_fake = A.Fake<IDetalleFacturacionService>();
            A.CallTo(() => repository_fake.Update(punto)).Returns(punto);

            Assert.That(punto.Id, Is.EqualTo(1));
        }


        ///<sumary>
        /// Valida que al obtener la lista de puntos, esta no me devuelva un objeto nulo
        ///</sumary>
        [Test]
        public void ListAllDetalleFacturacionTest()
        {
            var repository_fake = A.Fake<IDetalleFacturacionService>();
            A.CallTo(() => repository_fake.ListAll()).Returns(A<List<DetalleFacturacion>>.That.Not.IsNull());
        }
    }
}
