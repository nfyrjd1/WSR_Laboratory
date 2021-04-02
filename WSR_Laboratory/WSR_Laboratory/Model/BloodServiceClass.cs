using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace WSR_Laboratory.Model
{
    public class ServiceJson
    {
        public int serviceCode { get; set; }

        public object result { get; set; }
    }

    public class BloodServiceJson
    {
        public string patient { get; set; }

        public List<ServiceJson> services { get; set; }
    }

    public partial class blood_service
    {
        public BloodServiceJson BloodServiceJson
        {
            get
            {
                BloodServiceJson bloodService = new BloodServiceJson();
                bloodService.patient = blood.id_patient.ToString();
                bloodService.services = new List<ServiceJson>();

                ServiceJson serviceJson = new ServiceJson();
                serviceJson.serviceCode = Convert.ToInt32(service.code);
                bloodService.services.Add(serviceJson);

                return bloodService;
            }
        }

        public string statusStr
        {
            get
            {
                if (status != null)
                {
                    return status.name;
                }

                return "Не исследовано";
            }
        }

        public int percent { get; set; }

        public string percentStr
        {
            get
            {
                return percent + "%";
            }
        }

        public Visibility LoaderVisibility
        {
            get
            {
               if (status != null && status.name == "Отправлена на исследование")
               {
                    return Visibility.Visible;
               }

               return Visibility.Collapsed;
            }
        }

        public Visibility ResearchVisibility
        {
            get
            {
                if (status != null && status.name == "Отправлена на исследование")
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
        }

        public bool ResearchEnabled { get; set; }
    }
}
