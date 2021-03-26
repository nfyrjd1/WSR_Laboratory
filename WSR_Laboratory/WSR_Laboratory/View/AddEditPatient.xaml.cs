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
    /// Логика взаимодействия для AddEditPatient.xaml
    /// </summary>
    public partial class AddEditPatient : Page
    {
        patient current;
        ModalWindow Modal;
        public AddEditPatient(patient patient = null, ModalWindow Modal = null)
        {
            InitializeComponent();
            if (Modal != null)
            {
                this.Modal = Modal;
                BackBtn.Visibility = Visibility.Collapsed;
            }

            InsuranceCB.ItemsSource = Laboratory.GetContext().insurance.ToList();
            SocialTypeCB.ItemsSource = Laboratory.GetContext().social_type.ToList();

            if (patient == null)
            {
                patient = new patient();
                patient.birthday = DateTime.Now;
            }
            current = patient;
            DataContext = current;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder Errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(current.full_name))
            {
                Errors.AppendLine("Введите ФИО");
            }

            if (current.passport_series <= 0)
            {
                Errors.AppendLine("Введите серию паспорта");
            }

            if (current.passport_number <= 0)
            {
                Errors.AppendLine("Введите номер паспорта");
            }

            if (string.IsNullOrWhiteSpace(current.email))
            {
                Errors.AppendLine("Введите email");
            }

            if (string.IsNullOrWhiteSpace(current.phone))
            {
                Errors.AppendLine("Введите телефон");
            }

            DateTime Birthday = DateTime.Now;
            if (string.IsNullOrWhiteSpace(BirthdayDP.Text) || !DateTime.TryParse(BirthdayDP.Text, out Birthday))
            {
                Errors.AppendLine("Введите дату рождения");
            }

            if (current.social_number <= 0)
            {
                Errors.AppendLine("Введите номер страхового полиса");
            }

            if (current.social_type == null)
            {
                Errors.AppendLine("Выберите тип страхового полиса");
            }

            if (current.insurance == null)
            {
                Errors.AppendLine("Выберите страховую компанию");
            }

            if (Errors.Length > 0)
            {
                MessageBox.Show(Errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            current.birthday = Birthday;

            if (current.id_patient == 0)
            {
                Laboratory.GetContext().patient.Add(current);
            }
            Laboratory.GetContext().SaveChanges();
            Laboratory.GetContext().ChangeTracker.Entries<patient>().Where(p => p.Entity.id_patient == current.id_patient).ToList().ForEach(p => p.Reload());

            if (Modal != null)
            {
                Modal.Close();
                return;
            }

            Manager.MainFrame.GoBack();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
