using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using MVD.PagesAndWindows;
using HashPassword;
using System.CodeDom.Compiler;
using MVD.Mail;
using MVD.Model;
using System.IO;
using Moq;
using System.Collections.Generic;
using CheckConnection;
using AddEmployee;
using static AddEmployee.AddEmployee;


namespace MVDUnitTests
{
    [TestClass]
    public class UnitTest1
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
 
 
   
