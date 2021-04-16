using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WSR_Laboratory_Mobile.Model
{
    public class Service
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        [NotMapped]
        public string PriceStr
        {
            get
            {
                return $"{Price} руб";
            }
        }
    }
}
