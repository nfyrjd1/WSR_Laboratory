//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WSR_Laboratory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public patient()
        {
            this.blood = new HashSet<blood>();
        }
    
        public int id_patient { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string ein { get; set; }
        public string phone { get; set; }
        public decimal passport_series { get; set; }
        public decimal passport_number { get; set; }
        public System.DateTime birthday { get; set; }
        public string country { get; set; }
        public decimal social_number { get; set; }
        public bool social_type { get; set; }
        public int id_user { get; set; }
        public int id_insurance { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<blood> blood { get; set; }
        public virtual insurance insurance { get; set; }
        public virtual user user { get; set; }
    }
}
