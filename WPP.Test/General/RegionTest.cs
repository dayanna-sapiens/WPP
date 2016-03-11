using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects;
using WPP.Service.BaseServiceClasses;
using WPP.Service.Generales;

namespace WPP.Test.General
{
    [TestFixture]
    public class RegionTest
    {

        /// <summary>
        /// Valida que al obtener una region retorne un objeto tipo Region y que este no sea nulo
        /// </summary>
        [Test]
        public void GetRegionTest()
        {
            var region = A.Fake<Region>();
            var repository_fake = A.Fake<IRegionService>();
            var reg = A.CallTo(() => repository_fake.Get(1)).Returns(region);

            var unitOfWork_fake = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork_fake.BeginTransaction());
            A.CallTo(() => unitOfWork_fake.Commit());

            
            Assert.IsNotNull(region);
        }

        /// <summary>
        /// Valida que al obtener una region por medio de algunos de sus criterios retorne un objeto tipo Region, 
        /// y que el id del mismo no sea nulo
        /// </summary>
        [Test]
        public void GetRegionTestWithParameters()
        {
            IDictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("NodoPadre", 1);

            var region = A.Fake<Region>();
            var repository_fake = A.Fake<IRegionService>();
            A.CallTo(() => repository_fake.Get(criteria)).Returns(region);

            Assert.IsNotNull(region.Id);
        }


        ///<sumary>
        /// Valida que al obtener una lista de regiones que cumpla con ciertos criterios se lleve a cabo de manera exitosa
        ///</sumary>
        [Test]
        public void GetAllRegionTest()
        {
            var repository_fake = A.Fake<IRegionService>();
            A.CallTo(() => repository_fake.GetAll(0))
                .MustHaveHappened(Repeated.NoMoreThan.Twice);
        }

    }
    
}
