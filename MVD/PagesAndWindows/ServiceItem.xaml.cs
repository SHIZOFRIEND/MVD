using MVD.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для ServiceItem.xaml
    /// </summary>
    public partial class ServiceItem : Window
    {
        private Employee _employeeContext;
        private int _employeeId;
        public ServiceItem(Employee employeeContext, int employeeId)
        {
            InitializeComponent();
            _employeeContext = employeeContext;
            _employeeId = employeeId;
            LoadEntrancePasses();
            LoadVehicleTypes();
            LoadVehicleStatuses();
            LoadTrainingStatuses();
            LoadServiceItemData();
            LoadUnifromType();
            LoadWeaponTypes();
            LoadWeaponStatuses();
        }
        private void LoadServiceItemData()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var serviceItem = context.ServiceItems.FirstOrDefault(si => si.IDEmployee == _employeeId);
                    if (serviceItem != null)
                    {
                        cben.SelectedValue = serviceItem.IDEntrancePass;
                        var training = context.Training.FirstOrDefault(t => t.IDTraining == serviceItem.IDTraining);
                        var vehicle = context.Vehicle.FirstOrDefault(v => v.IDVehicle == serviceItem.IDVehicle);
                        var uniform = context.Unifrorm.FirstOrDefault(u => u.IDUniform == serviceItem.IDUniform);
                        var weapon = context.Weapon.FirstOrDefault(w => w.IDWeapon == serviceItem.IDWeapon);
                        if (training != null && vehicle != null)
                        {
                            txtTrainingName.Text = training.TrainingNmae;
                            txtDescription.Text = training.Description;
                            txtTrainingDate.Text = training.TrainingDate.ToString("yyyy-MM-dd");
                            txtTrainingCity.Text = training.TrainingCity;
                            txtTrainingLocation.Text = training.TrainingLocation;
                            txtNumberOfSeats.Text = training.NumberOfSeats.ToString();
                            txtDuration.Text = training.Duration;
                            cbtr.SelectedValue = training.IDTrainingStatus;
                            txtCarBrand.Text = vehicle.CarBrand;
                            txtModel.Text = vehicle.Model;
                            txtSerialNumber.Text = vehicle.SerialNumber;
                            txtCarNumber.Text = vehicle.CarNumber;
                            txtCarRegionNumber.Text = vehicle.CarRegionNumber.ToString();
                            cbVehicleType.SelectedValue = vehicle.IDVehicleType;
                            cbVehicleStatus.SelectedValue = vehicle.IDVehicleStatus;
                            txtcarnumberseats.Text = vehicle.NumberOfPlace;
                            txtcardate.Text = vehicle.DateOfIssues.ToString("yyyy-MM-dd");
                            txtUniformDescription.Text = uniform.Description;
                            txtUniformSerialNumber.Text = uniform.SerialNumber;
                            txtUniformDateOfIssue.Text = uniform.DateOfIssues.ToString("yyyy-MM-dd");
                            cbUniformType.SelectedValue = uniform.IDTypeUniform;
                            txtWeaponDescription.Text = weapon.Description;
                            txtWeaponSerialNumber.Text = weapon.SerialNumber;
                            txtWeaponDateOfIssue.Text = weapon.DateOfIssues.ToString("yyyy-MM-dd");
                            cbWeaponType.SelectedValue = weapon.IDWeaponType;
                            cbWeaponStatus.SelectedValue = weapon.IDWeaponStatus;
                        }
                        else
                        {
                            MessageBox.Show("Тренинг, информация о транспортном средстве, униформа или оружие не найдены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Служебный инвентарь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadWeaponTypes()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var weaponTypes = context.WeaponType.ToList();
                    cbWeaponType.ItemsSource = weaponTypes;
                    cbWeaponType.DisplayMemberPath = "WeaponType1";
                    cbWeaponType.SelectedValuePath = "IDWeaponType";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке типов оружия: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadWeaponStatuses()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var weaponStatuses = context.WeaponStatus.ToList();
                    cbWeaponStatus.ItemsSource = weaponStatuses;
                    cbWeaponStatus.DisplayMemberPath = "WeaponStatus1";
                    cbWeaponStatus.SelectedValuePath = "IDWeaponStatus";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статусов оружия: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadEntrancePasses()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var entrancePasses = context.EntrancePass.ToList();
                    cben.ItemsSource = entrancePasses;
                    cben.DisplayMemberPath = "StatusEntrancePass";
                    cben.SelectedValuePath = "IDEntrancePass";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пропусков: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadUnifromType()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var uniformTypes = context.TypeUniform.ToList();
                    cbUniformType.ItemsSource = uniformTypes;
                    cbUniformType.DisplayMemberPath = "TypeUniform1";
                    cbUniformType.SelectedValuePath = "IDTypeUniform";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке типов униформы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadTrainingStatuses()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var trainingStatuses = context.TrainingStatus.ToList();
                    cbtr.ItemsSource = trainingStatuses;
                    cbtr.DisplayMemberPath = "TrainingStatus1";
                    cbtr.SelectedValuePath = "IDTrainingStatus";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статусов тренинга: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadVehicleTypes()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var vehicleTypes = context.VehicleType.ToList();
                    cbVehicleType.ItemsSource = vehicleTypes;
                    cbVehicleType.DisplayMemberPath = "VehicleType1"; 
                    cbVehicleType.SelectedValuePath = "IDVehicleType";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке типов транспортных средств: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadVehicleStatuses()
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var vehicleStatuses = context.VehicleStatus.ToList();
                    cbVehicleStatus.ItemsSource = vehicleStatuses;
                    cbVehicleStatus.DisplayMemberPath = "VehicleStatus1";
                    cbVehicleStatus.SelectedValuePath = "IDVehicleStatus";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статусов транспортных средств: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void cben_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cbtr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cbUniformType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new BazaDan())
                {
                    var serviceItem = context.ServiceItems.FirstOrDefault(si => si.IDEmployee == _employeeId);
                    if (serviceItem == null)
                    {
                        serviceItem = new ServiceItems{ IDEmployee = _employeeId };
                        context.ServiceItems.Add(serviceItem);
                    }
                    serviceItem.IDEntrancePass = (int)cben.SelectedValue;
                    var training = context.Training.FirstOrDefault(t => t.IDTraining == serviceItem.IDTraining);
                    if (training == null)
                    {
                        training = new Training();
                        context.Training.Add(training);
                    }
                    training.TrainingNmae = txtTrainingName.Text;
                    training.Description = txtDescription.Text;
                    training.TrainingDate = DateTime.ParseExact(txtTrainingDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    training.TrainingCity = txtTrainingCity.Text;
                    training.TrainingLocation = txtTrainingLocation.Text;
                    training.NumberOfSeats = int.Parse(txtNumberOfSeats.Text);
                    training.Duration = txtDuration.Text;
                    training.IDTrainingStatus = (int)cbtr.SelectedValue;
                    var vehicle = context.Vehicle.FirstOrDefault(v => v.IDVehicle == serviceItem.IDVehicle);
                    if (vehicle == null)
                    {
                        vehicle = new Vehicle();
                        context.Vehicle.Add(vehicle);
                    }
                    vehicle.CarBrand = txtCarBrand.Text;
                    vehicle.Model = txtModel.Text;
                    vehicle.SerialNumber = txtSerialNumber.Text;
                    vehicle.CarNumber = txtCarNumber.Text;
                    vehicle.CarRegionNumber = int.Parse(txtCarRegionNumber.Text);
                    vehicle.IDVehicleType = (int)cbVehicleType.SelectedValue;
                    vehicle.IDVehicleStatus = (int)cbVehicleStatus.SelectedValue;
                    vehicle.NumberOfPlace = txtNumberOfSeats.Text;
                    vehicle.DateOfIssues = DateTime.ParseExact(txtcardate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    var uniform = context.Unifrorm.FirstOrDefault(u => u.IDUniform == serviceItem.IDUniform);
                    if (uniform == null)
                    {
                        uniform = new Unifrorm();
                        context.Unifrorm.Add(uniform);
                    }
                    uniform.Description = txtUniformDescription.Text;
                    uniform.SerialNumber = txtUniformSerialNumber.Text;
                    uniform.DateOfIssues = DateTime.ParseExact(txtUniformDateOfIssue.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    uniform.IDTypeUniform = (int)cbUniformType.SelectedValue;
                    var weapon = context.Weapon.FirstOrDefault(w => w.IDWeapon == serviceItem.IDWeapon);
                    if (weapon == null)
                    {
                        weapon = new Weapon();
                        context.Weapon.Add(weapon);
                    }
                    weapon.Description = txtWeaponDescription.Text;
                    weapon.SerialNumber = txtWeaponSerialNumber.Text;
                    weapon.DateOfIssues = DateTime.ParseExact(txtWeaponDateOfIssue.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    weapon.IDWeaponType = (int)cbWeaponType.SelectedValue;
                    weapon.IDWeaponStatus = (int)cbWeaponStatus.SelectedValue;
                    context.SaveChanges();            
                    MessageBox.Show("Изменения успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
