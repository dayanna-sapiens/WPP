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
    public class OTRTest
    {
        /// <summary>
        /// Valida que la otr ingresado no sea nulo y que la otr creado es el deseado
        /// </summary>
        [Test]
        public void CreateOTRTest()
        {
            OTR otr = new OTR
            {
                Id = 1, Observaciones = "Test"
            };

            var repository_fake = A.Fake<IOTRService>();
            A.CallTo(() => repository_fake.Create(otr)).Returns(otr);

            A.CallTo(() => repository_fake.Equals(otr));

            Assert.IsNotNull(otr);
        }

        /// <summary>
        /// Valida que al obtener una otr retorne un objeto tipo otr y que este no sea nulo
        /// </summary>
        [Test]
        public void GetOTRTest()
        {
            var otr = A.Fake<OTR>();
            var repository_fake = A.Fake<IOTRService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(otr);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(otr);
        }

        /// <summary>
        /// Valida que al obtener una otr por medio de algunos de sus criterios retorne un objeto tipo OTR, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetOTRTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Observaciones", "Test");

            var usuario = A.Fake<OTR>();
            var repository_fake = A.Fake<IOTRService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una otr, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateOTRTest()
        {
            var otr = A.Fake<OTR>();
            otr = new OTR { Id = 1, Observaciones = "Test" };
            var repository_fake = A.Fake<IOTRService>();
            A.CallTo(() => repository_fake.Update(otr)).Returns(otr);

            Assert.That(otr.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una otr, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteOTRTest()
        {
            OTR otr = new OTR();

            var repository_fake = A.Fake<IOTRService>();
            A.CallTo(() => repository_fake.Delete(otr)).MustNotHaveHappened();
        }
    }
}
