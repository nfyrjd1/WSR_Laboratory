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
using System.Windows.Shapes;
using WSR_Laboratory.Model;

namespace WSR_Laboratory.View
{
    /// <summary>
    /// Логика взаимодействия для ExportModalWindow.xaml
    /// </summary>
    public partial class ExportModalWindow : Window
    {
        List<blood_service> Services = new List<blood_service>();
        string ReportType = "";
        string ChartPath = "";
        public ExportModalWindow(List<blood_service> services, string reportType, string chartPath)
        {
            InitializeComponent();
            Services = services;
            ReportType = reportType;
            ChartPath = chartPath;
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TableCB.IsChecked == false && ChartCB.IsChecked == false)
            {
                MessageBox.Show("Вы не выбрали параметры экспорта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Services.Count > 0)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("WSR_Laboratory.Resources.ServiceReportHtml.html");
                StreamReader reader = new StreamReader(stream);
                string Html = reader.ReadToEnd();

                if (TableCB.IsChecked == true)
                {
                    string table = 
                        "<table>" +
                            "<thead>" +
                                "<tr>" +
                                    "<td>Дата</td>" +
                                    "<td>Услуга</td>" +
                                    $"<td>{ReportType}</td>" +
                                "</tr>" +
                            "</thead>" +
                            "<tbody>" +
                                "@Services" +
                            "</tbody>" +
                        "</table >";

                    Html = Html.Replace("@Table", table);
                    string services = "";
                    Services.ForEach(p => {
                        double res = ReportType == "Количество пациентов" ? p.PatientsCount : p.AverageResult;
                        services += $"<tr><td>{p.blood.date}</td><td>{p.service.name}</td><td>{res}</td></tr>\r\n";
                    });

                    Html = Html.Replace("@Services", services);
                } else
                {
                    Html = Html.Replace("@Table", "");
                }

                if (ChartCB.IsChecked == true && File.Exists(ChartPath))
                {
                    string chart = $"<img src=\"{ChartPath}\"/>";
                    Html = Html.Replace("@Chart", chart);
                }
                else
                {
                    Html = Html.Replace("@Chart", "");
                }

                FileStream pdfDest = File.Open($"Отчёт по услугам.pdf", FileMode.Create);
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(Html, pdfDest, converterProperties);
                Process.Start($"Отчёт по услугам.pdf");
                Close();
            }
        }
    }
}
