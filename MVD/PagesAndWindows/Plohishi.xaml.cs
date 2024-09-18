using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using MVD.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
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
namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для Plohishi.xaml
    /// </summary>
    public partial class Plohishi : Page
    {
        public ObservableCollection<Suspects> Suspectses { get; set; }
        public ObservableCollection<Crimes> Crimeses { get; set; }
        public ObservableCollection<Model.Cases> Caseses { get; set; }
        public ObservableCollection<CrimeType> CrimeTypes { get; set; }
        public ObservableCollection<Model.CaseType> CaseTypes { get; set; }
        public ObservableCollection<Model.CaseStatus> CaseStatuses { get; set; }
        public ObservableCollection<Model.Employee> Employees { get; set; }
        public Plohishi()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
        }
        private void LoadData()
        {
            using (var context = new BazaDan())
            {
                Suspectses = new ObservableCollection<Suspects>(context.Suspects.ToList());
                Crimeses = new ObservableCollection<Crimes>(context.Crimes.ToList());
                Caseses = new ObservableCollection<Model.Cases>(context.Cases.ToList());
                CrimeTypes = new ObservableCollection<CrimeType>(context.CrimeType.ToList());
                CaseTypes = new ObservableCollection<CaseType>(context.CaseType.ToList());
                CaseStatuses = new ObservableCollection<Model.CaseStatus>(context.CaseStatus.ToList());
                Employees = new ObservableCollection<Employee>(context.Employee.ToList());
            }
        }
        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySorting();
        }
        private void ApplySorting()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Suspectses);

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
        private void UpdateList_Click(object sender, RoutedEventArgs e)
        {
            Plohishi newAdminPage = new Plohishi();
            NavigationService.Navigate(newAdminPage);
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
                        Suspects dataItem = item as Suspects;

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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPlohisha addSotr = new AddPlohisha();
            bool? result = addSotr.ShowDialog();

            if (result == true)
            {
                LoadData();
            }
        }
        private void Excelcase_Click(object sender, RoutedEventArgs e)
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = excelApp.Workbooks.Add(XlSheetType.xlWorksheet);
            Worksheet worksheet = (Worksheet)excelApp.ActiveSheet;
            worksheet.Cells[1, 1] = "Имя";
            worksheet.Cells[1, 2] = "Фамилия";
            worksheet.Cells[1, 3] = "Отчество";
            worksheet.Cells[1, 4] = "Дата рождения";
            worksheet.Cells[1, 5] = "Номер телефона";
            worksheet.Cells[1, 6] = "Почта";
            worksheet.Cells[1, 7] = "Паспорт";
            worksheet.Cells[1, 8] = "Описание преступления";
            worksheet.Cells[1, 9] = "Город преступления";
            worksheet.Cells[1, 10] = "Место преступления";
            worksheet.Cells[1, 11] = "Дата преступления";
            worksheet.Cells[1, 12] = "Тип преступления";
            worksheet.Cells[1, 13] = "Код дела";
            worksheet.Cells[1, 14] = "Описание";
            worksheet.Cells[1, 15] = "Дата открытия дела";
            worksheet.Cells[1, 16] = "Дата закрытия дела";
            worksheet.Cells[1, 17] = "Тип дела";
            worksheet.Cells[1, 18] = "Cтатус дела";
            worksheet.Cells[1, 19] = "Сотрудник, открывший дело";
            int row = 2; 
            foreach (var suspect in Suspectses)
            {
                var crime = Crimeses.FirstOrDefault(c => c.IDCrime == suspect.IDCrime);
                var caseInfo = Caseses.FirstOrDefault(c => c.IDCase == suspect.IDCase);
                var crymetype = CrimeTypes.FirstOrDefault(c => c.IDCrimeType == crime.IDCrimeType);
                var employee = Employees.FirstOrDefault(r => r.IDEmployee == caseInfo.IDEmployee);
                var caseType = CaseTypes.FirstOrDefault(r => r.IDCaseType == caseInfo.IDCaseType);
                var caseStatus = CaseStatuses.FirstOrDefault(r => r.IDCaseStatus == caseInfo.IDCaseStatus);
                worksheet.Cells[row, 1] = suspect.FirstName;
                worksheet.Cells[row, 2] = suspect.LastName;
                worksheet.Cells[row, 3] = suspect.LastestName;
                worksheet.Cells[row, 4] = suspect.DateOfBirth.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 5] = suspect.NumberPhone;
                worksheet.Cells[row, 6] = suspect.Email;
                worksheet.Cells[row, 7] = suspect.PasportaSeria + " " + suspect.PasportNumber;
                worksheet.Cells[row, 8] = crime.DescriptionCrime;
                worksheet.Cells[row, 9] = crime.CrimeCity;
                worksheet.Cells[row, 10] = crime.CrimeLocation;
                worksheet.Cells[row, 11] = crime.CrimeDateTime?.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 12] = crymetype.CrimeType1;
                worksheet.Cells[row, 13] = caseInfo.CaseCode;
                worksheet.Cells[row, 14] = caseInfo.Description;
                worksheet.Cells[row, 15] = caseInfo.OpenDate?.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 16] = caseInfo.CloseDate?.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 17] = caseType?.CaseType1;
                worksheet.Cells[row, 18] = caseStatus?.CaseStatus1;
                worksheet.Cells[row, 19] = employee.FirstName + " " + employee.LastName + " " + employee.LastestName;
                row++;
            }
            try
            {
                string fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Плохиши.xlsx");
                workbook.SaveAs(fileName);
                excelApp.Quit();
                MessageBox.Show("Данные успешно экспортированы.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving Excel file: " + ex.Message);
            }
        }
    }
 }


