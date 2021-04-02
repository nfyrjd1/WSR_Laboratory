using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using WSR_Laboratory.Model;

namespace WSR_Laboratory.Service
{
    public class AnalyzerManager
    {
        public static Timer Timer;

        public static void StartTimer()
        {
            Timer = new Timer();
            Timer.Interval = 15000;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        public static void StopTimer()
        {
            if (Timer != null)
            {
                Timer.Stop();
            }            
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Get();
        }

        public static void Get()
        {
            Timer.Stop();

            List<analyzer> analyzers = Laboratory.GetContext().analyzer.ToList();
            foreach (analyzer analyzer in analyzers)
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                webClient.DownloadStringAsync(new Uri($"http://localhost:5000/api/analyzer/{analyzer.name}"));
                webClient.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) => {
                    WebClient_DownloadStringCompleted(sender, e);
                };
                System.Threading.Thread.Sleep(1000);
            }            

            Timer.Start();
        }

        private static void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Timer.Stop();

            if (e.Error == null)
            {
                BloodServiceJson Response = JsonConvert.DeserializeObject<BloodServiceJson>(e.Result);
                if (Response.patient == null)
                {
                    Timer.Start();
                    return;
                }
                int code = Response.services.Last().serviceCode;
                object result = Response.services.Last().result;
                decimal patient = decimal.Parse(Response.patient);

                service service = Laboratory.GetContext().service.Where(p => p.code == code).ToList().LastOrDefault();
                if (service != null)
                {
                    blood_service blood = Laboratory.GetContext().blood_service.Where(p => p.id_service == service.id_service && p.blood.id_patient == patient && (p.status == null || p.status.name == "Отправлена на исследование")).FirstOrDefault();
                    if (blood != null)
                    {
                        bool IsError = false;
                        MessageBoxResult Result = MessageBoxResult.No;
                        if (blood.analyzer_blood_service.Count > 0)
                        {
                            blood.analyzer_blood_service.LastOrDefault().date_finished = DateTime.Now;
                            blood.analyzer_blood_service.LastOrDefault().analyze_time_sec = DateTime.Now.Subtract(blood.analyzer_blood_service.LastOrDefault().date_reception).Seconds;
                        }

                        result = double.Parse(result.ToString());

                        if (blood.service.result_type == "Integer")
                        {
                            double Average = 0;
                            List<blood_service> services = Laboratory.GetContext().blood_service.Where(p => p.status.name == "Выполнено" && p.service.result_type == "Integer" && p.result != null).ToList();
                            if (services.Count > 0)
                            {
                                Average = services.Average(p => double.Parse(p.result.Replace(".", ",")));
                            }

                            if (Average > 0 && (Average > (double)result * 5 || Average < (double)result / 5))
                            {
                                IsError = true;
                                Result = MessageBox.Show($"Обнаружено отклонение от среднего результата исследований, возможнем сбой анализатора или некачественный биоматериал. Подтвердите результаты исследования:" +
                                    $"\r\nУслуга: {blood.service.name} " +
                                    $"\r\nПациент: {blood.blood.patient.full_name} " +
                                    $"\r\nРезультат: {result}", "Необходимо подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                            }
                        }
                        else
                        {
                            Result = MessageBox.Show($"Подтвердите результаты исследования: " +
                                $"\r\nУслуга: {blood.service.name} " +
                                $"\r\nПациент: {blood.blood.patient.full_name} " +
                                $"\r\nРезультат: {result}", "Необходимо подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                        }

                        blood.date_finished = DateTime.Now;

                        if (Result == MessageBoxResult.Yes)
                        {
                            blood.accepted = true;
                            blood.result = result.ToString();
                            blood.status = Laboratory.GetContext().status.ToList().Find(p => p.name == "Выполнено");
                        }
                        else
                        {
                            blood.accepted = false;

                            if (IsError)
                            {
                                blood.status = Laboratory.GetContext().status.ToList().Find(p => p.name == "Необходим повторный забор биоматериала");
                            }
                            else
                            {
                                blood.status = Laboratory.GetContext().status.ToList().Find(p => p.name == "Отклонено");
                            }
                        }

                        if (blood.blood.blood_service.All(p => p.status != null && p.status.name == "Отклонено"))
                        {
                            blood.blood.status = Laboratory.GetContext().status.ToList().Find(p => p.name == "Отклонено");
                            blood.blood.analyze_time_days = DateTime.Now.Subtract(blood.blood.date_create).Days;
                        }

                        if (blood.blood.blood_service.All(p => p.status != null && p.status.name == "Выполнено"))
                        {
                            blood.blood.status = Laboratory.GetContext().status.ToList().Find(p => p.name == "Выполнено");
                            blood.blood.analyze_time_days = DateTime.Now.Subtract(blood.blood.date_create).Days;
                        }

                        Laboratory.GetContext().SaveChanges();

                        Laboratory.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                        blood.percent = 100;
                    }
                }
            }

            Timer.Start();
        }
    }
}
