using MVD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ImportExelWordGS;
using Page = System.Windows.Controls.Page;
using Microsoft.Office.Interop.Excel;
using System.ComponentModel;
using Microsoft.Office.Interop.Word;
using System.IO;
using System.Diagnostics;
namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        private string polzovFullName;
        public ObservableCollection<Employee> Employees { get; set; }
        public ObservableCollection<Rol> Roles { get; set; }
        private Employee _currentEmployee;
        public Admin(Employee polzov)
        {
            InitializeComponent();
            LoadData();
            _currentEmployee = polzov;
            DataContext = this;
            polzovFullName = $"{polzov.FirstName} {polzov.LastName} {polzov.LastestName}";
        }
        private void LoadData()
        {
            using (var context = new BazaDan())
            {
                Employees = new ObservableCollection<Employee>(context.Employee.ToList());
                Roles = new ObservableCollection<Rol>(context.Rol.ToList());
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddSotr addSotr = new AddSotr();
            bool? result = addSotr.ShowDialog();

            if (result == true)
            {
                LoadData();
            }
        }
        private void txtSearch_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            ICollectionView view = CollectionViewSource.GetDefaultView(LViewProduct.ItemsSource);

            if (view != null)
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    view.Filter = null;
                }
                else
                {

                    view.Filter = item =>
                    {
                        Employee dataItem = item as Employee;

                        if (dataItem != null)
                        {
                            string itemName = dataItem.FirstName.ToLower();
                            return itemName.Contains(searchText);
                        }

                        return false;
                    };
                }
            }
        }
        private void ApplySorting()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Employees);

            if (view != null)
            {
                ComboBoxItem selectedSortItem = cmbSorting.SelectedItem as ComboBoxItem;

                if (selectedSortItem != null && selectedSortItem.Tag != null)
                {
                    string[] sortParams = selectedSortItem.Tag.ToString().Split(',');
                    string sortProperty = sortParams[0];
                    ListSortDirection sortDirection = (ListSortDirection)Enum.Parse(typeof(ListSortDirection), sortParams[1]);

                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(sortProperty, sortDirection));
                }
            }
        }
        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySorting();
        }
        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LViewProduct.SelectedItem != null)
            {
                EditWindow editWindow = new EditWindow();
                editWindow.DataContext = LViewProduct.SelectedItem;
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    LoadData();
                }
            }
        }
        private void UpdateList_Click(object sender, RoutedEventArgs e)
        {
            Admin newAdminPage = new Admin(_currentEmployee);
            NavigationService.Navigate(newAdminPage);
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            ImportExelWordGS.MainWindow importex = new ImportExelWordGS.MainWindow();
           
            bool? result = importex.ShowDialog();

            if (result == true)
            {
                LoadData();
            }
        }
        private void Excelу_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb = excel.Workbooks.Add(XlSheetType.xlWorksheet);
            Worksheet ws = (Worksheet)excel.ActiveSheet;
            ws.Cells[1, 1] = "Имя";
            ws.Cells[1, 2] = "Фамилия";
            ws.Cells[1, 3] = "Отчество";
            ws.Cells[1, 4] = "Номер телефона";
            ws.Cells[1, 5] = "Почта";
            ws.Cells[1, 6] = "Серия паспорта";
            ws.Cells[1, 7] = "Номер паспорта";
            ws.Cells[1, 8] = "Назввание роли";
            ws.Cells[1, 9] = "Пароль";
            ws.Cells[1, 10] = "Логин";
            ws.Cells[1, 11] = "Дата рождения";
            ws.Cells[1, 12] = "Дата принятия на работу";
            ws.Cells[1, 13] = "Зарплата";
            ws.Cells[1, 14] = "Двухфакторная аутентификация";
            int row = 2;
            foreach (var sotrydnik in Employees)
            {
                var role = Roles.FirstOrDefault(r => r.IDRol == sotrydnik.IDRol);

                if (role != null)
                {
                        ws.Cells[row, 1] = sotrydnik.FirstName;
                        ws.Cells[row, 2] = sotrydnik.LastName;
                        ws.Cells[row, 3] = sotrydnik.LastestName;
                        ws.Cells[row, 4] = sotrydnik.NumberPhone;
                        ws.Cells[row, 5] = sotrydnik.Email;
                        ws.Cells[row, 6] = sotrydnik.PasportaSeria;
                        ws.Cells[row, 7] = sotrydnik.PasportNumber;
                        ws.Cells[row, 8] = role.RolName;
                        ws.Cells[row, 9] = sotrydnik.Password;
                        ws.Cells[row, 10] = sotrydnik.Login;
                        ws.Cells[row, 11] = sotrydnik.DateOfBirth;
                        ws.Cells[row, 12] = sotrydnik.HireDate;
                        ws.Cells[row, 13] = sotrydnik.Salary;
                        ws.Cells[row, 14] = sotrydnik.TwoFactorAvtor == 0 ? "Нет двухфакторной аутентификации" : "Есть двухфакторная аутентификация";
                        row++;
                }
            }
            try
            {
                string fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "СписокСотрудников.xlsx");
                wb.SaveAs(fileName);
                excel.Quit();
                MessageBox.Show("Данные экспортированы в Excel.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении файла Excel: " + ex.Message);
            }
        }
        private void plohishi_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Cases());
        }
        private void document_Click(object sender, RoutedEventArgs e)
        {
            Appeal(polzovFullName);
        }
        private void Appeal(string fullName)
        {
            var items = new Dictionary<string, string>()
            {

                {"<FIO>", fullName},
                {"<dir>", "А.Ю. Захарову"},

            };
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document wordDoc = null;
            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                object missing = System.Reflection.Missing.Value;
                string fileName = @"C:\Users\Egor\source\repos\MVD\MVD\Documents\appeal.docx";
               
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
                string newFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "обращение.docx");
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
        private void documentvk_Click(object sender, RoutedEventArgs e)
        {
            request(polzovFullName);

        }
        private void request(string fullName)
        {
            var items = new Dictionary<string, string>()
            {

                {"<FIO>", fullName},
                {"<dir>", "В.Ю. Русскову"},
                {"<currentdate>", DateTime.Now.ToString("dd.MM.yyyy")},
                {"<vpisat>","(вписать вручную)" }
            };
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document wordDoc = null;
            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                object missing = System.Reflection.Missing.Value;
                string fileName = @"C:\Users\Egor\source\repos\MVD\MVD\Documents\request.docx";
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
                string newFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Заявка в Вконтакте.docx");
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
        private void exic_Click(object sender, RoutedEventArgs e)
        {
            importExel.MainWindow importexcel = new importExel.MainWindow();

            bool? result = importexcel.ShowDialog();

            if (result == true)
            {
                LoadData();
            }
        }

        private void sites_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document wordDoc = null;
            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                string fileName = @"C:\Users\Egor\source\repos\MVD\MVD\Documents\Sites.docx";
                if (!File.Exists(fileName))
                {
                    MessageBox.Show("Файл не найден: " + fileName);
                    return;
                }
                wordDoc = wordApp.Documents.Open(fileName, ReadOnly: false, Visible: true);


                wordApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void testc(object sender, RoutedEventArgs e)
        {
            try
            {
                string consoleAppPath = @"C:\Users\Egor\source\repos\MVD\CheckConnection\obj\Debug\CheckConnection.exe";
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = consoleAppPath,
                    UseShellExecute = true
                };
                Process process = Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

