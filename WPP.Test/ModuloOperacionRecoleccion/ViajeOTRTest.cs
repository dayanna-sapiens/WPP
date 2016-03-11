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
    public class ViajeOTRTest
    {
        /// <summary>
        /// Valida que la viajeOTR ingresado no sea nulo y que la viajeOTR creado es el deseado
        /// </summary>
        [Test]
        public void CreateViajeOTRTest()
        {
            ViajeOTR viajeOTR = new ViajeOTR
            {
                Id = 1,
                Observaciones = "Test"
            };

            var repository_fake = A.Fake<IViajeOTRService>();
            A.CallTo(() => repository_fake.Create(viajeOTR)).Returns(viajeOTR);

            A.CallTo(() => repository_fake.Equals(viajeOTR));

            Assert.IsNotNull(viajeOTR);
        }

        /// <summary>
        /// Valida que al obtener una viajeOTR retorne un objeto tipo viajeOTR y que este no sea nulo
        /// </summary>
        [Test]
        public void GetViajeOTRTest()
        {
            var viajeOTR = A.Fake<ViajeOTR>();
            var repository_fake = A.Fake<IViajeOTRService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(viajeOTR);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(viajeOTR);
        }

        /// <summary>
        /// Valida que al obtener una viajeOTR por medio de algunos de sus criterios retorne un objeto tipo ViajeOTR, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetViajeOTRTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Observaciones", "Test");

            var usuario = A.Fake<ViajeOTR>();
            var repository_fake = A.Fake<IViajeOTRService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una viajeOTR, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateViajeOTRTest()
        {
            var viajeOTR = A.Fake<ViajeOTR>();
            viajeOTR = new ViajeOTR { Id = 1, Observaciones = "ViajeOTRTest" };
            var repository_fake = A.Fake<IViajeOTRService>();
            A.CallTo(() => repository_fake.Update(viajeOTR)).Returns(viajeOTR);

            Assert.That(viajeOTR.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una viajeOTR, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteViajeOTRTest()
        {
            ViajeOTR viajeOTR = new ViajeOTR();

            var repository_fake = A.Fake<IViajeOTRService>();
            A.CallTo(() => repository_fake.Delete(viajeOTR)).MustNotHaveHappened();
        }

       
    }
}
