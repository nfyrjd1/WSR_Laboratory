using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
    /// Логика взаимодействия для ServiceReportPage.xaml
    /// </summary>
    public partial class ServiceReportPage : Page
    {
        List<blood_service> Services = new List<blood_service>();
        public ServiceReportPage()
        {
            InitializeComponent();
            ReportDP1.SelectedDate = DateTime.Now.AddYears(-10);
            ReportDP2.SelectedDate = DateTime.Now;

            ReportTypeCB.ItemsSource = new string[] { 
                "Количество пациентов",
                "Средний результат исследований",
            };
            ReportTypeCB.SelectedItem = "Количество пациентов";

            ServiceCB.ItemsSource = Laboratory.GetContext().service.ToList();
            ServiceCB.SelectedItem = ServiceCB.Items[0];
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            ExportModalWindow exportModal = new ExportModalWindow(ReportCountDG.ItemsSource as List<blood_service>, ServiceCB.SelectedItem as string, "Chart.png");
            exportModal.ShowDialog();
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateDate())
            {
                switch (ReportTypeCB.SelectedItem) {
                    case "Количество пациентов":
                        {
                            ReportCountDG.Visibility = Visibility.Visible;
                            ReportAverageDG.Visibility = Visibility.Collapsed;

                            List<blood> bloods = Laboratory.GetContext().blood.Where(p => p.date_create >= ReportDP1.SelectedDate
                            && p.date_create <= ReportDP2.SelectedDate
                            && p.status.name == "Выполнено").ToList();

                            List<blood_service> services = new List<blood_service>();
                            bloods.ForEach(p => {
                                p.blood_service.ToList().ForEach(t => {
                                    blood_service blood_Service = services.Find(x => x.blood.date_create.ToShortDateString() == p.date_create.ToShortDateString()
                                    && x.id_service == t.id_service);
                                    if (blood_Service == null)
                                    {
                                        t.PatientsCount = 1;
                                        services.Add(t);
                                    } else
                                    {
                                        if (blood_Service.blood.id_patient != p.id_patient)
                                        {
                                            blood_Service.PatientsCount += 1;
                                        }                                        
                                    }
                                });
                            });

                            services = services.OrderBy(p => p.service.name).OrderBy(p => p.blood.date_create).ToList();
                            ReportCountDG.ItemsSource = services;
                            Services = services;

                            service service = ServiceCB.SelectedItem as service;

                            List<double> ys = new List<double>();
                            List<double> xs = new List<double>();

                            Services.ForEach(p =>
                            {
                                if (p.id_service == service.id_service)
                                {
                                    xs.Add(p.blood.date_create.ToOADate());
                                    ys.Add(p.PatientsCount);
                                }
                            });
                            Chart.plt.Clear();

                            if (xs.Count > 0)
                            {
                                try
                                {
                                    Chart.plt.PlotScatter(xs.ToArray(), ys.ToArray());
                                    Chart.plt.Ticks(dateTimeX: true, dateTimeFormatStringX: "dd.MM.yyyy");
                                    Chart.plt.Title($"{service.name}");
                                    Chart.plt.SaveFig("Chart.png");
                                    Chart.Render();
                                }
                                catch (ArgumentException) {
                                    Chart.plt.Clear();
                                    Chart.Render();
                                }
                            };

                            break;
                        }
                    case "Средний результат исследований":
                        {
                            ReportCountDG.Visibility = Visibility.Collapsed;
                            ReportAverageDG.Visibility = Visibility.Visible;

                            List<blood> bloods = Laboratory.GetContext().blood.Where(p => p.date_create > ReportDP1.SelectedDate
                            && p.date_create < ReportDP2.SelectedDate
                            && p.status.name == "Выполнено" 
                            && p.blood_service.Any(t => t.service.result_type == "Integer" && !string.IsNullOrEmpty(t.result))).ToList();

                            List<blood_service> services = new List<blood_service>();
                            bloods.ForEach(p => {
                                p.blood_service.ToList().ForEach(t => {
                                    if (t.service.result_type != "Integer")
                                    {
                                        return;
                                    }

                                    blood_service blood_Service = services.Find(x => x.blood.date_create.ToShortDateString() == p.date_create.ToShortDateString()
                                    && x.id_service == t.id_service);
                                    if (blood_Service == null)
                                    {
                                        t.ResultCount = 1;
                                        double.TryParse(t.result.Replace(".", ","), out double result);
                                        t.ResultSum = result;
                                        services.Add(t);
                                    }
                                    else
                                    {
                                        blood_Service.ResultCount += 1;
                                        double.TryParse(t.result, out double result);
                                        blood_Service.ResultSum += result;
                                    }
                                });
                            });

                            ReportAverageDG.ItemsSource = services.OrderBy(p => p.service.name).OrderBy(p => p.blood.date_create);

                            Services = services;

                            service service = ServiceCB.SelectedItem as service;

                            List<double> ys = new List<double>();
                            List<double> xs = new List<double>();

                            Services.ForEach(p =>
                            {
                                if (p.id_service == service.id_service)
                                {
                                    xs.Add(p.blood.date_create.ToOADate());
                                    ys.Add(p.AverageResult);
                                }
                            });
                            Chart.plt.Clear();

                            if (xs.Count > 0)
                            {
                                try
                                {
                                    Chart.plt.PlotScatter(xs.ToArray(), ys.ToArray());
                                    Chart.plt.Ticks(dateTimeX: true, dateTimeFormatStringX: "dd.MM.yyyy");
                                    Chart.plt.Title($"{service.name}");
                                    Chart.plt.SaveFig("Chart.png");
                                    Chart.Render();
                                }
                                catch (ArgumentException)
                                {
                                    Chart.plt.Clear();
                                    Chart.Render();
                                }
                            };
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Не выбран тип отчёта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                }
            }
        }

        private bool ValidateDate()
        {
            if (string.IsNullOrWhiteSpace(ReportDP1.Text)
                || !DateTime.TryParse(ReportDP1.Text, out DateTime dateTime1)
                || ReportDP1.SelectedDate > ReportDP2.SelectedDate
                || string.IsNullOrWhiteSpace(ReportDP2.Text)
                || !DateTime.TryParse(ReportDP2.Text, out DateTime dateTime2))
            {
                MessageBox.Show("Неверный формат периода отчёта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void ServiceCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            service service = ServiceCB.SelectedItem as service;

            List<double> ys = new List<double>();
            List<double> xs = new List<double>();

            Services.ForEach(p =>
            {
                if (p.id_service == service.id_service)
                {
                    xs.Add(p.blood.date_create.ToOADate());
                    ys.Add(p.PatientsCount);
                }
            });
            Chart.plt.Clear();
            if (xs.Count > 0)
            {
                try
                {
                    Chart.plt.PlotScatter(xs.ToArray(), ys.ToArray());
                    Chart.plt.Ticks(dateTimeX: true, dateTimeFormatStringX: "dd.MM.yyyy");
                    Chart.plt.Title($"{service.name}");
                    Chart.plt.SaveFig("Chart.png");
                    Chart.Render();
                }
                catch (ArgumentException) { }
            }     
        }
    }
}
