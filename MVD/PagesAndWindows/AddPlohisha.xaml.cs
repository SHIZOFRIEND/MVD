using HashPassword;
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
using System.Windows.Shapes;

namespace MVD.PagesAndWindows
{
    /// <summary>
    /// Логика взаимодействия для AddPlohisha.xaml
    /// </summary>
    public partial class AddPlohisha : Window
    {
        public AddPlohisha()
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
           string.IsNullOrWhiteSpace(txtPochta.Text))
            {
                MessageBox.Show("Заполните все обязательные поля перед сохранением.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int idPolzovatelia = 0;
            try
            {
                using (var context = new BazaDan())
                {
                    Suspects newSotrydnik = new Suspects
                    {

                        FirstName = txtImya.Text,
                        LastName = txtfamilia.Text,
                        LastestName = txtotchestvo.Text,
                        NumberPhone = txtNumberPhone.Text,
                        Email = txtPochta.Text,
                        IDSuspect = idPolzovatelia,
                        PasportaSeria = txtseria.Text,
                        PasportNumber = txtnomerpasporta.Text,
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
                    context.Suspects.Add(newSotrydnik);
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
            imgPhoto.Source = null;
            txtdata.Text = string.Empty;
        }
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
