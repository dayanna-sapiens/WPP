using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Tests
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TrueTest() // PRUEBA CORRECTA
        {
            Assert.IsTrue(true);
        }
        
        [Test]
        public void AreTheValuesEqual() // PRUEBA CORRECTA
        {
            Assert.AreEqual(10, 4 + 6);
        }

        [Test]
        public void AreTheValuesTheSame() // PRUEBA ERROR
        {           
           Assert.AreSame(10, 3 + 6, "No son iguales");
        }

        [Test, Pairwise]
        public void CombinatorialTest([Values("a", "b", "c")] string a, [Values("+", "-")] string b, [Values("x", "y")] string c)
        {
            Console.WriteLine("{0} {1} {2}", a, b, c);
        }

        [TestCase(12, 3, 4)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 4, 3)]
        public void DivideTest(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }
        
        [Test]
        public void ConditionTest() // PRUEBA COORECTA
        {
            Assert.That(5 >= 2);
        }

        [Test]
        public void NunitAndFakeItEasyTest() // PRUEBA CON FAKEITEASY
        {
            // Arrange
            var account = A.Fake<IAccount>();
            A.CallTo(() => account.PasswordMatches(A<string>.Ignored)).Returns(true);
            A.CallTo(() => account.PasswordMatches(A<string>._)).Returns(true);// El A<string>.Ignored y A<string>._ funcionan igual
            
            var accountRepository = A.Fake<IAccountRepository>();
            A.CallTo(() => accountRepository.Find("username")).Returns(account);

            var service = new LoginService(accountRepository);

            // Act
            service.Login("username", "password");

            // Assert
            A.CallTo(() => account.SetLoggedIn(false)).MustNotHaveHappened();
            A.CallTo(() => account.SetLoggedIn(true)).MustHaveHappened();
          //A.CallTo(() => account.SetLoggedIn(false)).MustHaveHappened(Repeated.Exactly.Once); // Esto hace fallar el test

            A.CallTo(account).Where(call => call.Method.Name == "PasswordMatches").Throws(new Exception("Ejemplo Excepcion"));
            A.CallTo(account).WithReturnType<IAccount>();
            A.CallTo(account).WithReturnType<bool>().Returns(true);
        }
    }

    public interface IAccount
    {
        bool PasswordMatches(string password);
        void SetLoggedIn(bool isLoggedIn);
    }

    public interface IAccountRepository
    {
        IAccount Find(string username);
    }

    public class LoginService
    {
        private IAccountRepository accountRepository;

        public LoginService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Login(string username, string password)
        {
            var account = this.accountRepository.Find(username);
            account.SetLoggedIn(true);
        }
    }

}
