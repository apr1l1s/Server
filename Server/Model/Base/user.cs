//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Server.Model.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            this.teachers_workload = new HashSet<teachers_workload>();
        }
    
        public int user_id { get; set; }
        public string login { get; set; }
        public string pass { get; set; }
        public int prof_id { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string middlename { get; set; }
    
        public virtual profession profession { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<teachers_workload> teachers_workload { get; set; }
    }
}