using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MVD.Model;

namespace MVD.PagesAndWindows
{
    public partial class AboutCase : Window
    {
        private Model.Cases _caseContext;
        private int _caseId;
        private int someEmployeeId;
        public AboutCase(Model.Cases currentCase, int caseId)
        {
            InitializeComponent();
            _caseContext = currentCase;
            _caseId = caseId;
            LoadData();
        }
        private async void LoadData()
        {
            await LoadEvidType();
            await LoadInvestigationType();
            await LoadCrimeTypes();
            await LoadInvestigationStatus();
            await PopulateFieldsFromDatabase();
        }
        private async Task PopulateFieldsFromDatabase()
        {
            using (var dbContext = new BazaDan())
            {
                await PopulateField<Evidence>(dbContext, e => e.IDCase == _caseId, evidence =>
                {
                    txtDescription.DataContext = evidence;
                    txtLocation.DataContext = evidence;
                    txtCollectionDate.DataContext = evidence;
                    cbEvidenceType.SelectedValue = evidence.IDEvidenceType;
                });
                await PopulateField<Suspects>(dbContext, s => s.IDCase == _caseId, suspect =>
                {
                    txtSuspectFirstName.DataContext = suspect;
                    txtSuspectLastName.DataContext = suspect;
                    txtSuspectLastestName.DataContext = suspect;
                    txtSuspectDateOfBirth.DataContext = suspect;
                    txtSuspectNumberPhone.DataContext = suspect;
                    txtSuspectEmail.DataContext = suspect;
                    txtSuspectPasportaSeria.DataContext = suspect;
                    txtSuspectPasportNumber.DataContext = suspect;
                });
                await PopulateField<Arrest>(dbContext, a => a.IDCase == _caseId, arrest =>
                {
                    txtArrestDescription.DataContext = arrest;
                    txtArrestDateTime.DataContext = arrest;
                    txtArrestCity.DataContext = arrest;
                    txtArrestLocation.DataContext = arrest;
                });
                await PopulateField<Witnesses>(dbContext, w => w.IDCase == _caseId, witness =>
                {
                    txtFirstName.DataContext = witness;
                    txtLastName.DataContext = witness;
                    txtLastestName.DataContext = witness;
                    txtDateOfBirth.DataContext = witness;
                    txtNumberPhone.DataContext = witness;
                    txtEmail.DataContext = witness;
                    txtPasportaSeria.DataContext = witness;
                    txtPasportNumber.DataContext = witness;
                });
                await PopulateField<Crimes>(dbContext, c => c.IDCase == _caseId, crime =>
                {
                    txtCrimeCity.DataContext = crime;
                    txtCrimeDateTime.DataContext = crime;
                    txtcrimeloc.DataContext = crime;
                    txtaboutcrime.DataContext = crime;
                    cbCrimeType.SelectedValue = crime.IDCrimeType;
                });
                await PopulateField<Investigation>(dbContext, i => i.IDCase == _caseId, investigation =>
                {
                    txtInvestigationTitle.DataContext = investigation;
                    txtInvestigationDescription.DataContext = investigation;
                    txtStartDate.DataContext = investigation;
                    txtEndDate.DataContext = investigation;
                    cbInvestigationStatus.SelectedValue = investigation.IDInvestigationStatus;
                    cbInvestigationType.SelectedValue = investigation.IDInvestigationType;
                });
            }
        }
        private async Task PopulateField<T>(BazaDan dbContext, Func<T, bool> predicate, Action<T> setDataContext) where T : class
        {
            var entity = await Task.Run(() => dbContext.Set<T>().FirstOrDefault(predicate));
            if (entity != null)
            {
                setDataContext(entity);
            }
        }
        private async Task LoadComboBoxData<T>(ComboBox comboBox, Func<BazaDan, IQueryable<T>> query, string displayMember, string valueMember)
        {
            using (var context = new BazaDan())
            {
                var items = await Task.Run(() => query(context).ToList());
                comboBox.ItemsSource = items;
                comboBox.DisplayMemberPath = displayMember;
                comboBox.SelectedValuePath = valueMember;
            }
        }
        private Task LoadEvidType() => LoadComboBoxData(cbEvidenceType, context => context.EvidenceType, "EvidenceType1", "IDEvidenceType");
        private Task LoadInvestigationType() => LoadComboBoxData(cbInvestigationType, context => context.InvestigationType, "InvestigationType1", "IDInvestigationType");
        private Task LoadInvestigationStatus() => LoadComboBoxData(cbInvestigationStatus, context => context.InvestigationStatus, "InvestigationStatus1", "IDInvestigationStatus");
        private Task LoadCrimeTypes() => LoadComboBoxData(cbCrimeType, context => context.CrimeType, "CrimeType1", "IDCrimeType");
        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dbContext = new BazaDan())
                {
                    var caseExists = await dbContext.Cases.AnyAsync(c => c.IDCase == _caseId);
                    if (!caseExists)
                    {
                        MessageBox.Show("Дело с таким ID не существует.");
                        return;
                    }

                    await SaveChangesAsync();
                    MessageBox.Show("Изменения успешно сохранены!");
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
            }
        }
        private async Task SaveChangesAsync()
        {
            await SaveEntityChanges<Evidence>(dbContext => dbContext.Evidence.FirstOrDefault(ev => ev.IDCase == _caseId),
                () => new Evidence { IDCase = _caseId },
                evidence =>
                {
                    evidence.DescriptionEvidence = txtDescription.Text;
                    evidence.EvidenceLocation = txtLocation.Text;
                    evidence.CollectionDate = Convert.ToDateTime(txtCollectionDate.Text);
                    evidence.IDEvidenceType = (int)cbEvidenceType.SelectedValue;
                });
            await SaveEntityChanges<Suspects>(dbContext => dbContext.Suspects.FirstOrDefault(s => s.IDCase == _caseId),
                () => new Suspects { IDCase = _caseId },
                suspect =>
                {
                    suspect.FirstName = txtSuspectFirstName.Text;
                    suspect.LastName = txtSuspectLastName.Text;
                    suspect.LastestName = txtSuspectLastestName.Text;
                    suspect.DateOfBirth = Convert.ToDateTime(txtSuspectDateOfBirth.Text);
                    suspect.NumberPhone = txtSuspectNumberPhone.Text;
                    suspect.Email = txtSuspectEmail.Text;
                    suspect.PasportaSeria = txtSuspectPasportaSeria.Text;
                    suspect.PasportNumber = txtSuspectPasportNumber.Text;
                });
            await SaveEntityChanges<Arrest>(dbContext => dbContext.Arrest.FirstOrDefault(a => a.IDCase == _caseId),
                () => new Arrest { IDCase = _caseId },
                arrest =>
                {
                    arrest.ArrestDateTime = Convert.ToDateTime(txtArrestDateTime.Text);
                    arrest.ArrestCity = txtArrestCity.Text;
                    arrest.ArrestLocation = txtArrestLocation.Text;
                    arrest.DecriptionArrest = txtArrestDescription.Text;
                    
                    arrest.IDEmployee = GetExistingEmployeeId();
                });
            await SaveEntityChanges<Witnesses>(dbContext => dbContext.Witnesses.FirstOrDefault(w => w.IDCase == _caseId),
                () => new Witnesses { IDCase = _caseId },
                witness =>
                {
                    witness.FirstName = txtFirstName.Text;
                    witness.LastName = txtLastName.Text;
                    witness.LastestName = txtLastestName.Text;
                    witness.DateOfBirth = Convert.ToDateTime(txtDateOfBirth.Text);
                    witness.NumberPhone = txtNumberPhone.Text;
                    witness.Email = txtEmail.Text;
                    witness.PasportaSeria = txtPasportaSeria.Text;
                    witness.PasportNumber = txtPasportNumber.Text;
                });
            await SaveEntityChanges<Crimes>(dbContext => dbContext.Crimes.FirstOrDefault(c => c.IDCase == _caseId),
                () => new Crimes { IDCase = _caseId },
                crime =>
                {
                    crime.CrimeCity = txtCrimeCity.Text;
                    crime.CrimeDateTime = Convert.ToDateTime(txtCrimeDateTime.Text);
                    crime.CrimeLocation = txtcrimeloc.Text;
                    crime.DescriptionCrime = txtaboutcrime.Text;
                    crime.IDCrimeType = (int)cbCrimeType.SelectedValue;
                });
            await SaveEntityChanges<Investigation>(dbContext => dbContext.Investigation.FirstOrDefault(i => i.IDCase == _caseId),
                () => new Investigation { IDCase = _caseId },
                investigation =>
                {
                    investigation.Title = txtInvestigationTitle.Text;
                    investigation.DescriptionInvestigation = txtInvestigationDescription.Text;
                    investigation.StartDate = Convert.ToDateTime(txtStartDate.Text);
                    investigation.EndDate = Convert.ToDateTime(txtEndDate.Text);
                    investigation.IDEmployee = GetExistingEmployeeId();
                    investigation.IDInvestigationStatus = (int)cbInvestigationStatus.SelectedValue;
                    investigation.IDInvestigationType = (int)cbInvestigationType.SelectedValue;
                });
        }
        private async Task SaveEntityChanges<T>(Func<BazaDan, T> findEntity, Func<T> createEntity, Action<T> updateEntity) where T : class
        {
            using (var dbContext = new BazaDan())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var entity = await Task.Run(() => findEntity(dbContext));
                        if (entity == null)
                        {
                            entity = createEntity();
                            dbContext.Set<T>().Add(entity);
                        }
                        updateEntity(entity);
                        await Task.Run(() => dbContext.SaveChanges());

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
                        throw;
                    }
                }
            }
        }
        private int GetExistingEmployeeId()
        {
            using (var dbContext = new BazaDan())
            {
               
                return dbContext.Employee.Select(emp => emp.IDEmployee).FirstOrDefault();
            }
        }
        private void cbEvidenceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
