using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSR_Laboratory.Model
{
    public partial class blood
    {
        public string date
        {
            get
            {
                return date_create.ToShortDateString();
            }
        }

        public decimal price
        {
            get
            {
                decimal price = 0;
                foreach (blood_service service in blood_service)
                {
                    price += service.service.price;
                }

                return price;
            }
        }
    }
}
