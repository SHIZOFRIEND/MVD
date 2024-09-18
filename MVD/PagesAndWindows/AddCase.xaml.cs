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

namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для AddCase.xaml
    /// </summary>
    public partial class AddCase : Window
    {
        private int? IDCaseType;
        private string SelectedCaseType;
        private int? IDCaseSt;
        private string SelectedCaseSt;
        public AddCase()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var employee = context.Employee.FirstOrDefault(emp => emp.Login == txtEmployeeLogin.Text);
                    if (employee == null)
                    {
                        MessageBox.Show("Сотрудник с указанным логином не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var newCase = new Model.Cases
                    {
                        IDCase = int.Parse(txtCaseNumber.Text),
                        IDEmployee = employee.IDEmployee,
                        IDCaseType = (int)IDCaseType,
                        IDCaseStatus = (int)IDCaseSt,
                        CaseCode = txtCaseCode.Text,
                        Description = txtCaseDescription.Text,
                        OpenDate = DateTime.Now  
                    };
                    context.Cases.Add(newCase);
                    context.SaveChanges();
                    MessageBox.Show("Дело успешно добавлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении дела: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtCaseNumber.Text = string.Empty;
            cb.SelectedItem = null;
            cbс.SelectedItem = null;
            txtCaseCode.Text = string.Empty;
            txtCaseDescription.Text = string.Empty;
            txtEmployeeLogin.Text = string.Empty;        
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb.SelectedItem != null)
            {
                var selectedComboBoxItem = (ComboBoxItem)cb.SelectedItem;
                SelectedCaseType = selectedComboBoxItem.Content.ToString();
                int caseId;
                if (int.TryParse(selectedComboBoxItem.Tag.ToString(), out caseId))
                {
                    IDCaseType = caseId;
                }
                else
                {
                    MessageBox.Show("Ошибка при получении ID отдела.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Combc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbс.SelectedItem != null)
            {
                var selectedComboBoxItem = (ComboBoxItem)cbс.SelectedItem;
                SelectedCaseSt = selectedComboBoxItem.Content.ToString();
                int casestId;
                if (int.TryParse(selectedComboBoxItem.Tag.ToString(), out casestId))
                {
                    IDCaseSt = casestId;
                }
                else
                {
                    MessageBox.Show("Ошибка при получении ID отдела.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
