using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSR_Laboratory.Model
{
    public partial class patient
    {
        public string birthdayStr
        {
            get
            {
                return birthday.ToShortDateString();
            }
        }

        public string passport
        {
            get
            {
                return $"{passport_series} {passport_number}";
            }
        }
    }
}
