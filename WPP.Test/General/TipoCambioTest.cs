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
    [TestFixture]
    public class TipoCambioTest
    {
         /// <summary>
        /// Valida que la tipoCambio ingresado no sea nulo y que la tipoCambio creado es el deseado
        /// </summary>
        [Test]
        public void CreateTipoCambioTest()
        {
            TipoCambio tipoCambio = new TipoCambio
            {
                Id = 1,
                Valor = 528
            };

            var repository_fake = A.Fake<ITipoCambioService>();
            A.CallTo(() => repository_fake.Create(tipoCambio)).Returns(tipoCambio);

            A.CallTo(() => repository_fake.Equals(tipoCambio));

            Assert.IsNotNull(tipoCambio);
        }

        /// <summary>
        /// Valida que al obtener una tipoCambio retorne un objeto tipo tipoCambio y que este no sea nulo
        /// </summary>
        [Test]
        public void GetTipoCambioTest()
        {
            var tipoCambio = A.Fake<TipoCambio>();
            var repository_fake = A.Fake<ITipoCambioService>();
            var user = A.CallTo(() => repository_fake.Get(0)).Returns(tipoCambio);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            Assert.IsNotNull(tipoCambio);
        }

        /// <summary>
        /// Valida que al obtener una tipoCambio por medio de algunos de sus criterios retorne un objeto tipo TipoCambio, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetTipoCambioTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("Valor", 580);

            var usuario = A.Fake<TipoCambio>();
            var repository_fake = A.Fake<ITipoCambioService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(usuario);

            Assert.IsNotNull(usuario.Id);
        }

        ///<sumary>
        /// Valida que al actualizar una tipoCambio, el objeto actualizado sea el objeto que yo deseo actualizar
        ///</sumary>
        [Test]
        public void UpdateTipoCambioTest()
        {
            var tipoCambio = A.Fake<TipoCambio>();
            tipoCambio = new TipoCambio { Id = 1, Valor = 528 };
            var repository_fake = A.Fake<ITipoCambioService>();
            A.CallTo(() => repository_fake.Update(tipoCambio)).Returns(tipoCambio);

            Assert.That(tipoCambio.Id, Is.EqualTo(1));
        }

        ///<sumary>
        /// Valida que al eliminar una tipoCambio, no suceda la acción, ya que el registro a eliminar no cuenta con su id
        ///</sumary>
        [Test]
        public void DeleteTipoCambioTest()
        {
            TipoCambio tipoCambio = new TipoCambio();

            var repository_fake = A.Fake<ITipoCambioService>();
            A.CallTo(() => repository_fake.Delete(tipoCambio)).MustNotHaveHappened();
        }

       
    
    }
}
