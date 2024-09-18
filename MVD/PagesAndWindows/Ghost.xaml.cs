using HashPassword;
using Microsoft.Office.Interop.Word;
using MVD.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Page = System.Windows.Controls.Page;
using System.IO;
namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для Ghost.xaml
    /// </summary>
    public partial class Ghost : Page
    {
        public Ghost()
        {
            InitializeComponent();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImya.Text) ||
          string.IsNullOrWhiteSpace(txtfamilia.Text) ||
          string.IsNullOrWhiteSpace(txtotchestvo.Text) ||
          string.IsNullOrWhiteSpace(txtNumberPhone.Text) ||
          string.IsNullOrWhiteSpace(txtseria.Text) ||
          string.IsNullOrWhiteSpace(txtnomerpasporta.Text) ||
          string.IsNullOrWhiteSpace(txtstatment.Text) ||
          string.IsNullOrWhiteSpace(txtdata.Text))
            {
                MessageBox.Show("Заполните все обязательные поля перед сохранением.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int idclient = 0;
            try
            {
                using (var context = new BazaDan())
                {
                    ForClient newSotrydnik = new ForClient
                    {
                        FirstName = txtImya.Text,
                        LastName = txtfamilia.Text,
                        LastestName = txtotchestvo.Text,
                        NumberPhone = txtNumberPhone.Text,
                        IDClient = idclient,
                        PasportaSeria = txtseria.Text,
                        PasportNumber = txtnomerpasporta.Text,
                        Statment = txtstatment.Text,
                        DateOfBirth = Convert.ToDateTime(txtdata.Text),
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
                    context.ForClient.Add(newSotrydnik);
                    context.SaveChanges();
                    MessageBox.Show("Заявление успешно добавлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new Auto());
                    string seriaPasporta = txtseria.Text;
                    string familia = txtfamilia.Text;
                    string imya = txtImya.Text;
                    string otchestvo = txtotchestvo.Text;
                    string nomerpasporta = txtnomerpasporta.Text;
                    string statment = txtstatment.Text;
                    Dogovor(seriaPasporta, familia, imya, otchestvo, nomerpasporta, statment);
                }
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
            txtstatment.Text = string.Empty;
            txtnomerpasporta.Text = string.Empty;
            txtseria.Text = string.Empty;
            txtdata.Text = string.Empty;
        }
        private void Dogovor(string seriaPasporta, string familia, string imya, string otchestvo, string nomerpasporta, string statment)
        {
            var items = new Dictionary<string, string>()
            {
                {"<gorod>", "Новосибирск"},
                {"<currentdate>", DateTime.Now.ToString("dd.MM.yyyy")},
                {"<address>", "Серебренниковская 40"},
                {"<seria>", seriaPasporta},
                {"<imya>", imya},
                {"<fam>", familia},
                {"<mvd>", "ГУ МВД России по Новосибирску"},
                {"<otchwestvo>", otchestvo},
                {"<nomerpasp>", nomerpasporta},
                {"<statments>", statment}
            };
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document wordDoc = null;
            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                object missing = System.Reflection.Missing.Value;
                string fileName = @"C:\Users\Egor\source\repos\MVD\MVD\Documents\AcceptanceProtocol.docx";
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
                    Range myRange = wordDoc.Content;
                    myRange.Find.ClearFormatting();
                    myRange.Find.Execute(FindText: findText, ReplaceWith: replaceText, Replace: WdReplace.wdReplaceAll);
                }
                string newFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Протокол принятия устного заявления о преступлении.docx");
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
