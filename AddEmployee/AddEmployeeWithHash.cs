using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashPassword;
using MVD.Model;

namespace AddEmployee
{
    public class AddEmployee
    {

        public static void Main(string[] args)
        {
            string connectionString = @"Data Source=DESKTOP-OIV0VNA\SQLEXPRESS;Initial Catalog=MVDBD;Integrated Security=True;";
            AddEmployeeWithHash.Execute(connectionString);
        }
    }
    public class AddEmployeeWithHash
    {
        public static void Execute(string connectionString)
        {
            Console.Write("Введите Login: ");
            string login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите отчество: ");
            string lastestName = Console.ReadLine();
            Console.Write("Введите номер телефона: ");
            string phone = Console.ReadLine();
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();
            Console.Write("Введите id роли: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите id отдела: ");
            int iddep = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите наличие двухфакторной аутефикации(1-включить,0- выключить): ");
            int twofactoravtor = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите адрес электронной почты: ");
            string mail = Console.ReadLine();
            string hashedPassword = HashPasswort.HashPassword(password);
            Console.WriteLine("Хешированный пароль: " + hashedPassword);
            Console.Write("Введите серию паспорта: ");
            string seriapasporta = Console.ReadLine();
            Console.Write("Введите номер паспорта: ");
            string passportnumber = Console.ReadLine();
            Console.Write("Введите зарплату: ");
            string salary = Console.ReadLine();
            Console.Write("Введите дату рождения: ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime birthDate))
            {
                Console.WriteLine($"Вы ввели дату рождения: {birthDate.ToShortDateString()}");
            }
            else
            {
                Console.WriteLine("Неверный формат даты.");
            }
            Console.Write("Введите дату принятия на работу: ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime hiredate))
            {
                Console.WriteLine($"Вы ввели дату принятия на работу: {hiredate.ToShortDateString()}");
            }
            else
            {
                Console.WriteLine("Неверный формат даты.");
            }
            using (var context = new BazaDan())
            {
                context.Database.Connection.ConnectionString = connectionString;  
                var newUser = new Employee()
                {
                    Login = login,
                    Password = hashedPassword,
                    LastName = lastName,
                    FirstName = firstName,
                    IDRol = id,
                    TwoFactorAvtor = twofactoravtor,
                    LastestName = lastestName,
                    NumberPhone = phone,
                    IDDepartments = iddep,
                    Email = mail,
                    DateOfBirth = birthDate,
                    HireDate = hiredate,
                    PasportaSeria = seriapasporta,
                    PasportNumber = passportnumber,
                    Salary = salary,
                };
                context.Employee.Add(newUser);
                context.SaveChanges();
            }
            Console.WriteLine("Пользователь успешно добавлен в базу данных.");
        }
    }
}