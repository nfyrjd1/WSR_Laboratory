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
    
    public partial class blood_service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public blood_service()
        {
            this.analyzer_blood_service = new HashSet<analyzer_blood_service>();
        }
    
        public int id_blood_service { get; set; }
        public int id_blood { get; set; }
        public int id_service { get; set; }
        public int id_status { get; set; }
        public Nullable<System.DateTime> date_finished { get; set; }
        public Nullable<decimal> result { get; set; }
        public Nullable<bool> accepted { get; set; }
        public int id_employee { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<analyzer_blood_service> analyzer_blood_service { get; set; }
        public virtual blood blood { get; set; }
        public virtual employee employee { get; set; }
        public virtual service service { get; set; }
        public virtual status status { get; set; }
        public virtual status status1 { get; set; }
    }
}
