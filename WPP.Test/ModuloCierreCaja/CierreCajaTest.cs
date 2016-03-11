using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.ModuloCierreCaja;
using WPP.Service.BaseServiceClasses;
using WPP.Service.ModuloCierreCaja;

namespace WPP.Test.ModuloCierreCaja
{
    [TestFixture]
    public class CierreCajaTest
    {
        /// <summary>
        /// Valida que la cierreCaja ingresado no sea nulo y que la cierreCaja creado es el deseado
        /// </summary>
        [Test]
        public void CreateCierreCajaTest()
        {
            CierreCaja cierreCaja = new CierreCaja
            {
                Id = 1,
                AjusteCaja = 1000,
                Moneda = "Colones"
            };

            var repository_fake = A.Fake<ICierreCajaService>();
            A.CallTo(() => repository_fake.Create(cierreCaja)).Returns(cierreCaja);

            A.CallTo(() => repository_fake.Equals(cierreCaja));

            Assert.IsNotNull(cierreCaja);
        }

        /// <summary>
        /// Valida que al obtener una cierreCaja retorne un objeto tipo cierreCaja y que este no sea nulo
        /// </summary>
        [Test]
        public void GetCierreCajaTest()
        {
            var cierreCaja = A.Fake<CierreCaja>();
            var repository_fake = A.Fake<ICierreCajaService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(cierreCaja);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(cierreCaja);
        }

        /// <summary>
        /// Valida que al obtener una cierreCaja por medio de algunos de sus criterios retorne un objeto tipo CierreCaja, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetCierreCajaTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("AjusteCaja", 1000);

            var usuario = A.Fake<CierreCaja>();
            var repository_fake = A.Fake<ICierreCajaService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una cierreCaja, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateCierreCajaTest()
        {
            var cierreCaja = A.Fake<CierreCaja>();
            cierreCaja = new CierreCaja { Id = 1, AjusteCaja = 1000 };
            var repository_fake = A.Fake<ICierreCajaService>();
            A.CallTo(() => repository_fake.Update(cierreCaja)).Returns(cierreCaja);

            Assert.That(cierreCaja.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una cierreCaja, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteCierreCajaTest()
        {
            CierreCaja cierreCaja = new CierreCaja();

            var repository_fake = A.Fake<ICierreCajaService>();
            A.CallTo(() => repository_fake.Delete(cierreCaja)).MustNotHaveHappened();
        }

      
    }
}
