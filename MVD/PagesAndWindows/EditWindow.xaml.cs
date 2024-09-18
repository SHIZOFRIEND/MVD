using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using MVD.Model;
using MVD.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Window = System.Windows.Window;

namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private readonly EmployeeService employeeService;
       
        public EditWindow()
        {
            InitializeComponent();
            txtid.Visibility = Visibility.Hidden;
            LoadDepartments();
            LoadRoles();
            LoadTwoFactorAuthentication();
            DataContext = this;
            employeeService = new EmployeeService();
        }
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        private void LoadRoles()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var roles = context.Rol.ToList();
                    cbr.ItemsSource = roles;
                    cbr.DisplayMemberPath = "RolName";
                    cbr.SelectedValuePath = "IDRol";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке ролей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadDepartments()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var departments = context.Departments.ToList();
                    cbd.ItemsSource = departments;
                    cbd.DisplayMemberPath = "DepartmentName";
                    cbd.SelectedValuePath = "IDDepartment";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке отделов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadTwoFactorAuthentication()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    if (int.TryParse(txtid.Text, out int employeeId))
                    {
                        int? twoFactorAuthStatus = context.Employee
                            .Where(e => e.IDEmployee == employeeId)
                            .Select(e => e.TwoFactorAvtor)
                            .FirstOrDefault();
                        if (twoFactorAuthStatus.HasValue)
                        {
                            cbtwofactor.SelectedValue = twoFactorAuthStatus.Value;
                        }
                        else
                        {
                            cbtwofactor.SelectedValue = null;
                        }
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статуса двухфакторной аутентификации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImya.Text) ||
             string.IsNullOrWhiteSpace(txtfamilia.Text) ||
             string.IsNullOrWhiteSpace(txtotchestvo.Text) ||
             string.IsNullOrWhiteSpace(txtNumberPhone.Text) ||
             string.IsNullOrWhiteSpace(txtseria.Text) ||
             string.IsNullOrWhiteSpace(txtnomerpasporta.Text) ||
             string.IsNullOrWhiteSpace(txtPochta.Text) ||
             string.IsNullOrWhiteSpace(txtlogin.Text) ||
             string.IsNullOrWhiteSpace(txtpassword.Text) ||
             string.IsNullOrWhiteSpace(txtdatabir.Text) ||
             string.IsNullOrWhiteSpace(txtdatahire.Text) ||
             string.IsNullOrWhiteSpace(txtsalary.Text))
            {
                MessageBox.Show("Заполните все обязательные поля перед сохранением.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                
               var result = employeeService.ValidateAndCreateEmployee(
                 txtImya.Text, txtfamilia.Text, txtotchestvo.Text, txtNumberPhone.Text,
                  txtPochta.Text, Convert.ToInt32(cbr.SelectedValue), Convert.ToInt32(cbd.SelectedValue), txtseria.Text, txtnomerpasporta.Text,
                  txtpassword.Text, txtlogin.Text, Convert.ToDateTime(txtdatabir.Text),
                 Convert.ToDateTime(txtdatahire.Text), txtsalary.Text, Convert.ToInt32(cbtwofactor.SelectedValue));
                   if (!result.IsValid)
                {
                  MessageBox.Show(result.ErrorMessage, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (var context = new BazaDan())
                {
                    int idSotrydnika;
                    if (!int.TryParse(txtid.Text, out idSotrydnika))
                    {
                        MessageBox.Show("Невозможно получить идентификатор сотрудника.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Employee existingSotrydnik = context.Employee.FirstOrDefault(s => s.IDEmployee == idSotrydnika);

                    if (existingSotrydnik == null)
                    {
                        MessageBox.Show("Сотрудник не найден.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    existingSotrydnik.IDEmployee = idSotrydnika;
                    existingSotrydnik.FirstName = txtImya.Text;
                    existingSotrydnik.LastName = txtfamilia.Text;
                    existingSotrydnik.LastestName = txtotchestvo.Text;
                    existingSotrydnik.NumberPhone = txtNumberPhone.Text;
                    existingSotrydnik.Email = txtPochta.Text;
                    existingSotrydnik.IDRol = (int)cbr.SelectedValue;
                    existingSotrydnik.PasportaSeria = txtseria.Text;
                    existingSotrydnik.PasportNumber = txtnomerpasporta.Text;
                    existingSotrydnik.Password = txtpassword.Text;  
                    existingSotrydnik.Login = txtlogin.Text;
                    existingSotrydnik.IDDepartments = (int)cbd.SelectedValue;
                    existingSotrydnik.DateOfBirth=Convert.ToDateTime(txtdatabir.Text);
                    existingSotrydnik.HireDate = Convert.ToDateTime(txtdatahire.Text);
                    existingSotrydnik.TwoFactorAvtor = (int)cbtwofactor.SelectedValue;
                    existingSotrydnik.Salary=txtsalary.Text;
                    var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    if (!Validator.TryValidateObject(existingSotrydnik, new ValidationContext(existingSotrydnik), validationResults, true))
                    {
                        StringBuilder errorMessage = new StringBuilder("Введены некорректные данные. Пожалуйста, проверьте введенные значения:\n");

                        foreach (var validationResult in validationResults)
                        {
                            errorMessage.AppendLine(validationResult.ErrorMessage);
                        }

                        MessageBox.Show(errorMessage.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    context.SaveChanges();
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            txtImya.Text = string.Empty;
            txtfamilia.Text = string.Empty;
            txtotchestvo.Text = string.Empty;
            txtNumberPhone.Text = string.Empty;
            txtPochta.Text = string.Empty;
            txtnomerpasporta.Text = string.Empty;
            txtseria.Text = string.Empty;
            txtlogin.Text = string.Empty;
            txtpassword.Text = string.Empty;
            imgPhoto.Source = null;
            cbr.SelectedItem = null;
            cbtwofactor.SelectedItem = null;
            cbd.SelectedItem = null;
            txtdatabir.Text = string.Empty;
            txtdatahire.Text = string.Empty;
            txtsalary.Text = string.Empty;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту карточку?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new BazaDan())
                    {
                        Employee selectedSotrydnik = context.Employee.Find(((Employee)DataContext).IDEmployee) ;

                        if (selectedSotrydnik != null)
                        {
                            context.Employee.Remove(selectedSotrydnik);
                            context.SaveChanges();
                        }
                    }
                    MessageBox.Show("Карточка удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
            }
        }
        private void service_Click(object sender, RoutedEventArgs e)
        {
            Employee currentEmployee = DataContext as Employee;

            if (currentEmployee != null)
            {
                ServiceItem serviceItemWindow = new ServiceItem(currentEmployee, currentEmployee.IDEmployee);
                serviceItemWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не удалось передать контекст пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void ComboBoxtwofac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void cbr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void cbd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
