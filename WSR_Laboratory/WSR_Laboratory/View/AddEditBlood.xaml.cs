using iText.Html2pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для AddEditBlood.xaml
    /// </summary>
    public partial class AddEditBlood : Page
    {
        blood current = null;
        List<patient> Patients = new List<patient>();
        List<service> Services = new List<service>();
        bool IsNewBlood = false;

        public AddEditBlood(blood blood)
        {
            InitializeComponent();
            Patients = Laboratory.GetContext().patient.ToList();
            Services = Laboratory.GetContext().service.ToList();
            current = blood;
            BloodTB.IsReadOnly = true;
            DataContext = current;
        }

        public AddEditBlood()
        {
            InitializeComponent();
            Patients = Laboratory.GetContext().patient.ToList();
            Services = Laboratory.GetContext().service.ToList();
            current = new blood();
            IsNewBlood = true;
            current.id_blood = Laboratory.GetContext().blood.Max(p => p.id_blood) + 1;
            DataContext = current;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder Errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(BarcodeTB.Text))
            {
                Errors.AppendLine("Введите код пробирки");
            }

            if (!decimal.TryParse(BarcodeTB.Text, out decimal barcode) || barcode <= 0)
            {
                Errors.AppendLine("Неверный формат кода пробирки");
            }

            if (string.IsNullOrWhiteSpace(BloodTB.Text))
            {
                Errors.AppendLine("Введите номер заказа");
            }

            if (!int.TryParse(BloodTB.Text, out int id_blood) || id_blood <= 0)
            {
                Errors.AppendLine("Неверный формат номера заказа");
            }

            if (IsNewBlood)
            {
                blood copy = Laboratory.GetContext().blood.Find(id_blood);
                if (copy != null)
                {
                    Errors.AppendLine("Введённый код пробирки уже сохранён в базе данных");
                }
            }

            patient patient = Patients.Find(p => p.full_name == PatientTB.Text);
            if (patient == null)
            {
                Errors.AppendLine("Выберите пациента");
            }

            if (current.blood_service.Count == 0)
            {
                Errors.AppendLine("Добавьте услуги");
            }

            if (current.blood_service.Any(p => p.service == null))
            {
                Errors.AppendLine("Выберите услуги");
            }

            if (Errors.Length > 0)
            {
                MessageBox.Show(Errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            current.patient = patient;
            current.id_blood = id_blood;
            current.barcode = barcode;
            if (IsNewBlood)
            {
                current.date_create = DateTime.Now;
                Laboratory.GetContext().blood.Add(current);
            }
            Laboratory.GetContext().SaveChanges();
            Laboratory.GetContext().ChangeTracker.Entries<blood>().Where(p => p.Entity.id_blood == current.id_blood).ToList().ForEach(p => p.Reload());

            MessageBoxResult Result = MessageBox.Show("Сформировать электронный вид заказа?", "Необходимо подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("WSR_Laboratory.Resources.BloodHtml.html");
                StreamReader reader = new StreamReader(stream);
                string Html = reader.ReadToEnd();
                Html = Html.Replace("@Date", current.date);
                Html = Html.Replace("@IdBlood", current.id_blood.ToString());
                Html = Html.Replace("@Barcode", current.barcode.ToString());
                Html = Html.Replace("@SocianNumber", current.patient.social_number.ToString());
                Html = Html.Replace("@FullName", current.patient.full_name);
                Html = Html.Replace("@Birthday", current.patient.birthdayStr);
                string ServiceHtml = "";
                foreach(blood_service service in current.blood_service)
                {
                    ServiceHtml += $"<tr><td>{service.service.name}</td><td>${service.service.price}</td></tr>";
                }
                Html = Html.Replace("@Services", ServiceHtml);
                Html = Html.Replace("@ServiceSum", current.price.ToString());

                string Link = $"дата_заказа={current.date}T{current.date_create.ToLongTimeString()}&номер_заказа={current.id_blood}";
                Html = Html.Replace("@Link", $@"https://wsrussia.ru/?data=base64({Convert.ToBase64String(Encoding.UTF8.GetBytes(Link))})");

                FileStream pdfDest = File.Open($"Заказ{id_blood}.pdf", FileMode.Create);

                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(Html, pdfDest, converterProperties);
                Process.Start($"Заказ{id_blood}.pdf");
            }

            Manager.MainFrame.GoBack();
        }

        private void BarcodeTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                current.barcode = current.id_blood;
                BarcodeTB.Text = current.barcode.ToString();
            }
        }

        private void BarcodeBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder Errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(BarcodeTB.Text))
            {
                Errors.AppendLine("Введите код пробирки");
            }

            if (!decimal.TryParse(BarcodeTB.Text, out decimal barcodeDec) || barcodeDec == 0)
            {
                Errors.AppendLine("Неверный формат кода пробирки");
            }

            if (string.IsNullOrWhiteSpace(BloodTB.Text))
            {
                Errors.AppendLine("Введите номер заказа");
            }

            if (!int.TryParse(BloodTB.Text, out int id_blood) || id_blood == 0)
            {
                Errors.AppendLine("Неверный формат номера заказа");
            }

            if (Errors.Length > 0)
            {
                MessageBox.Show(Errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string date = DateTime.Now.ToShortDateString();
            if (!IsNewBlood)
            {
                date = current.date;
            }
            date = date.Replace(".", "");
            string code = BarcodeTB.Text;
            string blood = BloodTB.Text;

            string barcode = $"{blood}{date}{code}";
            MessageBoxResult Result = MessageBox.Show(barcode, "Формирование штрих-кода", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (Result == MessageBoxResult.Yes)
            {
                /*string html = "";
                for (int i = 0; i < barcode.Length; i++) {
                    int number = int.Parse(barcode[i].ToString());
                    string color = number == 0 ? "white" : "black";
                    double width = number == 0 ? 1.35 : 0.15 * number;
                    html += $"<div style=\"height: 25.93mm; margin-left: 0.2mm; display:inline-block; background-color:{color}; width: {width}mm\"></div> \r\n";
                }

                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("WSR_Laboratory.Resources.EmptyHtml.html");
                StreamReader reader = new StreamReader(stream);
                string Html = reader.ReadToEnd();
                html = $"<div style=\"\">{html}</div>";
                Html = Html.Replace("@Content", html);
                FileStream pdfDest = File.Open($"Штрих-код {barcode}.pdf", FileMode.Create);
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(Html, pdfDest, converterProperties);
                Process.Start($"Штрих-код {barcode}.pdf");*/
            }
        }

        private void PatientTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            patient patient = Patients.Find(p => p.full_name == (sender as TextBox).Text);
            if (patient == null)
            {
                PatientErrorText.Visibility = Visibility.Visible;
                PatientSuccessText.Visibility = Visibility.Collapsed;
            }
            else
            {
                PatientErrorText.Visibility = Visibility.Collapsed;
                PatientSuccessText.Visibility = Visibility.Visible;
            }
        }

        private void ServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            blood_service service = new blood_service();
            service.blood = current;
            current.blood_service.Add(service);

            ServicesDG.Items.Refresh();
        }

        private void DeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (((sender as Button).Parent as Grid) != null)
            {
                current.blood_service.Remove(((sender as Button).Parent as Grid).DataContext as blood_service);
                ServicesDG.Items.Refresh();
            }            
        }

        private void ServiceCB_Initialized(object sender, EventArgs e)
        {
            (sender as ComboBox).ItemsSource = Services;
        }

        private void ReadBarcodeBtn_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            int blood = rnd.Next(1, int.MaxValue);
            int code = rnd.Next(100000, 999999);
            string date = DateTime.Now.ToShortDateString().Replace(".", "");

            string barcode = $"{blood}{date}{code}";
            MessageBoxResult Result = MessageBox.Show(barcode, "Считывание штрих-кода пробирки", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (Result == MessageBoxResult.Yes)
            {
                BarcodeTB.Text = code.ToString();
                BloodTB.Text = blood.ToString();
            }
        }

        private void PatientLink_Click(object sender, RoutedEventArgs e)
        {
            ModalWindow modal = new ModalWindow();
            modal.ModalFrame.Navigate(new AddEditPatient(null, modal));
            if (modal.ShowDialog() == true)
            {
                Patients = Laboratory.GetContext().patient.ToList();
                PatientTB_TextChanged(PatientTB, null);
            }
        }
    }
}
