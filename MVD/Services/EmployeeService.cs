using MVD.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HashPassword;

namespace MVD.Services
{
    public class EmployeeService
    {
        public (bool IsValid, string ErrorMessage) ValidateAndCreateEmployee(
           string firstName, string lastName, string lastestName, string numberPhone,
           string email, int idRole,int idDepartment, string pasportaSeria, string pasportNumber,
           string password, string login, DateTime dateOfBirth, DateTime hireDate, string salary, int twofactor)
        {
            Employee newSotrydnik = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                LastestName = lastestName,
                NumberPhone = numberPhone,
                Email = email,
                IDRol = idRole,
                IDDepartments = idDepartment,
                PasportaSeria = pasportaSeria,
                PasportNumber = pasportNumber,
                Password = HashPassword(password),
                Login = login,
                DateOfBirth = dateOfBirth,
                HireDate = hireDate,
                Salary = salary,
                TwoFactorAvtor = twofactor,
            };
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newSotrydnik, new ValidationContext(newSotrydnik), validationResults, true))
            {
                StringBuilder errorMessage = new StringBuilder("Введены некорректные данные. Пожалуйста, проверьте введенные значения:\n");
                foreach (var validationResult in validationResults)
                {
                    errorMessage.AppendLine(validationResult.ErrorMessage);
                }
                return (false, errorMessage.ToString());
            }
            return (true, string.Empty);
        }

        public string HashPassword(string password)
        {
            return HashPasswort.HashPassword(password.Trim());
        }
    }
}