using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WSR_Laboratory_Mobile.Model
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Birthday { get; set; }

        [NotMapped]
        public string BirthdayStr
        {
            get
            {
                if (Birthday == null)
                {
                    return "Не указано";
                }

                return Birthday.Value.ToShortDateString();
            }
        }

        public string Phone { get; set; }

        [NotMapped]
        public string PhoneStr
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Phone))
                {
                    return "Не указано";
                }

                return Phone;
            }
        }

        public string Email { get; set; }

        [NotMapped]
        public string EmailStr
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    return "Не указано";
                }

                return Email;
            }
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Passport { get; set; }

        [NotMapped]
        public string PassportStr
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Passport))
                {
                    return "Не указано";
                }

                return Passport;
            }
        }

        public string Insurance { get; set; }

        [NotMapped]
        public string InsuranceStr
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Insurance))
                {
                    return "Не указано";
                }

                return Insurance;
            }
        }

        [NotMapped]
        public string Age { 
            get
            {
                if (Birthday == null)
                {
                    return "Не указано";
                }

                return ((int)((DateTime.Now - Birthday.Value).TotalDays / 365)).ToString();
            }
        }

        public bool? IsCurrentUser { get; set; }
    }
}
