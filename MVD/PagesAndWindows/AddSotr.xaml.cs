using HashPassword;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using MVD.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
using System.IO;
using Microsoft.Office.Interop.Word;
using Window = System.Windows.Window;
using MVD.Services;
namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для AddSotr.xaml
    /// </summary>
    public partial class AddSotr : Window
    {
        private int? IDRol;
        private string SelectedRoleName;
        private int? IDDep;
        private string SelectedDepName;
        private int? IDtwo;
        private string Selectedtwo;
        private readonly EmployeeService employeeService;
        public AddSotr()
        {
            InitializeComponent();
            employeeService = new EmployeeService();
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
            string.IsNullOrWhiteSpace(txtdata.Text) ||
            string.IsNullOrWhiteSpace(txtdatahire.Text) ||
            string.IsNullOrWhiteSpace(txtsalary.Text))
            {
                MessageBox.Show("Заполните все обязательные поля перед сохранением.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int idPolzovatelia = 0;
            try
            {
                string password = txtpassword.Text;
                string passwort = HashPasswort.HashPassword(password.Trim().Trim());
                var result = employeeService.ValidateAndCreateEmployee(
                   txtImya.Text, txtfamilia.Text, txtotchestvo.Text, txtNumberPhone.Text,
                   txtPochta.Text, IDRol.Value, IDDep.Value, txtseria.Text, txtnomerpasporta.Text,
                   txtpassword.Text, txtlogin.Text, Convert.ToDateTime(txtdata.Text),
                   Convert.ToDateTime(txtdatahire.Text), txtsalary.Text, IDtwo.Value);
                if (!result.IsValid)
                {
                    MessageBox.Show(result.ErrorMessage, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (var context = new BazaDan())
                {
                    Employee newSotrydnik = new Employee
                    {
                        FirstName = txtImya.Text,
                       LastName = txtfamilia.Text,
                        LastestName = txtotchestvo.Text,
                        NumberPhone = txtNumberPhone.Text,
                        Email = txtPochta.Text,
                        IDRol = (int)IDRol,
                        IDEmployee = idPolzovatelia,
                        PasportaSeria = txtseria.Text,
                        PasportNumber = txtnomerpasporta.Text,
                        Password= passwort,
                        Login = txtlogin.Text,
                        IDDepartments = (int)IDDep,
                        DateOfBirth = Convert.ToDateTime(txtdata.Text),
                        HireDate = Convert.ToDateTime(txtdatahire.Text),
                        Salary = txtsalary.Text,
                        TwoFactorAvtor = (int)IDtwo,    
                    };

                    var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    if (!Validator.TryValidateObject(newSotrydnik, new ValidationContext(newSotrydnik), validationResults, true))
                    {
                        StringBuilder errorMessage = new StringBuilder("Введены некорректные данные. Пожалуйста, проверьте введенные значения:\n");

                        foreach (var validationResult in validationResults)
                        {
                            errorMessage.AppendLine(validationResult.ErrorMessage);
                        }
                        MessageBox.Show(errorMessage.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    context.Employee.Add(newSotrydnik);
                    context.SaveChanges();   
                }
                string seriaPasporta = txtseria.Text;
                string familia = txtfamilia.Text;
                string imya = txtImya.Text;
                string otchestvo = txtotchestvo.Text;
                string nomerpasporta = txtnomerpasporta.Text;
                string salary = txtsalary.Text;
                Dogovor(seriaPasporta, familia, imya, otchestvo, nomerpasporta, salary, SelectedRoleName);
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
            cb.SelectedItem = null;
            cbtwofactor.SelectedItem = null;
            cbdep.SelectedItem = null;
            txtdata.Text = string.Empty;
            txtdatahire.Text = string.Empty;
            txtsalary.Text = string.Empty;
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
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb.SelectedItem != null)
            {
                var selectedComboBoxItem = (ComboBoxItem)cb.SelectedItem;
                SelectedRoleName = selectedComboBoxItem.Content.ToString();
                int roleId;
                if (int.TryParse(selectedComboBoxItem.Tag.ToString(), out roleId))
                {
                    IDRol = roleId;
                }
                else
                {
                    MessageBox.Show("Ошибка при получении ID отдела.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ComboBoxdep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbdep.SelectedItem != null)
            {
                var selectedComboBoxItem = (ComboBoxItem)cbdep.SelectedItem;
                SelectedDepName = selectedComboBoxItem.Content.ToString();
                int depId;
                if (int.TryParse(selectedComboBoxItem.Tag.ToString(), out depId))
                {
                    IDDep = depId;
                }
                else
                {
                    MessageBox.Show("Ошибка при получении ID роли.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ComboBoxtwofac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbtwofactor.SelectedItem != null)
            {
                var selectedComboBoxItem = (ComboBoxItem)cbtwofactor.SelectedItem;
                Selectedtwo = selectedComboBoxItem.Content.ToString();
                int twoId;
                if (int.TryParse(selectedComboBoxItem.Tag.ToString(), out twoId))
                {
                    IDtwo = twoId;
                }
                else
                {
                    MessageBox.Show("Ошибка при получении статуса двухфакторной аутефикации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Dogovor(string seriaPasporta, string familia, string imya, string otchestvo, string nomerpasporta, string roleName, string salary)
        {
            var items = new Dictionary<string, string>()
            {
                {"<gorod>", "Новосибирск"},
                {"<currentdate>", DateTime.Now.ToString("dd.MM.yyyy")},
                {"<number>", "1"},
                {"<comp>", "УБК ГУ МВД России по Новосибирской области"},
                {"<dir>", "Антонов Е.А"},
                {"<sotr>", imya+familia+otchestvo},
                {"<address>", "Серебренниковская 40"},
                {"<kpp>", "123456789"},
                {"<zap>", salary+"рублей" },
                {"<data>", "12"},
                {"<mvd>", "ГУ МВД России по Новосибирску"},
                {"<seria>", seriaPasporta},
                {"<imya>", imya},
                {"<fam>", familia},
                {"<otchwestvo>", otchestvo},
                {"<nomerpasp>", nomerpasporta},
                {"<role>", roleName}
            };
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document wordDoc = null;
            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                object missing = System.Reflection.Missing.Value;
                string fileName = @"C:\Users\Egor\source\repos\MVD\MVD\Documents\blank.docx";
             
                if (!File.Exists(fileName))
                {
                    MessageBox.Show("Файл не найден: " + fileName);
                    return;
                }
                wordDoc = wordApp.Documents.Open(fileName, ReadOnly: false, Visible: true);
                foreach (var item in items)
                {
                    object findText = item.Key;
                    object replaceText = item.Value;
                    Microsoft.Office.Interop.Word.Range myRange = wordDoc.Content;
                    myRange.Find.ClearFormatting();
                    myRange.Find.Execute(FindText: findText, ReplaceWith: replaceText, Replace: WdReplace.wdReplaceAll);
                }
                string newFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "trudovogo-dogovora.docx");
                wordDoc.SaveAs2(newFilePath);
                MessageBox.Show("Документ успешно сохранен: " + newFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
            finally
            {
                wordDoc?.Close();
                wordApp?.Quit();
            }
        }
    }
}
