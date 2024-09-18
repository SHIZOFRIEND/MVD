using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MVD.Services;
using CheckConnection;
using HashPassword;
namespace MVDUnitTesting
{
    [TestClass]
    public class MVDTests
    {
        private EmployeeService employeeService;
        public string ValidConnectionString = @"Data Source=DESKTOP-OIV0VNA\SQLEXPRESS;Initial Catalog=MVDBD;Integrated Security=True;";
        public string InvalidConnectionString = @"Data Source=DESKTOP-OIV0VNA\SQLEXPRESSsss;Initial Catalog=MVDBD;Integrated Security=True;";
        [TestInitialize]
        public void Setup()
        {
            employeeService = new EmployeeService();
        }
        [TestMethod]
        public void EmployeeReturnsValid()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Егорик", "Антонов", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1,1, "1234", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек",0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsTrue(result.IsValid, result.ErrorMessage);
        }
        [TestMethod]
        public void EmployeeInvalidFirstName()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Егорикeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee", "Антонов", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Имя"));
        }
        [TestMethod]
        public void EmployeeEmptyFirstName()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "", "Антонов", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Имя"));
        }
        [TestMethod]
        public void EmployeeEmptyLastName()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Фамилия"));
        }
        [TestMethod]
        public void EmployeeInvalidLastName()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Фамилия"));
        }
        [TestMethod]
        public void EmployeeInvalidLastestName()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Иванооооооооооооооооооооооооооооооооооооооооооооооооооооооооооовичччччччччччччччччччччччччччччччччччччччччччччч", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Отчество"));
        }
        [TestMethod]
        public void EmployeeInvalidPhoneNumber()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Иванович", "89618701902", "egor@gmail.com",
                1, 1, "1234", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Некорректный формат номера телефона"));
        }
        [TestMethod]
        public void EmployeeEmptyPhoneNumber()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Иванович", "", "egor@gmail.com",
                1, 1, "1234", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Номер телефона"));
        }
        [TestMethod]
        public void EmployeeInvalidEmail()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Антонович", "+7(961)870-19-02", "egorццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццццц@gmail.com",
                1, 1, "1234", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Некорректный формат адреса электронной почты"));
        }
        [TestMethod]
        public void EmployeeEmptyEmail()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Антонович", "+7(961)870-19-02", "",
                1, 1, "1234", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Адрес электронной почты является обязательным полем"));
        }
        [TestMethod]
        public void EmployeeInvalidSeriaPasporta()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "123431", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Серия паспорта"));
        }
        [TestMethod]
        public void EmployeeEmptySeriaPasporta()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "", "567890", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Серия паспорта"));
        }
        [TestMethod]
        public void EmployeeEmptyNomerPasporta()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Номер паспорта"));
        }
        [TestMethod]
        public void EmployeeInvalidNomerPasporta()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "Password123", "egorik",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Номер паспорта"));
        }
        [TestMethod]
        public void EmployeeInvalidLogin()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "122345", "Password123", "egoriddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Логин"));
        }
        [TestMethod]
        public void EmployeeEmptyLogin()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "122345", "Password123", "",
                DateTime.Now.AddYears(-30), DateTime.Now, "5 денюжек", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Логин"));
        }
        [TestMethod]
        public void EmployeeEmptySalary()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "122345", "Password123", "kiri",
                DateTime.Now.AddYears(-30), DateTime.Now, "", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Зарплата"));
        }
        [TestMethod]
        public void EmployeeInvalidSalary()
        {
            var result = employeeService.ValidateAndCreateEmployee(
                "Кирилл", "Свидерский", "Антонович", "+7(961)870-19-02", "egor@gmail.com",
                1, 1, "1234", "122345", "Password123", "kiri",
                DateTime.Now.AddYears(-30), DateTime.Now, "12222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222223", 0);
            if (!result.IsValid)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Поле зарплата должен быть от 5 до 20 символов"));
        }
        [TestMethod]
        public void HashCorrect()
        {
            string password = "12340";
            string exp = "3ba067469805939235e0d4e553501c05c8ad33a79ad21710d174d448bfb6409b";
            string act = HashPasswort.HashPassword(password);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void HashInCorrect()
        {
            string password = "123401";
            string exp = "3ba067469805939235e0d4e553501c05c8ad33a79ad21710d174d448bfb6409b";
            string act = HashPasswort.HashPassword(password);
            Assert.AreNotEqual(exp, act);
        }
        [TestMethod]
        public void HashEmpty()
        {
            string password = string.Empty;
            string exp = string.Empty;
            string act = HashPasswort.HashPassword(password);
            Assert.AreNotEqual(exp, act);
        }
        [TestMethod]
        public void CheckConnectionCorrected()
        {
            bool isConnected = Connection.CheckConnection(ValidConnectionString);
            Assert.IsTrue(isConnected, "Соединение с базой данных успешно.");
        }
        [TestMethod]
        public void CheckConnectionIncoreccted()
        {
            bool isConnected = Connection.CheckConnection(InvalidConnectionString);
            Assert.IsFalse(isConnected, "Соединение с базой данных успешно.");
        }
    }
}
