using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSR_Laboratory.Model
{
    public partial class analyzer
    {
        public string status
        {
            get
            {
                if (analyzer_blood_service.Any(p => p.blood_service.status != null && p.blood_service.status.name == "Отправлена на исследование"))
                {
                    return "Работает";
                }

                return "Свободен";
            }
        }
    }
}
