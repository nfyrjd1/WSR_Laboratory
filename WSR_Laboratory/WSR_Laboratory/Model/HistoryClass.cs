using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSR_Laboratory.Model
{
    public partial class history
    {
        public string status {
            get {
                return has_entered ? "Успешно" : "Не успешно";
            }
        }
    }
}
