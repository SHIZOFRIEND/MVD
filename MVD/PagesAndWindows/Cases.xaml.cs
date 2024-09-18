using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using MVD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Page = System.Windows.Controls.Page;
using System.IO;
using System.Diagnostics;
namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для Cases.xaml
    /// </summary>
    public partial class Cases : Page
    {
        public ObservableCollection<Model.Cases> Case { get; set; }
        public ObservableCollection<Model.CaseType> CaseTypes { get; set; }
        public ObservableCollection<Model.CaseStatus> CaseStatuses { get; set; }
        public ObservableCollection<Model.Employee> Employees { get; set; }
        public Cases()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
        }
        private void LoadData()
        {
            using (var context = new BazaDan())
            {
                Case = new ObservableCollection<Model.Cases>(context.Cases.ToList());
                CaseTypes = new ObservableCollection<CaseType>(context.CaseType.ToList());
                CaseStatuses = new ObservableCollection<Model.CaseStatus>(context.CaseStatus.ToList());
                Employees = new ObservableCollection<Employee>(context.Employee.ToList());
            }
        }
        private void Excelу_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Plohishi());
        }
        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document wordDoc = null;
            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                string fileName = @"C:\Users\Egor\source\repos\MVD\MVD\Documents\minirovanya.docx";
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddCase addSotr = new AddCase();
            bool? result = addSotr.ShowDialog();

            if (result == true)
            {
                LoadData();
            }
        }
        private void ApplySorting()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Case);

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
                        Model.Cases dataItem = item as Model.Cases;

                        if (dataItem != null)
                        {
                            string itemName = dataItem.Description.ToLower();
                            return itemName.Contains(searchText);
                        }

                        return false;
                    };
                }
            }
        }
        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LViewProduct.SelectedItem != null)
            {
                EditCase editWindow = new EditCase();
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
            Cases newAdminPage = new Cases();
            NavigationService.Navigate(newAdminPage);
        }
        private void Excel_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document wordDoc = null;
            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                string fileName = @"C:\Users\Egor\source\repos\MVD\MVD\Documents\plohishi.docx";
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
        private void Excelcase_Click(object sender, RoutedEventArgs e)
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = excelApp.Workbooks.Add(XlSheetType.xlWorksheet);
            Worksheet worksheet = (Worksheet)excelApp.ActiveSheet;
            worksheet.Cells[1, 1] = "Код дела";
            worksheet.Cells[1, 2] = "Описание";
            worksheet.Cells[1, 3] = "Дата открытия дела";
            worksheet.Cells[1, 4] = "Дата закрытия дела";
            worksheet.Cells[1, 5] = "Тип дела";
            worksheet.Cells[1, 6] = "Cтатус дела";
            worksheet.Cells[1, 7] = "Сотрудник, открывший дело";
            int row = 2;
            foreach (var cases in Case)
            {
                var employee = Employees.FirstOrDefault(r => r.IDEmployee == cases.IDEmployee);
                var caseType = CaseTypes.FirstOrDefault(r => r.IDCaseType == cases.IDCaseType);
                var caseStatus = CaseStatuses.FirstOrDefault(r => r.IDCaseStatus == cases.IDCaseStatus);
                worksheet.Cells[row, 1] = cases.CaseCode;
                worksheet.Cells[row, 2] = cases.Description;
                worksheet.Cells[row, 3] = cases.OpenDate?.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 4] = cases.CloseDate?.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 5] = caseType?.CaseType1;
                worksheet.Cells[row, 6] = caseStatus?.CaseStatus1;
                worksheet.Cells[row, 7] = employee.FirstName + " " + employee.LastName + " " + employee.LastestName;
                row++;
            }
            try
            {
                string fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Дела.xlsx");
                workbook.SaveAs(fileName);
                excelApp.Quit();
                MessageBox.Show("Данные успешно экспортированы.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибочка в сохранении: " + ex.Message);
            }
        }
        private void Formater_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string exePath = @"C:\Users\Egor\source\repos\MVD\MVD\ExeFiles\VKFormater.exe";
                Process.Start(exePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске exe файла: {ex.Message}");
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
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
