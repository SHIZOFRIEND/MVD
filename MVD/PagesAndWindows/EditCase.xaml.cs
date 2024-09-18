using MVD.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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

namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для EditCase.xaml
    /// </summary>
    public partial class EditCase : Window
    {
        public Model.Cases CaseData { get; set; }
        public Employee EmployeeData { get; set; }
        public EditCase()
        {
            InitializeComponent();
            DataContext = this;
            LoadCaseType();
            LoadCaseStatus();
            using (var dbContext = new BazaDan())
            {
                CaseData = dbContext.Cases.FirstOrDefault();
                EmployeeData = dbContext.Employee.FirstOrDefault(emp => emp.IDEmployee == CaseData.IDEmployee);
                txtCaseNumber.Text = CaseData.IDCase.ToString();
                txtCaseCode.Text = CaseData.CaseCode;
                txtCaseDescription.Text = CaseData.Description;
                txtEmployeeName.Text = $"{EmployeeData.LastName} {EmployeeData.FirstName} {EmployeeData.LastestName}";
            }
        }
        private void LoadCaseType()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var caseTypes = context.CaseType.ToList();
                    cbCaseType.ItemsSource = caseTypes;
                    cbCaseType.DisplayMemberPath = "CaseType1";
                    cbCaseType.SelectedValuePath = "IDCaseType";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке типов дела: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadCaseStatus()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var caseStatuses = context.CaseStatus.ToList();
                    cbCaseStatus.ItemsSource = caseStatuses;
                    cbCaseStatus.DisplayMemberPath = "CaseStatus1";
                    cbCaseStatus.SelectedValuePath = "IDCaseStatus";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статусов дела: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCaseNumber.Text) ||
        cbCaseType.SelectedItem == null ||
        cbCaseStatus.SelectedItem == null ||
        string.IsNullOrWhiteSpace(txtCaseCode.Text) ||
        string.IsNullOrWhiteSpace(txtCaseDescription.Text) ||
        string.IsNullOrWhiteSpace(txtEmployeeName.Text))
            {
                MessageBox.Show("Заполните все обязательные поля перед сохранением.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                using (var context = new BazaDan())
                {
                    int idCase;
                    if (!int.TryParse(txtCaseNumber.Text, out idCase))
                    {
                        MessageBox.Show("Невозможно получить идентификатор дела.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Model.Cases existingCase = context.Cases.FirstOrDefault(c => c.IDCase == idCase);
                    if (existingCase == null)
                    {
                        MessageBox.Show("Дело не найдено.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    existingCase.IDCaseType = (int)cbCaseType.SelectedValue;
                    existingCase.IDCaseStatus = (int)cbCaseStatus.SelectedValue;
                    existingCase.CaseCode = txtCaseCode.Text;
                    existingCase.Description = txtCaseDescription.Text;
                    context.SaveChanges();
                }
                MessageBox.Show("Данные успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void cbtr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtCaseNumber.Text = string.Empty;
            cbCaseType.SelectedItem = null;
            cbCaseStatus.SelectedItem = null;
            txtCaseCode.Text = string.Empty;
            txtCaseDescription.Text = string.Empty;
            txtEmployeeName.Text = string.Empty;      
        }
        private void about(object sender, RoutedEventArgs e)
        {
            Model.Cases currentCase = DataContext as Model.Cases;

            if (currentCase != null)
            {
                AboutCase serviceItemWindow = new AboutCase(currentCase, currentCase.IDCase);
                serviceItemWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не удалось передать контекст пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
