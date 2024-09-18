//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVD.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Suspects
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Suspects()
        {
            this.Arrest = new HashSet<Arrest>();
        }
    
        public int IDSuspect { get; set; }
        public Nullable<int> IDCase { get; set; }
        public Nullable<int> IDCrime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastestName { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string NumberPhone { get; set; }
        public string Email { get; set; }
        public string PasportaSeria { get; set; }
        public string PasportNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Arrest> Arrest { get; set; }
        public virtual Cases Cases { get; set; }
        public virtual Crimes Crimes { get; set; }
    }
}
