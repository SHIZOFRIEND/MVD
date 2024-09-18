using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MVD.PagesAndWindows;
using MVD.Mail;
using MVD.Model;
using HashPassword;

namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для Auto.xaml
    /// </summary>
    public partial class Auto : Page
    {
        private int countUnsuccessful = 0;
        
        public Auto()
        {
            InitializeComponent();
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Ghost());
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = txtbLogin.Text.Trim();
            string password = pswbPassword.Password;
            string passwort = HashPasswort.HashPassword(password.Trim().Trim());
            BazaDan bd = new BazaDan();
            Employee polzov = bd.Employee.FirstOrDefault(_ => _.Login == login && _.Password == passwort);
            int polzovCount = bd.Employee.Count(_ => _.Login == login && _.Password == passwort);
            if (polzov != null)
            {

                if (polzov.TwoFactorAvtor == 1)
                {
                    string userEmail = bd.Employee.FirstOrDefault(s => s.IDEmployee == polzov.IDEmployee)?.Email;
                    string confirmationCode = null;

                    if (userEmail != null)
                    {
                        
                         if (userEmail.Contains("@mail.ru"))
                        {
                            confirmationCode = MailRuMailSender.SendMailRu(userEmail);
                        }
                        else if (userEmail.Contains("@gmail.com"))
                        {
                            confirmationCode = GmailSender.SendGMail(userEmail);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Адрес электронной почты пользователя не найден.");
                        return;
                    }


                    ConfirmWindow confirmWindow = new ConfirmWindow(confirmationCode);
                    bool? result = confirmWindow.ShowDialog();


                    if (result == true)
                    {
                        string roleName = bd.Employee.First(_ => _.Login == login).Rol.RolName.ToString();
                        MessageBox.Show("Вы вошли под: " + roleName);
                        LoadForm(roleName);
                    }
                    else
                    {
                        MessageBox.Show("Введенный код неверный. Пожалуйста, попробуйте еще раз.");
                    }
                }
                else
                {

                    string roleName = bd.Employee.First(_ => _.Login == login).Rol.RolName.ToString(); 
                    MessageBox.Show("Вы вошли под: " + roleName);
                    LoadForm(roleName);
                }


                return;
            }

            if (countUnsuccessful < 3)
            {
                if (polzovCount > 0)
                {
                    SuccessfulLogin(polzov.Rol.RolName.ToString());
                    ClearFields();
                }
                else
                {
                    UnsuccessfulLogin();
                }
            }
            else
            {
                MessageBox.Show("ОШИБКА ВХОДА");
            }
        }
        private void UnsuccessfulLogin()
        {
            countUnsuccessful++;
          
            MessageBox.Show("Вы ввели неверный логин или пароль");
        }

        private void SuccessfulLogin(string roleName)
        {
            MessageBox.Show("Вы вошли под: " + roleName);
            LoadForm(roleName);
            ClearFields();
        }
        private void ClearFields()
        {
            txtbLogin.Clear();
            pswbPassword.Clear();
            
        }
        private void LoadForm(string roleName)
        {
            string login = txtbLogin.Text.Trim();
            string password = pswbPassword.Password;
            string passwort = HashPasswort.HashPassword(password.Trim().Trim());

            BazaDan bd = new BazaDan();
            Employee polzov = bd.Employee.FirstOrDefault(_ => _.Login == login && _.Password == passwort);
            switch (roleName)
            {
                case "Начальник управления":
                    NavigationService.Navigate(new Admin(polzov));
                    break;
                case "Заместитель начальника управления":
                    NavigationService.Navigate(new Admin(polzov));
                    break;
                case "Оперуполномоченный":
                    NavigationService.Navigate(new Admin(polzov));
                    break;
                case "Старший оперуполномоченный":
                    NavigationService.Navigate(new Admin(polzov));
                    break;
                case "Старший оперуполномоченный по ОВД":
                    NavigationService.Navigate(new Admin(polzov));
                    break;
            }
        }
        private void btnforget_Click(object sender, RoutedEventArgs e)
        {
            PasswordRecoveryWindow passwordRecoveryWindow = new PasswordRecoveryWindow();
            passwordRecoveryWindow.Show();
        }
    }
}
