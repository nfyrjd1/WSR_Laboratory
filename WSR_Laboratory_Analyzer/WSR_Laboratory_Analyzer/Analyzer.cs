using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSR_Laboratory_Analyzer
{
    public class Order
    {
        public string[] Services { get; set; }
        public DateTime DateTimeOrder { get; set; }
    }

    public class Analyzer
    {
        int AnalyzeTime = 30;
        string AnalyzerName = "Ledetect";
        string[] AllowedServices = new string[]
        {
            "TSH", "Амилаза", "Креатинин", "Кальций общий", "Глюкоза", "Общий белок"
        };
        Order CurrentOrder = null;

        public string Get(string AnalyzerName = "")
        {
            if (AnalyzerName != this.AnalyzerName)
            {
                return $"Ошибка 400. Analyzer with name '{AnalyzerName}' not found.";
            }

            if (CurrentOrder != null)
            {
                if (CurrentOrder.DateTimeOrder.AddSeconds(AnalyzeTime * CurrentOrder.Services.Length) < DateTime.Now)
                {
                    Random random = new Random();
                    string Message = "Статус 200. ";
                    foreach (string service in CurrentOrder.Services)
                    {
                        Message += $"Исследование: {service}, результат: {random.NextDouble()}; ";
                    }
                    CurrentOrder = null;
                    return Message;
                } else
                {
                    return $"Ошибка 400. Analyzer with name '{AnalyzerName}' not found.";
                }
            }

            return "Ошибка 400. Analyzer is not working.";
        }

        public string Post(string AnalyzerName = "", string[] Services = null)
        {
            if (AnalyzerName != this.AnalyzerName)
            {
                return $"Ошибка 400. Analyzer with name '{AnalyzerName}' not found.";
            }

            if (Services == null || Services.Any(p => !AllowedServices.Contains(p)))
            {
                return "Ошибка 400. Analyzer can not do this order. May be order contains services which analyzer does not support.";
            }

            if (CurrentOrder != null)
            {
                return "Ошибка 400. Analyzer is busy.";
            }

            CurrentOrder = new Order()
            {
                DateTimeOrder = DateTime.Now,
                Services = Services
            };

            return "Статус 200. Analyzer is working.";
        }
    }
}
