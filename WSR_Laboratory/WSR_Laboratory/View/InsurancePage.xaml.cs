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

namespace WSR_Laboratory.View
{
    /// <summary>
    /// Логика взаимодействия для InsurancePage.xaml
    /// </summary>
    public partial class InsurancePage : Page
    {
        public InsurancePage()
        {
            InitializeComponent();
            ReportDate1.SelectedDate = DateTime.Now.AddYears(-10);
            ReportDate2.SelectedDate = DateTime.Now;
            InsuranceDG.ItemsSource = Laboratory.GetContext().insurance.ToList();
        }

        private bool ValidateDate()
        {
            if (string.IsNullOrWhiteSpace(ReportDate1.Text) 
                || !DateTime.TryParse(ReportDate1.Text, out DateTime dateTime1) 
                || ReportDate1.SelectedDate > ReportDate2.SelectedDate
                || string.IsNullOrWhiteSpace(ReportDate2.Text)
                || !DateTime.TryParse(ReportDate2.Text, out DateTime dateTime2))
            {
                MessageBox.Show("Неверный формат периода отчёта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void ReportPdfBtn_Click(object sender, RoutedEventArgs e)
        {
            insurance insurance = (sender as Button).DataContext as insurance;

            if (ValidateDate())
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("WSR_Laboratory.Resources.InsuranceHtml.html");
                StreamReader reader = new StreamReader(stream);
                string Html = reader.ReadToEnd();

                Html = Html.Replace("@Date", $"{ReportDate1.Text} - {ReportDate2.Text}");
                Html = Html.Replace("@Name", insurance.name);

                string content = "";

                List<patient> patients = Laboratory.GetContext().patient.Where(p => p.id_insurance == insurance.id_insurance
                && p.blood.Any(t => t.date_create > ReportDate1.SelectedDate && t.date_create < ReportDate2.SelectedDate)).ToList();
                double totalSum = 0;
                foreach (patient patient in patients)
                {
                    content += $"<tr><td><strong>ФИО пациента:</strong></td><td><strong>{patient.full_name}</strong></td></tr>";
                    content += "<tr><td>Услуга</td><td>Цена</td></tr>";
                    foreach (blood blood in patient.blood)
                    {
                        foreach (blood_service service in blood.blood_service)
                        {
                            content += $"<tr><td>{service.service.name}</td><td>{service.service.price}руб</td></tr>";
                            totalSum += (double)service.service.price;
                        }
                    }
                }
                Html = Html.Replace("@Content", content);
                Html = Html.Replace("@Sum", $"{totalSum}руб");

                FileStream pdfDest = File.Open($"Счёт страховой компании {insurance.name}.pdf", FileMode.Create);
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(Html, pdfDest, converterProperties);
                Process.Start($"Счёт страховой компании {insurance.name}.pdf");
            }
        }

        private void ReportCsvBtn_Click(object sender, RoutedEventArgs e)
        {
            insurance insurance = (sender as Button).DataContext as insurance;
            if (ValidateDate())
            {
                string Csv = $"Период для оплаты:;{ReportDate1.Text} - {ReportDate2.Text}\r\n" +
                             $"Страховая компания:;{insurance.name}\r\n" +
                             $"Счёт:\r\n";
                List<patient> patients = Laboratory.GetContext().patient.Where(p => p.id_insurance == insurance.id_insurance
                && p.blood.Any(t => t.date_create > ReportDate1.SelectedDate && t.date_create < ReportDate2.SelectedDate)).ToList();
                double totalSum = 0;
                foreach (patient patient in patients)
                {
                    Csv += $"ФИО пациента:;{patient.full_name}\r\n";
                    Csv += $"Услуга;Цена;\r\n";
                    foreach (blood blood in patient.blood)
                    {
                        foreach (blood_service service in blood.blood_service)
                        {
                            Csv += $"{service.service.name};{service.service.price}руб;\r\n";
                            totalSum += (double)service.service.price;
                        }
                    }
                }

                Csv += $"Итого:;{totalSum}руб";

                File.WriteAllText($"Счёт страховой компании {insurance.name}.csv", Csv, Encoding.UTF8);
                Process.Start($"Счёт страховой компании {insurance.name}.csv");
            }
        }
    }
}
