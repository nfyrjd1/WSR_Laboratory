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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WSR_Laboratory.Model;
using WSR_Laboratory.Service;

namespace WSR_Laboratory.View
{
    /// <summary>
    /// Логика взаимодействия для PatientPage.xaml
    /// </summary>
    public partial class PatientPage : Page
    {
        public PatientPage()
        {
            InitializeComponent();
            UpdatePatients();
        }

        private void UpdatePatients()
        {
            List<patient> Patients = Laboratory.GetContext().patient.ToList();
            if (!string.IsNullOrWhiteSpace(SearchTB.Text))
            {
                string Search = SearchTB.Text.ToLower();

                Patients = Patients.Where(p =>
                Levenshtein.Distance(p.full_name.ToLower(), Search) == 3 ||
                p.email.ToLower().Contains(Search) ||
                (p.ein != null && p.ein.ToLower().Contains(Search)) ||
                p.phone.ToLower().Contains(Search) ||
                p.passport.ToLower().Contains(Search) ||
                p.birthdayStr.ToLower().Contains(Search) ||
                (p.country != null && p.country.ToLower().Contains(Search)) ||
                p.social_number.ToString().Contains(Search) ||
                p.social_type.name.Contains(Search) ||

                p.insurance.name.ToLower().Contains(Search) ||
                p.insurance.inn.ToString().Contains(Search) ||
                p.insurance.address.ToLower().Contains(Search) ||
                p.insurance.bik.ToString().Contains(Search) ||
                p.insurance.payment_account.ToString().Contains(Search)
                ).ToList();
            }
            PatientDG.ItemsSource = Patients;
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePatients();
        }

        private void PatientDGRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as DataGridRow).DataContext != null) {
                patient patient = (sender as DataGridRow).DataContext as patient;
                Manager.MainFrame.Navigate(new AddEditPatient(patient));
            }
        }

        private void PatientBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPatient());
        }
    }
}
