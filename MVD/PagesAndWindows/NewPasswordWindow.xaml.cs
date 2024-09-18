using MVD.Model;
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
using System.Windows.Shapes;
using HashPassword;

namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для NewPasswordWindow.xaml
    /// </summary>
    public partial class NewPasswordWindow : Window
    {
        private string userEmail;
        public NewPasswordWindow(string userEmail)
        {
            InitializeComponent();
            this.userEmail = userEmail;
        }
        private void SaveNewPassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, попробуйте снова.");
                return;
            }
            ChangePassword(userEmail, newPassword);
        }
        private void ChangePassword(string userEmail, string newPassword)
        {
            try
            {
                using (BazaDan bd = new BazaDan())
                {
                    Employee employee = bd.Employee.FirstOrDefault(e => e.Email == userEmail);
                    if (employee != null)
                    {
                        employee.Password = HashPasswort.HashPassword(newPassword);
                        bd.SaveChanges();
                        MessageBox.Show("Пароль успешно изменен.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Сотрудник с указанной почтой не найден.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при изменении пароля: " + ex.Message);
            }
        }
    }
}
