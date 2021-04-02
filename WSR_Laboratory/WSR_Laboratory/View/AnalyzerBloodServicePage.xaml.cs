using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Логика взаимодействия для AnalyzerBloodServicePage.xaml
    /// </summary>
    public partial class AnalyzerBloodServicePage : Page
    {
        Timer UpdateTimer;
        Timer PercentTimer;
        analyzer CurrentAnalyzer;
        public AnalyzerBloodServicePage(analyzer Analyzer)
        {
            InitializeComponent();
            CurrentAnalyzer = Analyzer;

            Update();

            UpdateTimer = new Timer();
            UpdateTimer.Interval = 8000;
            UpdateTimer.Elapsed += UpdateTimer_Elapsed;
            UpdateTimer.Start();

            PercentTimer = new Timer();
            PercentTimer.Interval = 1000;
            PercentTimer.Elapsed += PercentTimer_Elapsed;
        }

        private void PercentTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                List<blood_service> blood_service = Laboratory.GetContext().blood_service.ToList().Where(p => p.status != null && p.status.name == "Отправлена на исследование").ToList();

                if (blood_service.Count > 0)
                {
                    foreach (blood_service blood in blood_service)
                    {
                        blood.percent += 5;

                        if (blood.percent >= 95)
                        {
                            PercentTimer.Stop();
                        }

                        Dispatcher.Invoke(() => {
                            AnalyzerBloodServiceDG.Items.Refresh();
                        });
                    }                    
                }
            } catch
            {

            }
            
        }

        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(Update);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void Update()
        {
            try
            {
                List<blood_service> Blood = Laboratory.GetContext().blood_service.Where(
                p => (p.status == null || p.status.name == "Отправлена на исследование") &&
                p.service.analyzer_service.Any(t => t.id_analyzer == CurrentAnalyzer.id_analyzer)
            ).ToList();

                if (Blood.Any(p => p.status != null && p.status.name == "Отправлена на исследование"
                && p.analyzer_blood_service.Any(t => t.id_analyzer == CurrentAnalyzer.id_analyzer && t.date_finished == null)))
                {
                    Blood.ForEach(p => p.ResearchEnabled = false);
                }
                else
                {
                    Blood.ForEach(p => p.ResearchEnabled = true);
                }

                AnalyzerBloodServiceDG.ItemsSource = Blood;
            }
            catch { }
        }

        private void ResearchBtn_Click(object sender, RoutedEventArgs e)
        {
            blood_service blood = (sender as Button).DataContext as blood_service;

            if (blood != null)
            {
                Post(blood, CurrentAnalyzer);
            }            
        }

        public void Post(blood_service blood, analyzer CurrentAnalyzer)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string post = JsonConvert.SerializeObject(blood.BloodServiceJson);
            webClient.UploadStringCompleted += (object sender, UploadStringCompletedEventArgs e) => {
                WebClient_UploadStringCompleted(sender, e, blood);
            };
            webClient.UploadStringAsync(new Uri($"http://localhost:5000/api/analyzer/{CurrentAnalyzer.name}"), post);
        }

        private void WebClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e, blood_service blood)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Данный анализатор занят");
                return;
            }

            analyzer_blood_service analyzer_Blood = blood.analyzer_blood_service.LastOrDefault();
            if (analyzer_Blood == null)
            {
                analyzer_Blood = new analyzer_blood_service();
                analyzer_Blood.analyzer = CurrentAnalyzer;
                analyzer_Blood.date_reception = DateTime.Now;
                blood.analyzer_blood_service.Add(analyzer_Blood);
            }

            blood.status = Laboratory.GetContext().status.ToList().Find(p => p.name == "Отправлена на исследование");
            blood.blood.status = Laboratory.GetContext().status.ToList().Find(p => p.name == "Отправлена на исследование");
            Laboratory.GetContext().SaveChanges();


            Update();

            PercentTimer.Start();
        }
    }
}
