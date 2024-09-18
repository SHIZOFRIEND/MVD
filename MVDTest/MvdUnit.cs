using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MVD;
using HashPassword;
using CheckConnection;
using MVD.Model;
 
namespace MVDTest
{
    [TestClass]
    public class MvdUnit
    {
        private const string ValidConnectionString = @"Data Source=DESKTOP-OIV0VNA\SQLEXPRESS;Initial Catalog=MVDBD;Integrated Security=True;";
        private const string InvalidConnectionString = @"Data Source=InvalidServer;Initial Catalog=InvalidDatabase;Integrated Security=True;";
        [TestMethod]
        public void HashPassword_CorrectInput_ReturnsCorrectHash()
        {
            string password = "myPassword123";
            string expectedHash = "71d4ec024886c1c8e4707fb02b46fd568df44e77dd5055cadc3451747f0f2716";
            string actualHash = HashPasswort.HashPassword(password);
            Assert.AreEqual(expectedHash, actualHash);
        }
        [TestMethod]
        public void HashPassword_EmptyInput_ReturnsEmptyHash()
        {
            string password = string.Empty;
            string expectedHash = string.Empty;
            string actualHash = HashPasswort.HashPassword(password);
            Assert.AreEqual(expectedHash, actualHash);
        }
        [TestMethod]
        public void TestValidConnection()
        {

            bool isConnected = Connection.CheckConnection(ValidConnectionString);

            Assert.IsTrue(isConnected, "Ожидалось успешное соединение.");
        }

        [TestMethod]
        public void TestInvalidConnection()
        {

            bool isConnected = Connection.CheckConnection(InvalidConnectionString);

            Assert.IsFalse(isConnected, "Ожидалась ошибка соединения.");
        }
        
    }
}
