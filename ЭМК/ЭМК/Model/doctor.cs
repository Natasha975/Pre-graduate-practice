//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ЭМК.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class doctor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public doctor()
        {
            this.inspection = new HashSet<inspection>();
        }
    
        public int id_doctor { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string surname { get; set; }
        public int id_specialization { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    
        public virtual specialization specialization { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inspection> inspection { get; set; }
    }
}
